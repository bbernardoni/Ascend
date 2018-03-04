using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearFilesCanvasController : MonoBehaviour {

	SceneManager manager;

	// Use this for initialization
	void Start () {
		manager = SceneManager.CurrentManager;
	}

	// Update is called once per frame
	void Update () {

	}

	public void ClearAllFiles_OnClick()
	{
		//Debug.Log("Clear all files");
		manager.DeleteFilesOfThisPlayer();
	}
}
