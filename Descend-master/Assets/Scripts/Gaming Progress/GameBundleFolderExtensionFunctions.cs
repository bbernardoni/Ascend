using System;
using System.Xml;
using System.IO;

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