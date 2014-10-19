using UnityEngine;
using System.Collections;

public class CloudFactory : MonoBehaviour {
	[SerializeField] PrefabPool[] cloudPools;
	[SerializeField] int preinstantiatedCount = 10;
	[SerializeField] float minCloudInterval = .2f;
	[SerializeField] float maxCloudInterval = .6f;
	[SerializeField] float minHeight = 15f;
	[SerializeField] float maxHeight = 100f;
	[SerializeField] float screenPadding = 2f;

	[SerializeField] Camera backgroundCamera;

	// Use this for initialization
	void Start () {
		if (cloudPools.Length == 0) Destroy(this);

		PreInstantiate();
		StartCoroutine(SpawnClouds());
	}

	void PreInstantiate() {
		for (int i = 0; i < preinstantiatedCount; i++) {
			float orthographicWidth = backgroundCamera.orthographicSize * backgroundCamera.aspect;
			Vector3 position = new Vector3(backgroundCamera.transform.position.x + 
			                               Random.Range(-orthographicWidth, orthographicWidth), 
			                               backgroundCamera.transform.position.y + 
			                               Random.Range(minHeight, maxHeight), 0);
			cloudPools[Random.Range(0, cloudPools.Length)].Spawn(position, Quaternion.identity);
		}
	}
	
	IEnumerator SpawnClouds() {
		while (true) {
			float xOffset = backgroundCamera.orthographicSize * backgroundCamera.aspect + screenPadding;
			Vector3 position = new Vector3(backgroundCamera.transform.position.x + xOffset, 
			                               backgroundCamera.transform.position.y + Random.Range(minHeight, maxHeight),
			                               0);
			cloudPools[Random.Range(0, cloudPools.Length)].Spawn(position, Quaternion.identity);
			yield return new WaitForSeconds(Random.Range(minCloudInterval, maxCloudInterval));
		}
	}
}
