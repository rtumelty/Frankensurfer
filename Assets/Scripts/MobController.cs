using HutongGames.PlayMaker;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class MobController : MonoBehaviour {
	[SerializeField] GameObject player;

	[SerializeField] float currentFollowDistance;
	[SerializeField] float maximumFollowDistance = 60f;
	[SerializeField] float moveSpeed = 50f;
	[SerializeField] float dangerZone = 5f;

	bool onTerrain = false;
	[SerializeField]LayerMask raycastMask;
	FrankensurferControls controls;
	PlayMakerFSM stateManager;
	bool inDanger = false;

	public float normalizedDistance  = 0;

	public float MaximumFollowDistance {
		get {
			return maximumFollowDistance;
		}
		set {
			maximumFollowDistance = value;
		}
	}

	public float MoveSpeed {
		get {
			return moveSpeed;
		}
		set {
			moveSpeed = value;
		}
	}

	// Use this for initialization
	void Awake () {
		currentFollowDistance = maximumFollowDistance;
		transform.position = player.transform.position - new Vector3(currentFollowDistance, 0, 0);
		raycastMask = 1;//~(1 << gameObject.layer);

		controls = player.GetComponent<FrankensurferControls>() as FrankensurferControls;
		gameObject.SetActive(false);
		rigidbody2D.isKinematic = true;

		stateManager = GameObject.Find("_StateManager").GetComponent<PlayMakerFSM>() as PlayMakerFSM;
	}
	
	// Update is called once per frame
	void Update() {
		currentFollowDistance = Mathf.Min(player.transform.position.x - transform.position.x, maximumFollowDistance);
		normalizedDistance = Mathf.Clamp01(1 - (currentFollowDistance / maximumFollowDistance));
		stateManager.FsmVariables.GetFsmFloat("MobDistance").Value = normalizedDistance;

		if (currentFollowDistance < 25) {
			RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector2(0, -1), 
			                                     Mathf.Infinity, raycastMask);

			if (hit.collider == null) onTerrain = false;

			if (!onTerrain) {
				hit = Physics2D.Raycast(new Vector2(transform.position.x, 500), new Vector2(0, -1), 
				                        Mathf.Infinity, raycastMask);
				
				if (hit.point != Vector2.zero) {
					transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
					rigidbody2D.velocity = Vector2.zero;
					rigidbody2D.isKinematic = false;
					onTerrain = true;
				}
			}

			if (currentFollowDistance < dangerZone && !inDanger) StartCoroutine("AlmostCaught");

			if (currentFollowDistance <= 0 && !controls.GameOver) {
				transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
			}
		} else { 
			onTerrain = false;

			if (currentFollowDistance >= maximumFollowDistance) {
				transform.position = new Vector3(player.transform.position.x - maximumFollowDistance,
				                                 transform.position.y, transform.position.z);
			}
		}
	}

	void FixedUpdate () {
		Vector2 velocity = rigidbody2D.velocity;
		velocity.x = moveSpeed;

		rigidbody2D.velocity = velocity;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") other.transform.SendMessageUpwards("EndGame");
	}

	IEnumerator AlmostCaught() {
		inDanger = true;

		while (currentFollowDistance < dangerZone) {
			if (currentFollowDistance < 0) {
				inDanger = false;
				yield break;
			}
			yield return new WaitForSeconds(Time.deltaTime);
		}

		PlayMakerFSM.BroadcastEvent("Escaped");
		inDanger = false;
	}
}
