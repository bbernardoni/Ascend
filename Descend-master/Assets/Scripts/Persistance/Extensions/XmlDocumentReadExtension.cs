//
//  XmlDocumentReadExtension.cs.cs
//  Pacman > Persistance
//
//  Created by Zehua Chen on 8/5/18.
//  Copyright © 2018 Zehua Chen. All rights reserved.
//
using System;
using System.Xml;
using UnityEngine;

public static class XmlDocumentReadExtension
{
    public static string ReadString(this XmlDocument doc, string tag)
    {

        var elements = doc.DocumentElement.GetElementsByTagName(tag);

        if (elements.Count < 1)
        {
            throw new Exception(String.Format(
                "ReadString: Cannot find elements with tag: {0}",
                tag
            ));
        }
        else if (elements.Count > 1)
        {
            throw new Exception(String.Format(
                "ReadString: More than one element with tag: {0}",
                tag
             ));
        }

        return elements[0].InnerText;
    }

    public static float ReadFloat(this XmlDocument doc, string tag)
    {
        return float.Parse(ReadString(doc, tag));
    }

    public static bool ReadBool(this XmlDocument doc, string tag)
    {
        return Convert.ToBoolean(ReadString(doc, tag));
    }

    public static Vector3 ReadVector3(this XmlDocument doc, string tag)
    {

        var elements = doc.DocumentElement.GetElementsByTagName(tag);

        if (elements.Count < 1)
        {
            throw new Exception(String.Format(
                @"ReadVector3: Cannot find elements with tag: {0};
                elements.Count = {1}
                ",
                tag, elements.Count
            ));
        }
        else if (elements.Count > 1)
        {
            throw new Exception(String.Format(
                @"ReadVector3: More than one element with tag: {0};
                elements.Count = {1}
                ",
                tag, elements.Count
            ));
        }

        var element = elements[0];
        var attributes = element.Attributes;

        if (attributes.Count != 3)
        {
            throw new Exception(String.Format(
                "ReadVector3: vector3 elements has {0} attributes; expecting 3",
                attributes.Count
            ));
        }

        var xAttribute = attributes[0];
        var yAttribute = attributes[1];
        var zAttribute = attributes[2];

        var x = float.Parse(xAttribute.InnerText);
        var y = float.Parse(yAttribute.InnerText);
        var z = float.Parse(zAttribute.InnerText);

        return new Vector3(x, y, z);
    }
}