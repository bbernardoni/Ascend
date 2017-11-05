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
    public bool inUse;
    public bool beingHeld;

}
