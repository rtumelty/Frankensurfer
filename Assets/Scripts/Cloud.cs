using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Cloud : MonoBehaviour {
	GameObject backgroundCamera;
	[SerializeField] float minSize = .25f;
	[SerializeField] float maxSize = 2f;
	[SerializeField] float minSpeed = .5f;
	[SerializeField] float maxSpeed = 5f;
	[SerializeField] float lifeTime = 10f;
	[SerializeField] float minHeight = 0;
	[SerializeField] float maxHeight = 4;

	void Awake() {
		backgroundCamera = GameObject.Find("_BackgroundCamera") as GameObject;
	}
	
	void OnEnable() {
		float cameraHeight = backgroundCamera.transform.position.y;
		transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, 
                                   		cameraHeight + minHeight, cameraHeight + maxHeight), 
		                                 transform.position.z);
		transform.localScale = new Vector3(1, 1, 1) * Random.Range(minSize, maxSize);
		rigidbody2D.velocity = new Vector3(Random.Range(minSpeed, maxSpeed), 0, 0);
		StartCoroutine(Lifetime());
	}
	
	void OnDisable() {
		rigidbody2D.velocity = Vector3.zero;
	}

	IEnumerator Lifetime() {
		yield return new WaitForSeconds(lifeTime);


		while (renderer.isVisible) yield return new WaitForSeconds(Time.deltaTime);

		gameObject.SetActive(false);
	}
}
