using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSingleton : MonoBehaviour {

	public static Dictionary<string,GameObject> Instances;

	public string UniqueIdentifier;

	public GameObjectSingleton()
	{
		/* Initialize the Dictionary if it doesn't yet exist */
		if (Instances == null)
		{
			Instances = new Dictionary<string,GameObject>();
		}
	}

	private void Awake()
	{
		if (Instances == null)
			Debug.Log("Constructor failed(?)");
		/* If a gameObject already exists with this identifier, destroy this duplicate */
		if (Instances.ContainsKey(UniqueIdentifier))
		{
			Debug.Log("Destroying duplicate " + this.name);
			DestroyImmediate(this.gameObject);
		}
		/* Otherwise this is the first instance. Add it to the dictionary and mark it to not be destroyed */
		else
		{
			Debug.Log("First instance of " + this.name);
			Instances.Add(UniqueIdentifier, this.gameObject);
			DontDestroyOnLoad(this.gameObject);
		}
	}

	public GameObject GetInstance()
	{
		GameObject obj = null;
		if (Instances.TryGetValue(UniqueIdentifier, out obj))
			return obj;
		else
			{
				Debug.Log("No instance exists for: " + UniqueIdentifier + " (called from " + this.name + ")");
				return null;
			}
	}
	
	public void RemoveGameObject()
	{
		if (Instances.ContainsKey(UniqueIdentifier))
			Instances.Remove(UniqueIdentifier);
		Destroy(this.gameObject);
	}

	void OnDestroy()
	{
		GameObject original;
		Instances.TryGetValue(UniqueIdentifier, out original);
		if (original == this.gameObject)
			Instances.Remove(UniqueIdentifier);
	}
}
