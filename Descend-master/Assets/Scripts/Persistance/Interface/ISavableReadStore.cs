//
//  ISavableReadStore.cs
//  Pacman > Persistance
//
//  Created by Zehua Chen on 8/13/18.
//  Copyright © 2018 Zehua Chen. All rights reserved.
//
using System;
using UnityEngine;

/// <summary>
/// Store used to load data from
/// </summary>
public interface ISavableReadStore: ISavableStore
{
    /// <summary>
    /// Read string from the store.
    /// </summary>
    /// <param name="tag">the tag the value is assigned during writing</param>
    /// <returns>a value if there is a matching tag</returns>
    string ReadString(string tag);
    /// <summary>
    /// Read bool from the store.
    /// </summary>
    /// <param name="tag">the tag the value is assigned during writing</param>
    /// <returns>a value if there is a matching tag</returns>
    bool ReadBool(string tag);
    /// <summary>
    /// Read float from the store.
    /// </summary>
    /// <param name="tag">the tag the value is assigned during writing</param>
    /// <returns>a value if there is a matching tag</returns>
    float ReadFloat(string tag);
    /// <summary>
    /// Read vector3 from the store.
    /// </summary>
    /// <param name="tag">the tag the value is assigned during writing</param>
    /// <returns>a value if there is a matching tag</returns>
    Vector3 ReadVector3(string tag);
}