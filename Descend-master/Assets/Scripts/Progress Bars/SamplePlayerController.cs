using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayerController : MonoBehaviour, IPlayerControllerWithBars {

	public event BarValueChangedEventHandler HealthChanged;
	public event BarValueChangedEventHandler FuelChanged;

	// Use this for initialization
	void Start () 
	{
		HealthChanged(100);
		FuelChanged(100);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			HealthChanged(0);
			FuelChanged(0);
		}
		
	}
}
