using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class HealthBarController : BarController 
{

	public override void Awake()
	{
		base.Awake();
		base.ValueChanged += HealthChanged;

	}

	void HealthChanged(float newHealth)
	{
		//TO-DO: apply animation
	}

    public static HealthBarController HealthBarControllerWithName(string name)
	{
		return GameObject.Find(name).GetComponent<HealthBarController>();
	}
}
