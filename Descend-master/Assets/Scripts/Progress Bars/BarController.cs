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
		}
	}

	protected virtual void Awake()
	{
		slider = GetComponent<Slider>();

	}

	// Update is called once per frame
	void Update()
	{

	}
}
