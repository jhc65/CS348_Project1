using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour {

    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider backgroundVolumeSlider;
    [SerializeField] private Slider effectsVolumeSlider;

	// Use this for initialization
	void Start () {
		/* Set the volume sliders to the initial values from Constants */
        masterVolumeSlider.value = Constants.masterVolume;
        backgroundVolumeSlider.value = Constants.backgroundVolume;
        effectsVolumeSlider.value = Constants.effectsVolume;
	}
	
	public void AudioSliderChanged()
    {
        Debug.Log("Updating volume from sliders");
        /* Update the values for all sliders */
        Constants.masterVolume = masterVolumeSlider.value;
        Constants.backgroundVolume = backgroundVolumeSlider.value;
        Constants.effectsVolume = effectsVolumeSlider.value;

        /* Tell the Audio Manager to update */
        AudioManager.Instance.UpdateAudioMixer();
    }
}
