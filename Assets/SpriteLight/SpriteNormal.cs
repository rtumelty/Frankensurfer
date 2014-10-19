using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteNormal : MonoBehaviour {
	[SerializeField] Texture normalMap;
	static MaterialPropertyBlock block;
	bool run = false;

	// Use this for initialization
	void Awake() {
		if (block == null) block = new MaterialPropertyBlock();
		renderer.castShadows = true;
		Debug.Log(renderer.castShadows);
	}

	void Start () {
		if (renderer.sharedMaterial.HasProperty("_BumpMap")) {
			if (normalMap != null) {
				run = true;
			} else Debug.LogError("Normal Map field must contain a texture");
		}
		else Debug.LogError("SpriteRenderer material has no '_BumpMap' property");
	}

	void Update() {
		if (run) SetNormalMap();
	}

	void SetNormalMap() {
		renderer.GetPropertyBlock(block);
		block.AddTexture("_BumpMap", normalMap);
		renderer.SetPropertyBlock(block);
	}
}
