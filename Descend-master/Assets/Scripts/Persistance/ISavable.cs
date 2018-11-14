//
//  Savable.cs
//  Pacman > Persistance
//
//  Created by Zehua Chen on 8/1/18.
//  Copyright © 2018 Zehua Chen. All rights reserved.
//
using System;
using System.Xml;
using UnityEngine;

/// <summary>
/// An object that can store and load data from SceneSaver
/// </summary>
public interface ISavable
{
    /// <summary>
    /// Called when the SceneSaver object is saving the scene
    /// </summary>
    /// <param name="store">store to save data to</param>
    /// <remarks>
    /// Override this method to save custom data.
    /// </remarks>
    void OnSave(ISavableWriteStore store);
    /// <summary>
    /// Called when the SceneSaver object is loading the scene
    /// </summary>
    /// <param name="store">store to load data data</param>
    /// <remarks>
    /// Override this method to load custom data.
    /// </remarks>
    void OnLoad(ISavableReadStore store);
}
