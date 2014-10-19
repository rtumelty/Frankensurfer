using UnityEngine;
using System.Collections;

public class Fog : MonoBehaviour {
	[SerializeField] float minSpeed;
	[SerializeField] float maxSpeed;

	float width;
	float speed;

	// Use this for initialization
	void Start () {
		SpriteRenderer fog = GetComponentInChildren<SpriteRenderer>() as SpriteRenderer;
		width = fog.bounds.size.x;

		speed = Random.Range(minSpeed, maxSpeed);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 position = transform.position;
		position.x += speed * Time.deltaTime;
		
		if (position.x >= width) position.x -= width;
		else if (position.x <= -width) position.x += width;

		transform.position = position;
	}
}
