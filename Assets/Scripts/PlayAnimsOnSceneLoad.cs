using UnityEngine;
using System.Collections;

public class PlayAnimsOnSceneLoad : MonoBehaviour {
	[SerializeField] Animator[] animators;
	
	void Start() {
		foreach (Animator a in animators)
			a.SetTrigger("Start");
	}
}
