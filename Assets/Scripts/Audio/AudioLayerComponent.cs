using UnityEngine;
using System.Collections;

public class AudioLayerComponent : MonoBehaviour {
	[SerializeField] AudioSource[] sources;
	[SerializeField] string audioLayer;

	private string backupLayer;
	float[] volumes;

	public string AudioLayer {
		get {
			return audioLayer;
		}
		set {
			audioLayer = value;

			Register();
		}
	}

	void Awake() {
		if (sources == null) sources = new AudioSource[0];

		if (sources.Length == 0) sources = GetComponents<AudioSource>();
		volumes = new float[sources.Length];

		for (int i = 0; i < sources.Length; i++) volumes[i] = sources[i].volume;
	}

	void OnEnable () {
		Register();
	}

	void OnDisable () {
		Deregister();
	}
	
	void Register() {
		if (sources == null) sources = new AudioSource[0];
		Deregister();

		backupLayer = audioLayer;

		if (gameObject.activeInHierarchy) {
			AudioLayerManager.Instance.RegisterAudioSource(this, audioLayer);
		}
	}
	
	void Deregister() {
		AudioLayerManager.Instance.DeregisterAudioSource(this, backupLayer);
	}

	public void SetVolumes(float f) {
		for (int i = 0; i < sources.Length; i++)
			sources[i].volume = volumes[i] * f;
	}

	public void SetMute(bool muted) {
		foreach (AudioSource source in sources)
			source.mute = muted;
	}
}
