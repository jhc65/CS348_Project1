using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour {

	public static EffectsManager Instance;
	public enum Effects {Confetti};

	[SerializeField] private GameObject Confetti;

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
		}
	}

	public void PlayEffect(Effects effect)
	{
		switch (effect)
		{
			case Effects.Confetti:
				Instantiate(Confetti); /* The Confetti particle system has autoDestroy */
			break;
			default:

			break;
		}
	}
}
