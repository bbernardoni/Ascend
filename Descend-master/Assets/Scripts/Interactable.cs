using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {

	public enum TYPE
    {
        MELEE, THROWABLE, ENVIRONMENT
    }

    public TYPE InteractableType;

    public abstract void function(GameObject Player);
    protected bool inUse;
    protected bool beingHeld;
    [HideInInspector] public bool inTrigger;
    [HideInInspector] public GameObject player;

    void Update(){
        if(inTrigger && Input.GetKeyDown(KeyCode.E)){
            function(player);
        }

        UpdateInteractable();
    }

    protected virtual void UpdateInteractable() { }

}
