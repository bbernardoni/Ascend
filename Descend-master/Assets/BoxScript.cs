using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour {

    public Rigidbody2D rb2d;
    public AudioSource audio;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(rb2d.velocity.magnitude > 0.1)
        {
            if(!audio.isPlaying)
                audio.Play();
            audio.pitch = rb2d.velocity.magnitude;
            
        } else
        {
            audio.Stop();
        }
	}
}
