using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BarController : MonoBehaviour
{
	protected Slider slider;

	public float Value
	{
		get
		{
			return slider.value;
		}
		set
		{
			slider.value = value;
			ValueChanged(value);
		}
	}

	protected delegate void ValueChangedHandler(float newValue);
	protected event ValueChangedHandler ValueChanged;

	public virtual void Awake()
	{
		slider = GetComponent<Slider>();
	}
}
