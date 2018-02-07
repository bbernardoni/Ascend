using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {

    public Slider volSlider;
    public AudioSource bgMusic;
	
	// Update is called once per frame
	void Update () {
        //change when slider changed
        bgMusic.volume = volSlider.value;
	}
}
