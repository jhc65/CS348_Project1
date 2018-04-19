using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EffectsManager : MonoBehaviour {

	public static EffectsManager Instance;
	public enum Effects {
		// Visual
		Confetti,
		
		//Auditory
		Yay};

	[SerializeField] private GameObject Confetti;
	[SerializeField] private AudioClip Yay;

	private AudioSource audioSource;

	void Awake()
	{
		if (Instance != null)
		{
			Debug.LogWarning("Deleting " + this.name + " due to duplicate EffectsManager.");
			GameObject.Destroy(this.gameObject);
		}
		else
		{
			Instance = this;
			Instance.audioSource = GetComponent<AudioSource>();
		}
	}

	public void PlayEffect(Effects effect)
	{
		switch (effect)
		{
			case Effects.Confetti:
				Instantiate(Confetti); /* The Confetti particle system has autoDestroy */
			break;
			case Effects.Yay:
				audioSource.PlayOneShot(Yay);
			break;
			default:

			break;
		}
	}
}
