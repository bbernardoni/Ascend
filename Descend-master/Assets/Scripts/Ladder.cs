using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour {

	private PlayerController thePlayer;

	// Use this for initialization
	void Start () {
		thePlayer = FindObjectOfType<PlayerController>();
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			thePlayer.onLadder = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player") {
			thePlayer.onLadder = false;
		}
	}
}
