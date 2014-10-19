using UnityEngine;
using System.Collections;
using HutongGames;

public class Pickup : MonoBehaviour {
	public enum PickupType {
		None,
		Headstone,
		Crossbones,
		Skull
	}

	[SerializeField] PickupType type;
	[SerializeField] int pointValue;
	[SerializeField] float multiplierIncrease;
	[SerializeField] Vector2 addedForce = Vector2.zero;
	[SerializeField] bool spawnFromPool = true;
	[SerializeField] GameObject spawnOnCollision;
	[SerializeField] string prefabPoolIdentifier;
	[SerializeField] string playmakerEvent;

	PrefabPool pool;
	ScoreTracker score;
	AudioSource source;

	void Start() {
		if (spawnFromPool) {
			PrefabPool[] pools = FindObjectsOfType<PrefabPool>();

			foreach (PrefabPool p in pools) {
				if (p.identifier == prefabPoolIdentifier) {
					pool = p;
					break;
				}
			}

			if (pool == null) {
				Debug.LogError("No prefab pool found with identifier " + prefabPoolIdentifier);
				this.enabled = false;
			}
		} else if (spawnOnCollision == null) {
			Debug.LogError("No prefab selected");
			this.enabled = false;
		}

		source = GetComponent<AudioSource>();
		score = FindObjectOfType<ScoreTracker>();
	}

	void OnTriggerEnter2D (Collider2D other) {
		GameObject collidedObject = other.gameObject;

		if (collidedObject.GetComponentInChildren<FrankensurferControls>() != null) {
			// Analytics
			GA.API.Design.NewEvent("Collected:"+gameObject.name);

			if (playmakerEvent != "") PlayMakerFSM.BroadcastEvent(playmakerEvent);
			if (source != null) source.Play();

			if (multiplierIncrease != 0)
				score.AddMultiplier(multiplierIncrease);
			score.AddPoints((int)(pointValue * score.CurrentScore.multiplier), type);
			collidedObject.rigidbody2D.AddForce(addedForce);

			if (type == PickupType.Skull) {
				collidedObject.GetComponentInChildren<Animator>().SetTrigger("skull");
			}

			GameObject points;

			if (spawnFromPool) {
				points = pool.Spawn(transform.position, transform.rotation);
			}
			else
				points = Instantiate(spawnOnCollision, transform.position, Quaternion.identity) as GameObject;

			if (points != null) points.SendMessageUpwards("ShowPoints", (int)(pointValue * score.CurrentScore.multiplier));
			gameObject.SetActive(false);
		}
	}
}
