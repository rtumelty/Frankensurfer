using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioLayerManager : Singleton<AudioLayerManager> {	
	Dictionary<string, AudioLayer> layers;

	public Dictionary<string, AudioLayer> Layers {
		get {
			CheckInit();

			return layers;
		}
	}

	// Use this for initialization
	void Awake () {
		//DontDestroyOnLoad(this);
		CheckInit();

	}

	void CheckInit() {
		if (layers == null) {
			layers = new Dictionary<string, AudioLayer>();

			int count = PlayerPrefs.GetInt("layerCount");
			for (int i = 0; i < count; i++) {
				string name = PlayerPrefs.GetString("layer" + i + "Name");

				AudioLayer layer;
				if (layers.TryGetValue(name, out layer) == false) {
					string muted = PlayerPrefs.GetString("layer" + i + "Mute");

					layer = new AudioLayer(name, PlayerPrefs.GetFloat("layer" + i + "Volume"), 
					                       (string.Equals(muted, "true")) ? true : false);
					layers.Add(name, layer);
				} else {
					layer.Volume = PlayerPrefs.GetFloat("layer" + i + "Volume");
					layer.Mute = (string.Equals(PlayerPrefs.GetString("layer" + i + "Mute"), "true")) ? true : false;
				}
			}
		}
	}

	void OnDisable() {
		PlayerPrefs.SetInt("layerCount", layers.Count);

		int i = 0;
		foreach (KeyValuePair<string, AudioLayer> pair in layers ) {
			PlayerPrefs.SetString("layer" + i + "Name", pair.Value.name);
			PlayerPrefs.SetString("layer" + i + "Mute", (pair.Value.Mute) ? "true" : "false");
			PlayerPrefs.SetFloat("layer" + i + "Volume", pair.Value.Volume);
			i++;
		}
	}
	
	public void RegisterAudioSource(AudioLayerComponent source, string audioLayer) {
		if (string.IsNullOrEmpty(audioLayer)) audioLayer = "default";
		CheckInit();
		AudioLayer layer;
		if (layers.TryGetValue(audioLayer, out layer) == false) {
			layer = new AudioLayer(audioLayer, 1f, false);
			layers.Add(audioLayer, layer);
		}

		layer.RegisterSource(source);
	}

	public void DeregisterAudioSource(AudioLayerComponent source, string audioLayer) {
		if (string.IsNullOrEmpty(audioLayer)) audioLayer = "default";
		AudioLayer layer;
		if (layers.TryGetValue(audioLayer, out layer) == false) {
			return;
		}
		
		layer.RemoveSource(source);
	}
	
	public void MuteLayer(string audioLayer) {
		AudioLayer layer;
		
		if (layers.TryGetValue(audioLayer, out layer) == false) {
			return;
		}
		else {
			layer.Mute = !layer.Mute;
		}
	}

	public bool LayerMuted(string audioLayer) {
		AudioLayer layer;
		
		if (layers.TryGetValue(audioLayer, out layer) == false) {
			return true;
		}
		else {
			return layer.Mute;
		}
	}

	public AudioLayer GetLayer(string audioLayer) {
		AudioLayer layer;
		
		if (layers.TryGetValue(audioLayer, out layer) == false) {
			return null;
		}
		else {
			return layer;
		}
	}
}
