using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampScript : Interactable
{
    
    private PlayerController player;
    private bool overBarrel;

    public override void function()
    {
        // toggle holding state
        inUse = !inUse;
        if(inUse) {
            transform.parent = player.transform;
        } else {
            transform.parent = null;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Oil Barrel"))
        {
            overBarrel = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Oil Barrel"))
        {
            overBarrel = false;
        }
    }

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        overBarrel = false;
    }

    protected override void UpdateInteractable()
    {
        Light lt = gameObject.GetComponentInChildren<Light>();
        if(Input.GetButtonDown("Refill") && inUse && overBarrel)
        {
            lt.intensity = 1.5f;
        }
        lt.intensity = lt.intensity - 0.001f;
    }
}
