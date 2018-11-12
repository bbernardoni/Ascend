using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampScript : Interactable
{
    
    private PlayerController player;
    private bool overBarrel;

    private Rigidbody2D rb;
    private Vector3 start;
    private Vector3 end, force;
    private float startTime, endTime, deltaTime;

    public AudioSource audioFill;

    public override void function()
    {
        // toggle holding state
        inUse = !inUse;
        rb.isKinematic = inUse;
        if(inUse)
        {
            transform.parent = player.transform;
            transform.position = player.transform.position;
        } else
        {
            rb.velocity = new Vector2(0.0f, 0.0f);
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
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void UpdateInteractable()
    {
        ThrowUsingMouse(player.facingRight);
        ThrowUsingQ(player.facingRight);

        Light lt = gameObject.GetComponentInChildren<Light>();
        if(Input.GetButtonDown("Refill") && inUse && overBarrel)
        {
            lt.intensity = 1.5f;
            audioFill.Play();
        }
        lt.intensity = lt.intensity - 0.001f;
    }

    void ThrowUsingQ(bool facingRight)
    {
        if(Input.GetKeyDown(KeyCode.Q)) startTime = Time.time;
        else if(Input.GetKeyUp(KeyCode.Q))
        {
            endTime = Time.time;
            deltaTime = (endTime - startTime)*10;
        }
        if(Input.GetKeyUp(KeyCode.Q) && inUse)
        {
            rb.isKinematic = false;
            if(facingRight) { rb.velocity = new Vector2(deltaTime, deltaTime); } else { rb.velocity = new Vector2(-deltaTime, deltaTime); }
            transform.parent = null;
            inUse = false;
        }
    }


    void ThrowUsingMouse(bool facingRight)
    {
        if(Input.GetMouseButtonDown(0)) start = Input.mousePosition;
        else if(Input.GetMouseButtonUp(0))
        {
            end = Input.mousePosition;
            force = end - start;
        }
        if(Input.GetMouseButtonUp(0) && inUse)
        {
            rb.isKinematic = false;
            rb.velocity = new Vector2(-force[0]/10, -force[1]/10);
            transform.parent = null;
            inUse = false;
        }
    }
}