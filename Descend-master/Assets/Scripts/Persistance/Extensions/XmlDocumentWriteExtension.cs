//
//  XmlDocumentWriteExtension.cs.cs
//  Pacman > Persistance
//
//  Created by Zehua Chen on 8/5/18.
//  Copyright © 2018 Zehua Chen. All rights reserved.
//
using System;
using System.Xml;
using UnityEngine;

public static class XmlDocumentWriteExtension
{
        
    public static void WriteString(this XmlDocument doc, string tag, string value)
    {
        var e = doc.CreateElement(tag);
        e.InnerText = value;

        doc.DocumentElement.AppendChild(e);
    }
    
    public static void WriteFloat(this XmlDocument doc, string tag, float number)
    {
        WriteString(doc, tag, number.ToString());
    }

    public static void WriteBool(this XmlDocument doc, string tag, bool value)
    {
        WriteString(doc, tag, value.ToString());
    }

    public static void WriteVector3(this XmlDocument doc, string tag, Vector3 vector3)
    {
        var element = doc.CreateElement(tag);
        var attributes = element.Attributes;

        var xAttribute = doc.CreateAttribute("x");
        xAttribute.InnerText = vector3.x.ToString();

        var yAttribute = doc.CreateAttribute("y");
        yAttribute.InnerText = vector3.y.ToString();

        var zAttribute = doc.CreateAttribute("z");
        zAttribute.InnerText = vector3.z.ToString();

        attributes.Append(xAttribute);
        attributes.Append(yAttribute);
        attributes.Append(zAttribute);

        doc.DocumentElement.AppendChild(element);
    }
}