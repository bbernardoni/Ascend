using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {
 
    public GameObject currentInterObj;
    public Collider2D collider;
    bool pickedup;
    int frameupdate = 0;
    public Rigidbody2D rb;
    public Vector3 start;
    public Vector3 end, force;
    public float startTime, endTime, deltaTime;
	// Use this for initialization
    void OnTriggerEnter2D(Collider2D other){
        
        if((other.CompareTag("Lamp") || other.CompareTag("Interactable")) && !pickedup){
                Debug.Log(other.name);
                currentInterObj = other.gameObject;
                rb = other.GetComponent<Rigidbody2D>();
                
        }
        
    }
    
    void OnTriggerExit2D(Collider2D other){
        if((other.CompareTag("Lamp") || other.CompareTag("Interactable")) && !pickedup){
                if(other.gameObject == currentInterObj){
                    currentInterObj = null;
                    rb = null;
                    
                }
        }
    }
	void Start () {
        currentInterObj = null;
        pickedup = false;
	}
	
	// Update is called once per frame
	void Update () {
        bool facingRight = PlayerController.getfacingRight();
        
        Pickup();
        Drop();
        if(currentInterObj.CompareTag("Lamp")){
            ThrowUsingMouse(facingRight);
            ThrowUsingQ(facingRight);
        }
        frameupdate--;
    }
    
    void ThrowUsingQ(bool facingRight){
        if(Input.GetKeyDown(KeyCode.Q)) startTime = Time.time;
        else if(Input.GetKeyUp(KeyCode.Q)) {
            endTime = Time.time;
            deltaTime = (endTime - startTime)*10;
        }
        if (Input.GetKeyUp(KeyCode.Q) && currentInterObj && pickedup)
        {
            rb.isKinematic = false;
            if(facingRight){rb.velocity = new Vector2(deltaTime, deltaTime);}
            else{rb.velocity = new Vector2(-deltaTime, deltaTime);}
            currentInterObj.transform.parent = null;
            pickedup = false;
            frameupdate = 1;
        }
    }
    
    
    void ThrowUsingMouse(bool facingRight){
        if(Input.GetMouseButtonDown(0)) start = Input.mousePosition;
        else if(Input.GetMouseButtonUp(0)) {
            end = Input.mousePosition;
            force = end - start;
        }
        if (Input.GetMouseButtonUp(0) && currentInterObj && pickedup)
        {
            Debug.Log(currentInterObj);
            rb.isKinematic = false;
            rb.velocity = new Vector2(-force[0]/10, -force[1]/10);
            currentInterObj.transform.parent = null;
            pickedup = false;
            frameupdate = 1;
        }
    }
    
    void Drop(){
        if(Input.GetKeyDown(KeyCode.E) && currentInterObj && pickedup && frameupdate <= 0){
            rb.isKinematic = false;
            rb.velocity = new Vector2(0.0f, 0.0f);
            currentInterObj.transform.parent = null;
            pickedup = false;
            
            frameupdate = 1;
            
        }
    }
    
    void Pickup(){
        if(Input.GetKeyDown(KeyCode.E) && currentInterObj && !pickedup && frameupdate <= 0){
            rb.isKinematic = true;
            currentInterObj.transform.parent = this.transform;
            currentInterObj.transform.position = this.transform.position;
            pickedup = true;
            frameupdate = 1;
            
        }
    }
    
    
}


