using UnityEngine;
using System.Collections;

public class PlayerWithStatus : MonoBehaviour
{
    public string NameOfHealthBar;
	public string NameOfFuelBar;

	private HealthBarController healthBarController;
	private FuelBarController fuelBarController;

	private float _health;
	private float _fuel;

    /// <summary>
    /// Gets or sets the health.
    /// </summary>
    /// <value>The health.</value>
	protected float Health
	{
		get
		{
			return _health;
		}
		set
		{
			healthBarController.Value = value;
			_health = value;
		}
	}

    /// <summary>
    /// Gets or sets the fuel.
    /// </summary>
    /// <value>The fuel.</value>
	protected float Fuel
	{
		get
		{
			return _fuel;
		}
		set
		{
			fuelBarController.Value = value;
			_fuel = value;
		}
	}

	protected virtual void Awake()
	{
        healthBarController = HealthBarController.HealthBarControllerWithName(NameOfHealthBar);
        fuelBarController = FuelBarController.FuelBarControllerWithName(NameOfFuelBar);
	}
}
