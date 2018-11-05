using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour {

    bool activated;
    public Animator anim;
    public AudioSource audio;

    // Use this for initialization
    void Start () {
        activated = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!activated)
        {
            Debug.Log("trap collision: " + collision.gameObject.name);
            if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
            {
                activated = true;
                Destroy(collision.gameObject); //Replace with kill function later
                GetComponent<Collider2D>().enabled = false;
                anim.Play("trap_close");
                audio.Play();
            }
        }
    }
}
