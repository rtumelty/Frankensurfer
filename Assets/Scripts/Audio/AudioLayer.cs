using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AudioLayer {	
	public string name;
	public bool expand = false;
	public Vector2 scrollPos = Vector2.zero;

	public List<AudioLayerComponent> sources;

	float volume;
	bool mute;

	public float Volume {
		get {
			return volume;
		}
		set {
			volume = value;

			foreach (AudioLayerComponent source in sources)
				source.SetVolumes(value);
		}
	}

	public bool Mute {
		get {
			return mute;
		}
		set {
			mute = value;

			foreach (AudioLayerComponent source in sources)
				source.SetMute(value);
		}
	}

	public AudioLayer (string layerName, float layerVolume, bool muted) {
		name = layerName;

		sources = new List<AudioLayerComponent>();

		volume = layerVolume;
		mute = muted;
	}

	public void RegisterSource(AudioLayerComponent newSource) {
		if (sources.Contains(newSource)) return;
		else {
			newSource.SetVolumes(volume);
			newSource.SetMute(mute);

			sources.Add(newSource);
		}
	}

	public void RemoveSource(AudioLayerComponent source) {
		if (sources.Contains(source)) 
			sources.Remove(source);
	}
}
