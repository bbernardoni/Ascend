using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : Interactable {

    public Rigidbody2D playerRB;
    private Rigidbody2D rb2d;
    private AudioSource audio;

    public override void function(GameObject Player){
        beingHeld = !beingHeld;
        if(beingHeld){
            GetComponent<FixedJoint2D>().connectedBody = playerRB;
        } else {
            GetComponent<FixedJoint2D>().connectedBody = null;
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
