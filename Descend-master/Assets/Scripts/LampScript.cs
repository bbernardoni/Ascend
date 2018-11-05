using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampScript : MonoBehaviour {

    public GameObject currentInterObj = null;
    
    
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Oil Barrel")){
                Debug.Log(other.name);
                currentInterObj = other.gameObject;
        }
        if(other.CompareTag("Player")){
            GetComponent<Rigidbody2D>().isKinematic = false;
        }
    }
    void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Oil Barrel") ){
                if(other.gameObject == currentInterObj){
                    currentInterObj = null;
                }
        }
        if(other.CompareTag("Player")){
            GetComponent<Rigidbody2D>().isKinematic = false;
        }
    }
    
    void Update () {
        Light lt = this.transform.GetChild(0).gameObject.GetComponent<Light>();
        if(Input.GetButtonDown("Refill") && currentInterObj){
                lt.intensity = 1.5f;
        }
        lt.intensity = lt.intensity - 0.0001f;
        
    }
}
