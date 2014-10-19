using UnityEngine;
using System.Collections;

public class Points : MonoBehaviour {
	[SerializeField] Sprite[] numberSprites;
	
	[SerializeField] GameObject[] digits;
	[SerializeField] int lifeTime = 5;

	Animator animator;

	void Awake() {
		animator = GetComponent<Animator>() as Animator;
	}

	void OnEnable() {
		foreach (GameObject go in digits) go.SetActive(false);
	}

	void ShowPoints(int points) {
		points = Mathf.Min(points, 999999);

		for (int i = 0, j = 100000; i < digits.Length; i++, j /= 10) {
			if (j <= points) {
				
				int value = (points % (j*10)) / j;

				digits[i].SetActive(true);
				SpriteRenderer spriteRend = digits[i].GetComponent<SpriteRenderer>() as SpriteRenderer;
				spriteRend.sprite = numberSprites[value];
			}
		}

		if (animator != null) animator.SetTrigger("Play");

		StartCoroutine(DisableAfter(lifeTime));
	}

	IEnumerator DisableAfter(int seconds) {
		yield return new WaitForSeconds(seconds);
		gameObject.SetActive(false);
	}
}
