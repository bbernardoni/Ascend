//
//  SavableStore.cs
//  Pacman > Persistance
//
//  Created by Zehua Chen on 8/11/18.
//  Copyright © 2018 Zehua Chen. All rights reserved.
//
using System;
using System.Xml;
using UnityEngine;

public class XmlSavableStore: ISavableWriteStore, ISavableReadStore
{
    public XmlDocument document;
    public XmlElement containerElement;
    
    public object Document { get { return document; }  }
    public object Container { get { return containerElement; }  }
    
    public XmlSavableStore(XmlDocument document, XmlElement containerElement)
    {
        this.document = document;
        this.containerElement = containerElement;
    }
    
    public void WriteString(string tag, string value)
    {
        var insertedElement = document.CreateElement(tag);
        insertedElement.InnerText = value;
        containerElement.AppendChild(insertedElement);
    }
    
    public void WriteBool(string tag, bool value)
    {
        WriteString(tag, value.ToString());
    }
    
    public void WriteFloat(string tag, float value)
    {
        WriteString(tag, value.ToString());
    }
    
    public void WriteVector3(string tag, Vector3 value)
    {
        var element = document.CreateElement(tag);
        var attributes = element.Attributes;

        var xAttribute = document.CreateAttribute("x");
        xAttribute.InnerText = value.x.ToString();

        var yAttribute = document.CreateAttribute("y");
        yAttribute.InnerText = value.y.ToString();

        var zAttribute = document.CreateAttribute("z");
        zAttribute.InnerText = value.z.ToString();

        attributes.Append(xAttribute);
        attributes.Append(yAttribute);
        attributes.Append(zAttribute);

        containerElement.AppendChild(element);
    }
    
    public string ReadString(string tag)
    {
        var elements = containerElement.GetElementsByTagName(tag);
        
        if (elements.Count > 1 || elements.Count < 1)
        {
            throw new Exception(String.Format("In container {0}, there"
            + "should only be 1 {1}", containerElement.Name, tag));
        }
        
        return elements[0].InnerText;
    }
    
    public bool ReadBool(string tag)
    {
        return Convert.ToBoolean(ReadString(tag));
    }
    
    public float ReadFloat(string tag)
    {
        return float.Parse(ReadString(tag));
    }
    
    public Vector3 ReadVector3(string tag)
    {
        var elements = containerElement.GetElementsByTagName(tag);
        
        if (elements.Count > 1 || elements.Count < 1)
        {
            throw new Exception(String.Format("In container {0}, there"
            + "should only be 1 {1}", containerElement.Name, tag));
        }
        
        var element = elements[0];
        var attributes = element.Attributes;


        var xAttribute = attributes[0];
        var yAttribute = attributes[1];
        var zAttribute = attributes[2];

        var x = float.Parse(xAttribute.InnerText);
        var y = float.Parse(yAttribute.InnerText);
        var z = float.Parse(zAttribute.InnerText);

        return new Vector3(x, y, z);
    }
}