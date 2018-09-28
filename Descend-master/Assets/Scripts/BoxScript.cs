using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : Interactable {
    
    private Rigidbody2D rb2d;
    private AudioSource audio;

    public override void function(GameObject Player){
        beingHeld = !beingHeld;
        GetComponent<FixedJoint2D>().enabled = beingHeld;
        if(beingHeld){
            rb2d.bodyType = RigidbodyType2D.Dynamic;
        } else {
            //rb2d.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    // Use this for initialization
    void Start (){
        rb2d = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
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
