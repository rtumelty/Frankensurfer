using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(AudioLayerManager))]
public class AudioLayerManagerEditor : Editor {
	string activeLayer = "";
	Object[] selection;

	Vector2 scrollPos = Vector2.zero;

	public override void OnInspectorGUI() {

		DisplayLayers();

		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(new GUIContent("Active Layer", "Layer "), GUILayout.Width(100));
			activeLayer = EditorGUILayout.TextField(activeLayer, GUILayout.ExpandWidth(true));
		EditorGUILayout.EndHorizontal();

		if (GUILayout.Button(new GUIContent("Register Selected Audiosources to layer", "Adds RegisterAudioLayer " +
			"script to any selected gameobjects with an AudioSource component (Scene objects or Prefabs) and " +
			"registers them to the layer specified above. If left blank, the 'default' audio layer will be selected.")))
		{
			selection = Selection.GetFiltered(typeof(GameObject), SelectionMode.Unfiltered);

			foreach (Object o in selection) {
				GameObject go = o as GameObject;

				if (go != null) {
					if (go.GetComponent<AudioSource>() != null) {
						AudioLayerComponent register = go.GetComponent<AudioLayerComponent>();
						if (register == null)
							register = go.AddComponent<AudioLayerComponent>();

						register.AudioLayer = activeLayer;
					}
				}
			}
		}

		if (GUILayout.Button(new GUIContent("Register Scene Audiosources to layer", "Adds RegisterAudioLayer " +
			"script to all scene gameobjects with an AudioSource and registers them to the layer specified " +
            "above. If left blank, the 'default' audio layer will be selected."))) 
		{
			AudioSource[] sceneObjects = FindObjectsOfType<AudioSource>();

			foreach (AudioSource source in sceneObjects) {
				GameObject go = source.gameObject;
				
				AudioLayerComponent register = go.GetComponent<AudioLayerComponent>();
				if (register == null)
					register = go.AddComponent<AudioLayerComponent>();

				register.AudioLayer = activeLayer;
			}
		}
    }

	void DisplayLayers() {
		Dictionary<string, AudioLayer> layers = AudioLayerManager.Instance.Layers;

		EditorGUILayout.LabelField("Layers", EditorStyles.boldLabel);
		
		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Number of layers:", GUILayout.Width(120f));
			EditorGUILayout.LabelField(layers.Count.ToString());
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();

		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
			foreach (KeyValuePair<string, AudioLayer> layerEntry in layers) {
				AudioLayer layer = layerEntry.Value;
				layer.expand = EditorGUILayout.Foldout(layer.expand, layer.name, EditorStyles.miniBoldLabel);

				if (layer.expand) {
				}
			}
		EditorGUILayout.EndScrollView();
	}
}
