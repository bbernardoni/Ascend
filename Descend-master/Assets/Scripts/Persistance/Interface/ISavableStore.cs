//
//  ISavableStore.cs
//  Pacman > Persistance
//
//  Created by Zehua Chen on 8/13/18.
//  Copyright © 2018 Zehua Chen. All rights reserved.
//
using System;

/// <summary>
/// A store to read and write data from, by Savable objects
/// </summary>
public interface ISavableStore
{
    /// <summary>
    /// The document containing the store
    /// </summary>
    object Document { get; }
    /// <summary>
    /// The container in the document that contains the store.
    /// </summary>
    object Container { get; }
}