using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class MaintainWorldRotation : MonoBehaviour {
	[SerializeField] Vector3 targetRotation;
	Quaternion rot;

	void Start() {
		rot = new Quaternion();
		rot.eulerAngles = targetRotation;
	}

	// Update is called once per frame
	void Update () {
		transform.rotation = rot;
	}
}
