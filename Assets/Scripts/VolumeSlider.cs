using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
	public string volumeType;
	public Text volumePercent;
	Slider slider;
	
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
		slider.value = PlayerPrefs.GetFloat(volumeType + "Volume", 1f);
		volumePercent.text = Mathf.RoundToInt(slider.value * 100) + "%";
    }
	
	public void SetVolumeText(float value)
	{
		PlayerPrefs.SetFloat(volumeType + "Volume", value);
		volumePercent.text = Mathf.RoundToInt(value * 100) + "%";
	}
}
