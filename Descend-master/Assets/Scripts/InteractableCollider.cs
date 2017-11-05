using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider Other)
    {
        Debug.Log("In range of collider");
        if (Other.gameObject.CompareTag("Interactable"))
        {
            Debug.Log("Elevator Button pressed");
            GameObject player = GetComponentInParent<GameObject>();
            Other.GetComponent<Interactable>().function(player);
        }
        Destroy(Other.gameObject);
    }
}
