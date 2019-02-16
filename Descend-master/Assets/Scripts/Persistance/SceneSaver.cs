//
//  SceneSaver.cs
//  Pacman > Persistance
//
//  Created by Zehua Chen on 8/1/18.
//  Copyright © 2018 Zehua Chen. All rights reserved.
//
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void SceneSavingStartedHandler();
public delegate void SceneSavingEndedHandler();

/// <summary>
/// SceneSaver loads and saves data to xml file on the disk, while 
/// informing Savable objects of the data it loads and saves.
/// </summary>
public class SceneSaver : MonoBehaviour
{
    /// <summary>
    /// Name of the xml file to save the savables
    /// </summary>
    private static string SavablesFileName
    {
        get { return "savables.xml"; }
    }

    /// <summary>
    /// Root element tag of the xml file where savables are stored
    /// </summary>
    private static string SavablesDocumentElementTag
    {
        get { return "savables"; }
    }

    private static string SavablesContainerElementTag
    {
        get { return "container"; }
    }

    /// <summary>
    /// Path to the folder where UserData folder is at.
    /// </summary>
    public static string PathToPutUserDataFolder
    {
        get {
            return Application.dataPath;
        }
    }

    /// <summary>
    /// Name of the user data folder.
    /// </summary>
    public static string NameofUserDataFolder
    {
        get {
            return "UserData";
        }
    }
    
    private ISavable[] savables;

    private string scenePath = null;

    /// <summary>
    /// Triggered when the saving operation begins
    /// </summary>
    public event SceneSavingEndedHandler SceneSavingEnded;
    /// <summary>
    /// Triggered when the xml file has been flushed to disk
    /// </summary>
    public event SceneSavingStartedHandler SceneSavingStarted;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // Get Scene name
        string sceneName = SceneManager.GetActiveScene().name;

        // Make sure "User Data" exists
        string userDataPath = Path.Combine(
            SceneSaver.PathToPutUserDataFolder,
            SceneSaver.NameofUserDataFolder
        );
        EnsureExistance(userDataPath);

        // Make sure there is a folder for the scene
        scenePath = Path.Combine(userDataPath, sceneName);
        EnsureExistance(scenePath);

    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Load();
    }

    /// <summary> 
    /// Make sure a path exists on the disk.
    /// </summary>
    /// <param name="path">
    /// a path that must not be ""
    /// </param>
    protected void EnsureExistance(string path)
    {
        if(path == "") { return; }

        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    /// <summary> 
    /// Save all the savables
    /// </summary>
    /// <param name="blocking">
    /// whether the opeartion of flushing the xml file to the disk is blocking.
    /// </param>
    public void Save(bool blocking = true)
    {
        var savables = FindObjectsOfType<MonoBehaviour>().OfType<ISavable>();
        if(!savables.Any()) return;

        EnsureExistance(scenePath);

        if (SceneSavingStarted != null)
            SceneSavingStarted();

        string filePath = Path.Combine(scenePath, SceneSaver.SavablesFileName);

        XmlDocument doc = new XmlDocument();
        XmlElement rootElement = doc.CreateElement(SavablesDocumentElementTag);
        doc.AppendChild(rootElement);

        foreach(var savable in savables)
        {
            XmlElement containerElement = doc.CreateElement(SceneSaver.SavablesContainerElementTag);
            var attributes = containerElement.Attributes;

            var containerNameAttr = doc.CreateAttribute("name");
            containerNameAttr.InnerText = savable.ToString();

            attributes.Append(containerNameAttr);
            doc.DocumentElement.AppendChild(containerElement);

            XmlSavableStore store = new XmlSavableStore(doc, containerElement);
            savable.OnSave(store);

        }

        if(blocking) SaveXmlDocumentBlocking(doc, filePath);
        else SaveXmlDocumentNonBlocking(doc, filePath);

        if(SceneSavingEnded != null)
            SceneSavingEnded();
    }

    /// <summary> 
    /// Load all the savables
    /// </summary>
    public void Load()
    {
        var savables = FindObjectsOfType<MonoBehaviour>().OfType<ISavable>();
        if(!savables.Any()) return;

        EnsureExistance(scenePath);

        var filePath = Path.Combine(scenePath, SceneSaver.SavablesFileName);

        if(!File.Exists(filePath)) return;

        XmlDocument doc = new XmlDocument();
        doc.Load(filePath);

        Dictionary<string, XmlElement> containerElements = new Dictionary<string, XmlElement>();
        foreach(XmlElement container in doc.DocumentElement.ChildNodes)
        {
            string name = container.Attributes[0].InnerText;
            containerElements.Add(name, container);
        }
        
        foreach(var savable in savables)
        {
            XmlElement containerElement;
            if(containerElements.TryGetValue(savable.ToString(), out containerElement))
            {
                XmlSavableStore store = new XmlSavableStore(doc, containerElement);
                savable.OnLoad(store);
            }
        }

    }

    /// <summary>
    /// Delete the data of this scene
    /// </summary>
    public void DeleteSceneData()
    {
        if(scenePath == null)
        {
            scenePath = Path.Combine(
                SceneSaver.PathToPutUserDataFolder,
                SceneSaver.NameofUserDataFolder
            );
            
            string sceneName = SceneManager.GetActiveScene().name;
            scenePath = Path.Combine(scenePath, sceneName);
        }

        if(Directory.Exists(scenePath))
        {
            Directory.Delete(scenePath, true);
        }
    }

    /// <summary>
    /// Delete all data of the current user
    /// </summary>
    public static void DeleteAllData()
    {
        var userDataPath = Path.Combine(PathToPutUserDataFolder, NameofUserDataFolder);
        Directory.Delete(userDataPath, true);
    }

    /// <summary>
    /// Write the xml document to disk blocking the current thread
    /// </summary>
    /// <param name="doc">the xml document to save</param>
    /// <param name="filePath">the file path to save the xml document to</param>
    private void SaveXmlDocumentBlocking(XmlDocument doc, string filePath)
    {
        using(XmlWriter writer = XmlWriter.Create(filePath))
        {
            doc.Save(writer);
        }
    }

    /// <summary>
    /// Write the xml document to disk NOT blocking the current thread
    /// </summary>
    /// <param name="doc">the xml document to save</param>
    /// <param name="filePath">the file path to save the xml document to</param>
    private void SaveXmlDocumentNonBlocking(XmlDocument doc, string filePath)
    {
        using(XmlWriter writer = XmlWriter.Create(filePath))
        {
            doc.Save(writer);
        }
    }
}
