using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour {

    bool activated;

	// Use this for initialization
	void Start () {
        activated = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!activated)
        {
            Debug.Log("trap collision: " + collision.gameObject.name);
            if (collision.gameObject == GameObject.Find("enemy") || collision.gameObject == GameObject.Find("Player"))
            {
                activated = true;
                Destroy(collision.gameObject); //Replace with kill function later
                GetComponent<SpriteRenderer>().color = Color.blue; //Replace with trap animation later
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}
