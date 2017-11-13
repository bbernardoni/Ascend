using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBarController : BarController
{
    public static FuelBarController FuelBarControllerWithName(string name)
    {
        return GameObject.Find(name).GetComponent<FuelBarController>();
    }
}