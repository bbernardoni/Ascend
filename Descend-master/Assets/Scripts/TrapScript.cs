using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    bool activated;
    public Animator anim;
    public AudioSource audio;

    // Use this for initialization
    void Start()
    {
        activated = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!activated)
        {
            Debug.Log("trap collision: " + collider.gameObject.name);
            if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "Enemy")
            {
                activated = true;
                Destroy(collider.gameObject); //Replace with kill function later
                collider.enabled = false;
                anim.Play("trap_close");
                audio.Play();
            }
        }
    }
}