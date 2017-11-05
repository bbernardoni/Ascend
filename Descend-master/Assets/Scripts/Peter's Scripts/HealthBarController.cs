using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class HealthBarController : BarController 
{
    public static HealthBarController HealthBarControllerWithName(string name)
	{
		return GameObject.Find(name).GetComponent<HealthBarController>();
	}
}
