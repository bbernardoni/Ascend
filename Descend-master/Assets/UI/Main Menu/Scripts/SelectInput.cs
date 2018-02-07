using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectInput : MonoBehaviour {

    public EventSystem eventSys;
    public GameObject selectedObj;

    private bool selected;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxisRaw("Vertical") != 0 && !selected)
        {
            eventSys.SetSelectedGameObject(selectedObj);
            selected = true;
        }
	}

    private void OnDisable()
    {
        selected = false;
    }
}
