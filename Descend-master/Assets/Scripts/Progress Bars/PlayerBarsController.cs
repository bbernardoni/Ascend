using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void BarValueChangedEventHandler(float newValue);

public interface IPlayerControllerWithBars
{
	event BarValueChangedEventHandler HealthChanged;
	event BarValueChangedEventHandler FuelChanged;
}

public class PlayerBarsController : MonoBehaviour {

	[Header("Valid for Seperate Player Controller")]
	[Tooltip("Leave it blank if the player controller is \"PlayerController\"")]
	public MonoBehaviour debugPlayerController;

	private IPlayerControllerWithBars player;

	private HealthBarController healthBarController = null;
	private FuelBarController fuelBarController = null;

	void Awake()
	{

		healthBarController = MonoBehaviour.FindObjectOfType(typeof(HealthBarController)) as HealthBarController;
		fuelBarController = MonoBehaviour.FindObjectOfType(typeof(FuelBarController)) as FuelBarController;

		if (debugPlayerController != null)
		{
			if (debugPlayerController is IPlayerControllerWithBars)
			{
				player = debugPlayerController as IPlayerControllerWithBars;
				player.HealthChanged += HealthChanged;
				player.FuelChanged += FuelChanged;
			}
			else
			{
				Debug.Log("Debug Player Controller needs to conform to IPlayerWithBars");
			}
			return;

		}
		PlayerController controller = GetComponent<PlayerController>();
		
		if (controller is IPlayerControllerWithBars) 
		{
			player = (IPlayerControllerWithBars)controller;
			player.FuelChanged += FuelChanged;
			player.HealthChanged += FuelChanged;
		}
		else 
		{
			Debug.LogError("Player Controller class needs to implement IPlayerWithBars" +
			"interface to communicate with health and fuel bars");
		}
	}

	void HealthChanged(float newHealth)
	{
		if (healthBarController != null) 
		{
			healthBarController.Value = newHealth;
		}
		else
		{
			Debug.LogWarning("No Health Bar Found!");
		}
	}

	void FuelChanged(float newFuel)
	{
		if (fuelBarController != null)
		{
			fuelBarController.Value = newFuel;
		}
		else
		{
			Debug.LogWarning("No Fuel Bar Found");
		}
	}

}
