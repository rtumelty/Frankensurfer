using UnityEngine;
using System.Collections;

public class OrientationSettings : MonoBehaviour {
	[SerializeField] bool portrait = true;
	[SerializeField] bool portraitUpsideDown = true;
	[SerializeField] bool landscapeLeft = true;
	[SerializeField] bool landscapeRight = true;

	// Use this for initialization
	void Awake () {
		Screen.autorotateToLandscapeLeft = landscapeLeft;
		Screen.autorotateToLandscapeRight = landscapeRight;
		Screen.autorotateToPortrait = portrait;
		Screen.autorotateToPortraitUpsideDown = portraitUpsideDown;
	}
}
