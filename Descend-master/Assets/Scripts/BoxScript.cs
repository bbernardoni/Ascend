using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : Interactable {
    
    private Rigidbody2D rb2d;
    private AudioSource audio;
    private GameObject holder;

    public override void function(GameObject Player){
        beingHeld = !beingHeld;
        if(beingHeld){
            rb2d.bodyType = RigidbodyType2D.Dynamic;
            FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
            joint.enableCollision = true;
            joint.connectedBody = Player.GetComponent<Rigidbody2D>();
            joint.breakTorque = 900;
        } else {
            rb2d.bodyType = RigidbodyType2D.Kinematic;
            rb2d.velocity = new Vector2(0.0f, 0.0f);
            Destroy(GetComponent<FixedJoint2D>());
        }
        rb2d.freezeRotation = !beingHeld;
        Player.GetComponent<PlayerController>().holdingBox = beingHeld;
        holder = Player;
    }

    void OnJointBreak2D(Joint2D brokenJoint){
        beingHeld = false;
        rb2d.freezeRotation = true;
        holder.GetComponent<PlayerController>().holdingBox = false;
        //rb2d.velocity = new Vector2(0.0f, rb2d.velocity.y);
    }

    // Use this for initialization
    void Start (){
        rb2d = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected override void UpdateInteractable() {
		if(rb2d.velocity.magnitude > 0.1) {
            if(!audio.isPlaying)
                audio.Play();
            audio.pitch = rb2d.velocity.magnitude;
        } else {
            audio.Stop();
        }

        if(!beingHeld && rb2d.velocity.magnitude == 0.0f)
            rb2d.bodyType = RigidbodyType2D.Kinematic;
    }
    
}
