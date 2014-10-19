using UnityEngine;
using System.Collections;

public class DontDestroy : MonoBehaviour {

	public GameObject[] objects;

	// Use this for initialization
	void Awake () {
		if (objects == null)
			DontDestroyOnLoad (gameObject);
		else if (objects.Length == 0)
			DontDestroyOnLoad (gameObject);
		else {
			foreach (GameObject go in objects) {
				DontDestroyOnLoad(go);
			}
		}
	}
}
