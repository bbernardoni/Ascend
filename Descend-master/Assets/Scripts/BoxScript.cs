using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : Interactable, ISavable {
    
    private Rigidbody2D rb2d;
    private AudioSource boxAudio;
    private PlayerController player;

    public override void function(){
        // toggle holding state
        inUse = !inUse;
        if(inUse){
            rb2d.bodyType = RigidbodyType2D.Dynamic;
            FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
            joint.enableCollision = true;
            joint.connectedBody = player.GetComponent<Rigidbody2D>();
            joint.breakTorque = 900;
        } else {
            rb2d.bodyType = RigidbodyType2D.Kinematic;
            rb2d.velocity = Vector2.zero;
            Destroy(GetComponent<FixedJoint2D>());
        }
        rb2d.freezeRotation = !inUse;
        player.SetHoldingBox(inUse);
    }

    void OnJointBreak2D(Joint2D brokenJoint){
        rb2d.freezeRotation = true;
        inUse = false;
        player.SetHoldingBox(false);
        //rb2d.velocity = new Vector2(0.0f, rb2d.velocity.y);
    }
    
    void Start (){
        rb2d = GetComponent<Rigidbody2D>();
        boxAudio = GetComponent<AudioSource>();
        player = FindObjectOfType<PlayerController>();
    }
    
    protected override void UpdateInteractable() {
        // audio stuff
		if(Mathf.Abs(rb2d.velocity.x) > 0.1) {
            if(!boxAudio.isPlaying)
                boxAudio.Play();
        } else {
            boxAudio.Stop();
        }
    }

    void FixedUpdate() {
        // freeze box when it hits the ground
        if(!inUse && rb2d.velocity.magnitude == 0.0f)
            rb2d.bodyType = RigidbodyType2D.Kinematic;
    }

    public void OnSave(ISavableWriteStore store)
    {
        store.WriteVector3("pos", rb2d.position);
    }

    public void OnLoad(ISavableReadStore store)
    {
        inUse = false;
        rb2d.position = store.ReadVector3("pos");
        rb2d.velocity = new Vector2(0, 0.001f);
        rb2d.bodyType = RigidbodyType2D.Dynamic;
        rb2d.freezeRotation = true;
        FixedJoint2D joint = GetComponent<FixedJoint2D>();
        if(joint)
            Destroy(joint);
    }
}
