//
//  ISavableWriteStore.cs
//  Pacman > Persistance
//
//  Created by Zehua Chen on 8/13/18.
//  Copyright © 2018 Zehua Chen. All rights reserved.
//
using System;
using UnityEngine;

/// <summary>
/// Store used to write data to
/// </summary>
public interface ISavableWriteStore: ISavableStore
{
    /// <summary>
    /// Write string to the store.
    /// </summary>
    /// <param name="tag">the tag to be used to help read the value</param>
    /// <param name="value">value</param>
    void WriteString(string tag, string value);
    /// <summary>
    /// Write bool to the store.
    /// </summary>
    /// <param name="tag">the tag to be used to help read the value</param>
    /// <param name="value">value</param>
    void WriteBool(string tag, bool value);
    /// <summary>
    /// Write float to the store.
    /// </summary>
    /// <param name="tag">the tag to be used to help read the value</param>
    /// <param name="value">value</param>
    void WriteFloat(string tag, float value);
    /// <summary>
    /// Write Vector3 to the store.
    /// </summary>
    /// <param name="tag">the tag to be used to help read the value</param>
    /// <param name="value">value</param>
    void WriteVector3(string tag, Vector3 value);
}