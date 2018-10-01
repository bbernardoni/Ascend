//
//  CheckpointController.cs
//  Pacman > Persistance
//
//  Created by Zehua Chen on 8/10/18.
//  Copyright © 2018 Zehua Chen. All rights reserved.
//
using System.Xml;
using UnityEngine;

public class CheckpointController : Savable
{
    /// <summary>
    /// The xml tag used to store "Used" property
    /// </summary>
    private static string TagOfUsed { get { return "checkpoint_used"; } }
    
    /// <summary>
    /// A refernece to the scene saver object, assigned in the inspector.
    /// </summary> 
    [SerializeField]
    private SceneSaver saver;
    
    /// <summary>
    /// Readonly referene to the scene saver, whose Save(bool) method
    /// is invoked to save the scene.
    /// </summary>
    public SceneSaver SceneSaver
    {
        get { return saver; }
    }
    
    private bool _used;
    
    /// <summary>
    /// A flag on whether the check point has been used.
    /// </summary>
    /// <remarks>
    /// If true, then OnCheckpointDisable will be called to disable the checkopint
    /// If false, then OnCheckpointEnable will be called to enable the checkpoint
    /// </remarks>
    public bool Used 
    { 
        get { return _used; }
        set 
        {
            _used = value;
            ToggleCheckpoint(_used);
        }    
    }
    
    public override string ContainerElementTag
    {
        get { return "checkpoint"; }
    }
    
    /// <summary>
    /// Save the scene and mark the checkpoint as used.
    /// </summary>
    public void Save()
    {
        this.Used = true;
        this.SceneSaver.Save();
    }
    
    public override void OnSave(ISavableWriteStore store)
    {
        store.WriteBool(TagOfUsed, this.Used);
    }
    
    public override void OnLoad(ISavableReadStore store)
    {
        ToggleCheckpoint(store.ReadBool(TagOfUsed));
    }
    
    /// <summary>
    /// Called to diable the checkpoint
    /// </summary>
    /// <remarks>
    /// Override to provide custom disabled behavior.
    /// </remarks>
    protected virtual void OnCheckpointDisable(){}
    /// <summary>
    /// Called to enable the checkpoint
    /// </summary>
    /// <remarks>
    /// Override to provide custom enabled behavior.
    /// </remarks>
    protected virtual void OnCheckpointEnable(){}
    /// <summary>
    /// Enabled or disable the checkpoint based on the parameter
    /// </summary>
    /// <param name="isUsed">If the checkpoint has been used</param>
    private void ToggleCheckpoint(bool isUsed)
    {
        if (isUsed)
        {
            OnCheckpointDisable();
        }
        else 
        {
            OnCheckpointEnable();
        }
    }

}