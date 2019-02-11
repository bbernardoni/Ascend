using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectInput : MonoBehaviour {
    
    public Selectable selectedObj;

    private bool selected;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxisRaw("Vertical") != 0 && !selected)
        {
            selectedObj.Select();
            selected = true;
        }
    }

    private void OnDisable()
    {
        selected = false;
    }
}
