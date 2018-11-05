using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {

	/* not currently being used
    public enum TYPE
    {
        MELEE, THROWABLE, ENVIRONMENT
    }

    public TYPE InteractableType;*/

    public abstract void function();
    protected bool inUse = false;
    [HideInInspector] public bool inTrigger = false;

    void Update(){
        if(inTrigger && Input.GetKeyDown(KeyCode.E)){
            function();
        }

        UpdateInteractable();
    }

    protected virtual void UpdateInteractable() { }

}
