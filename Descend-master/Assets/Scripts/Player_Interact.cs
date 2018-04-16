using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Interact : MonoBehaviour {
 
    
    public GameObject currentInterObj;
    bool pickedup;
    int frameupdate = 0;
	// Use this for initialization
    void OnTriggerEnter2D(Collider2D other){
        Debug.Log(true);
        if(other.CompareTag("Lamp")){
                Debug.Log(other.name);
                currentInterObj = other.gameObject;
        }
    }
    
    void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Lamp") && !pickedup){
                if(other.gameObject == currentInterObj){
                    currentInterObj = null;
                }
        }
    }
	void Start () {
        currentInterObj = null;
        pickedup = false;
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetButtonDown("Interact") && currentInterObj && pickedup && frameupdate <= 0){
            Debug.Log(currentInterObj);
            currentInterObj.transform.parent = null;
            pickedup = false;
            frameupdate = 1;
            
        }
		if(Input.GetButtonDown("Interact") && currentInterObj && !pickedup && frameupdate <= 0){
            currentInterObj.transform.parent = this.transform;
            pickedup = true;
            Debug.Log(pickedup);
            frameupdate = 1;
        }
        
        frameupdate--;
    }
}
