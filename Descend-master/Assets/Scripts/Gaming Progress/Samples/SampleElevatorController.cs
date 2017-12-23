using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleElevatorController : MonoBehaviour {

	public Transform StartPosition;
	public Transform EndPosition;

	public SceneManager sceneManager;

	bool activated = false;

	// Use this for initialization
	void Start () {
		this.gameObject.transform.position = StartPosition.position;
		Debug.Log(string.Format("Elevator's Initial Position: {0}", this.gameObject.transform.position));
	}
	
	// Update is called once per frame
	void Update () {
		//Press space to activate the elevator
		if (Input.GetKeyDown(KeyCode.Space)) {
			if (!activated) 
			{
				this.transform.position = EndPosition.position;
				activated = true;
			}
			else 
			{
				this.transform.position = StartPosition.position;
				activated = false;
			}
			
			sceneManager.UpdateEnvironmentItem(gameObject);
		}
	}
}
