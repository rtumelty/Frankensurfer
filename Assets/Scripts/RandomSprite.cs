using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomSprite : MonoBehaviour {
	public Sprite[] sprites;
	public int lifeTime = 5;

	SpriteRenderer spriteRenderer;

	void Awake() {
		if (sprites.Length == 0) Destroy (this);

		spriteRenderer = GetComponent<SpriteRenderer>() as SpriteRenderer;
	}

	void OnEnable () {
		spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

		StartCoroutine(DisableAfter());
	}

	IEnumerator DisableAfter() {
		yield return new WaitForSeconds(lifeTime);

		while (spriteRenderer.isVisible) yield return new WaitForSeconds(Time.deltaTime);

		gameObject.SetActive(false);
	}
}
