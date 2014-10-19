using UnityEngine;
using System.Collections;

public class SetNGUISlider : MonoBehaviour {
	[SerializeField] UISlider slider;

	public float sliderPosition = 0;

	void Start() {
		if (slider == null)
			slider = GetComponent<UISlider>() as UISlider;
	}

	void Update() {
		slider.value = sliderPosition;
	}
}
