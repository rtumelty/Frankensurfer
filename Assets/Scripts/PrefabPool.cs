﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrefabPool : MonoBehaviour {
	public string identifier;
	
	List<GameObject> pool;
	int currentPoolIndex = 0;
	[SerializeField] GameObject prefabToPool;
	[SerializeField] int preinstantiatedCount = 20;
	[SerializeField] int maxObjectCount = 100;

	// Use this for initialization
	void Awake () {
		pool = new List<GameObject>();

		for (int i = 0; i < preinstantiatedCount; i++) {
			GameObject newInstance = Instantiate(prefabToPool) as GameObject;
			pool.Add(newInstance);
			newInstance.transform.parent = transform;

			newInstance.SetActive(false);
		}
	}

	public GameObject Spawn(Vector3 position, Quaternion rotation) {
		GameObject prefab = GetNextInactiveObject();
		prefab.transform.position = position;
		prefab.transform.rotation = rotation;
		prefab.SetActive(true);
		return prefab;
	}

	private GameObject GetNextInactiveObject() {
		bool active = true;
		GameObject go = null;

		for (int i = 0; i < pool.Count && active == true; i++) {
			go = pool[currentPoolIndex++];
			if (currentPoolIndex >= pool.Count) currentPoolIndex = 0;

			active = go.activeSelf;
		}

		if (active) {
			if (pool.Count < maxObjectCount) {
				go = Instantiate(prefabToPool) as GameObject;
				go.transform.parent = transform;
				pool.Add(go);
			}
			else {
				go = pool[currentPoolIndex++];
			}
		}

		return go;
	}
}
