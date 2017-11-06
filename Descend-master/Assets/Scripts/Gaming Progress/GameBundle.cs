/*
 * Created by Zehua Chen on 9/28/2017
 * 
 * The following items follows camel Casing
 * - Parameter Names
 * - Private Variables
 * 
 * The following items follows Pascal Casing
 * - Public Functions
 * - Public Properties
 */

using UnityEngine;
using System.Collections;

using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

/// <summary>
/// Game Folder class represents a folder on the disk
/// </summary>
public class GameFolder
{
    protected string _folderPathOnDisk;

    /// <summary>
    /// The location of the folder on the disk
    /// </summary>
    /// <value>the location</value>
    public string Location
    {
        get
        {
            return _folderPathOnDisk;
        }

    }

    //Instance methods

    //Private Functons

    /// <summary>
    /// Create a empty document on the disk in the folder. You can choose to either have a dot for extension.
    /// If you don't the function will create one.
    /// </summary>
    /// <param name="documentName">Document name.</param>
    /// <param name="extension">Extension.</param>
    protected void CreateDocument(string documentName, string extension)
    {
        if (!extension.StartsWith("."))
        {
            extension = "." + extension;
        }
        documentName += extension;

        string documentPathOnDisk = Path.Combine(Location, documentName);

        //Debug.Log(documentPathOnDisk);

        if (!File.Exists(documentPathOnDisk))
        {
            Debug.Log("Creating File");
            using (FileStream fs = new FileStream(documentPathOnDisk, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Flush();
                    Debug.Log("File Created");
                }
            }
        }
    }



    /// <summary>
    /// Create a Text Document in the folder.
    /// </summary>
    /// <param name="documentName">Document name.</param>
	protected void CreateTextDocument(string documentName)
    {
        CreateDocument(documentName, ".txt");
    }

    /// <summary>
    /// Create a child folder
    /// </summary>
    /// <param name="folderName">the name of the folder</param>
    protected void CreateFolder(string folderName)
    {
        string folderPathOnDisk = Path.Combine(Location, folderName);

        Directory.CreateDirectory(folderPathOnDisk);

        //Debug.Log(folderPathOnDisk);
    }

    //Public Functions

    public GameFolder(string path)
    {
        _folderPathOnDisk = path;
    }

    /// <summary>
    /// Check if a child folder with a specified name exist in the folder.
    /// </summary>
    /// <returns><c>true</c>, if folder exists, <c>false</c> if the folder does not exist.</returns>
    /// <param name="folderName">the name of the folder</param>
    public bool DoesFolderExist(string folderName)
    {
        string folderPathOnDisk = Path.Combine(this.Location, folderName);
        return Directory.Exists(folderPathOnDisk);

    }


    /// <summary>
    /// Opens a child folder in this folder, if no such folder exists, create one
    /// </summary>
    /// <returns>The folder.</returns>
    /// <param name="folderName">Folder name.</param>
    public virtual GameFolder OpenFolder(string folderName)
    {
        string folderPathOnDisk = Path.Combine(this._folderPathOnDisk, folderName);

        if (!Directory.Exists(folderPathOnDisk))
        {
            CreateFolder(folderName);
            return new GameFolder(folderPathOnDisk);
        }
        else
        {
            return new GameFolder(folderPathOnDisk);
        }
    }

    /// <summary>
    /// See if a file exist
    /// </summary>
    /// <returns><c>true</c>, if file exist<c>false</c> otherwise false.</returns>
    /// <param name="document">Document.</param>
    /// <param name="extension">Extension.</param>
    public bool DoesFileExist(string document, string extension)
    {
        string fullName = document + extension;
        string filePathOnDisk = Path.Combine(this.Location, fullName);



        return File.Exists(filePathOnDisk);
    }

    /// <summary>
    /// Doeses the xml file exist.
    /// </summary>
    /// <returns><c>true</c>, if xml file exist <c>false</c> otherwisefalse.</returns>
    /// <param name="documentName">Document name.</param>
    public virtual bool DoesXmlFileExist(string documentName)
    {
        return DoesFileExist(documentName, ".xml");
    }


    /// <summary>
    /// Create a XmlDocument in the folder.
    /// </summary>
    /// <param name="documentName">Document name.</param>
    public virtual XmlDocument CreateXmlDocument(string documentName)
    {
        XmlDocument xmlDoc = new XmlDocument();

        return xmlDoc;

    }

    /// <summary>
    /// Opens a Xml Document in the folder, if no such file exists, create one
    /// </summary>
    /// <returns>The xml document.</returns>
    /// <param name="documentName">Document name.</param>
    public virtual XmlDocument OpenXmlDocument(string documentName)
    {
        XmlDocument xmlDoc = new XmlDocument();

        if (!documentName.Contains(".xml"))
        {
            documentName += ".xml";
        }

        string documentPathOnDisk = Path.Combine(this.Location, documentName);

        if (File.Exists(documentPathOnDisk))
        {
            using (FileStream fs = new FileStream(documentPathOnDisk, FileMode.Open))
            {
                xmlDoc.Load(fs);
            }
            return xmlDoc;
        }
        else
        {
            return null;
        }

    }

    /// <summary>
    /// Serializes the data base to an xml file on disk. The file name should not have extension
    /// </summary>
    /// <param name="dataBase">Data base.</param>
    /// <param name="fileName">File name WITHOUT extension</param>
    public void SerializeDataBase(GameDataBase dataBase, string fileName)
    {
        XmlSerializer xs = new XmlSerializer(dataBase.GetType());
        fileName += ".xml";
        string filePathOnDisk = Path.Combine(this.Location, fileName);

        using (FileStream fs = new FileStream(filePathOnDisk, FileMode.OpenOrCreate))
        {
            xs.Serialize(fs, dataBase);
        }
    }

    /// <summary>
    /// Deserializes the data base from an xml file if there is such a file, other wise
    /// Initialize a new instance of GameDataBase of its child classes based on the Type passed
    /// </summary>
    /// <returns>The data base.</returns>
    /// <param name="fileName">File name.</param>
    /// <param name="dataBaseType">Data base type.</param>
    public GameDataBase DeserializeDataBase<T>(string fileName)
    {
        GameDataBase tempDataBase;
        if (this.DoesXmlFileExist(fileName)) {
            fileName += ".xml";
            string filePathOnDisk = Path.Combine(this.Location, fileName);
            XmlSerializer xs = new XmlSerializer(typeof(T));

            using (FileStream fs = new FileStream(filePathOnDisk, FileMode.OpenOrCreate))
            {
                tempDataBase = xs.Deserialize(fs) as GameDataBase;
            }
        }
        else 
        {
            tempDataBase = (GameDataBase)Activator.CreateInstance(typeof(T));
        }

        return tempDataBase;
    }

    public GameDataBase DeserializeDataBase<T>(string fileName, params object[] constructorParameters)
    {
        GameDataBase temp = DeserializeDataBase<T>(fileName);
        temp.Initialize(constructorParameters);
        return temp;
    }
}

/// <summary>
/// Game Bundle class provides
/// several methods for convinient loading and writing data to a Unity game's resource location.
/// </summary>
public class GameBundle : GameFolder
{
    /*
     * Variables
     */

    //Instance variables
    private GameFolder _playerFolder;
    private GameFolder _sceneFolders;

    /*
     * Properties
     */
    public GameFolder SceneFolders 
    {
        get 
        {
            return _sceneFolders;
        }
    }

    public GameFolder PlayerFolder 
    {
        get 
        {
            return _playerFolder;
        }
    }

    /*
     * Functions
     */

    // Static Functions

    /// <summary>
    /// Returns the bundle of the folder unity creates for this game.
    /// On Mac, it is located in ~/Library/Application Support/[Your Company]/[Your Game]/
    /// On Windows, it is ... (to be filled)
    /// </summary>
    /// <returns>a bundle initialized to the folder unity creates for this game</returns>
    public static GameBundle MainBundle
    {
        get
        {
            GameBundle bundle;
            string applicationDataPath = Application.dataPath;
            string gameBundlePath = Path.Combine(applicationDataPath, "Game Data");

            //Game bundle is located in a folder under Application.dataPath named "Game Data"
            //if the folder does not exist, create the folder, 
            //otherwise, place the folder in the "Game Data" folder
            if (!Directory.Exists(gameBundlePath)) {
                Directory.CreateDirectory(gameBundlePath);
            }

            bundle = new GameBundle(gameBundlePath);

            /*
             * Scenes is where you save your scenes
             * Player is where the persistant data of player is stored
             */
            bundle._sceneFolders = bundle.OpenFolder("Scenes");
            bundle._playerFolder = bundle.OpenFolder("Player");

			return bundle;
        }
    }

    /// <summary>
    /// Returns the bundle at a custom location. This may be useful for transfering user data?
    /// </summary>
    /// <returns>A bundle initialized to the custom folder for this game</returns>
    /// <param name="path">the path to intialize the bundle at</param>
    public static GameBundle CustomBundle(string path)
    {
        GameBundle bundle = new GameBundle(path);

        return bundle;
    }

    public GameFolder OpenSpecificSceneFolder (string sceneNameOnDisk) 
    {
        return this.SceneFolders.OpenFolder(sceneNameOnDisk);
    }

    public GameBundle (string path) : base(path)
    {

    }


}

public static class GameBundleFolderExtensionFunction 
{
    /// <summary>
    /// Saves to folder.
    /// </summary>
    /// <param name="xmldoc">Xmldoc.</param>
    /// <param name="folder">Folder.</param>
    public static void SaveToFolder(this XmlDocument xmlDoc, GameFolder folder, string name)
    {
        if (!name.Contains(".xml"))
        {
            name += ".xml";
        }
        string filePathOnDisk = Path.Combine(folder.Location, name);

        xmlDoc.Save(filePathOnDisk);
    }

    /// <summary>
    /// Save to UserData folder of a Game Bundle
    /// </summary>
    /// <param name="xmlDoc">Xml document.</param>
    /// <param name="bundle">Bundle.</param>
    public static void SaveToBundle(this XmlDocument xmlDoc, GameBundle bundle, string name)
    {
        SaveToFolder(xmlDoc, bundle, name);
    }
}