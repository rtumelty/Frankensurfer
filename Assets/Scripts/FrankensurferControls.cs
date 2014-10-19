using HutongGames.PlayMaker;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class FrankensurferControls : MonoBehaviour {
	[SerializeField] GameObject spriteGameObject;
	[SerializeField] GameObject mob;
	[SerializeField] GameObject cameraTarget;
	[SerializeField] ParticleSystem dirtParticleSystem;
	[SerializeField] AudioSource boardScrape;
	[SerializeField] AudioClip endGameClip;
	[SerializeField] Transform spritePivot;
	[SerializeField] float deltaGround = .1f;
	[SerializeField] float baseSpeed = 5f;
	[SerializeField] float minVerticalSpeed = -40f;
	[SerializeField] float angleMatchTime = .1f;
	[SerializeField] bool isGrounded = false;
	[SerializeField] int airAnimations = 4;
	[SerializeField] Vector2 startForce;
	[SerializeField] Vector2 downForce;
	[SerializeField] float minimumRelativeSizeOnScreen = .1f;
	[SerializeField] float baseAirTimeMultiplier = .1f;
	[SerializeField] float airMultiplierScaleFactor = 2f;
	[SerializeField] float baseGroundedMultiplier = -.1f;
	[SerializeField] float groundedMultiplierScaleFactor = 2f;
	[SerializeField] float multiplierInterval = .65f;

	CircleCollider2D circleCollider;
	LayerMask raycastMask;
	RaycastHit2D hitInformation;
	float distanceToGround = 0f;
	Vector2 surfaceNormal;
	float angle = 0f;
	Camera mainCamera;
	ScoreTracker score;
	
	bool moving = false;
	bool gameOver = false;

	public bool Moving {
		get {
			return moving;
		}
	}

	public bool GameOver {
		get {
			return gameOver;
		}
	}

	Animator spriteAnimator;

	// Use this for initialization
	void Start () {
		mainCamera = Camera.main;
		mainCamera.GetComponent<CameraController>().enabled = true;

		circleCollider = GetComponent<CircleCollider2D>() as CircleCollider2D;
		raycastMask = ~(1 << gameObject.layer);

		if (cameraTarget == null) cameraTarget = new GameObject();
		cameraTarget.transform.position = transform.TransformPoint(new Vector3(0, 3, 0));

		if (spriteGameObject == null)
			spriteGameObject = gameObject;
		spriteAnimator = spriteGameObject.GetComponent<Animator>() as Animator;

		score = GetComponent<ScoreTracker>() as ScoreTracker;
	}
	
	// Update is called once per frame
	void Update() {
		MatchRotation();
		ScaleCharacter();

		spriteAnimator.SetFloat("Angle", angle);
		spriteAnimator.SetFloat("Speed", rigidbody2D.velocity.magnitude);
		spriteAnimator.SetFloat("Height", distanceToGround);
	}

	void FixedUpdate () {
		bool wasGrounded = isGrounded;
		IsGrounded();
		if (wasGrounded != isGrounded) {
			if (wasGrounded == true) {
				dirtParticleSystem.enableEmission = false;
				boardScrape.Stop();

				StartCoroutine(AirTime());
				spriteAnimator.SetFloat("randomAnim", Random.Range(0, airAnimations));
				spriteAnimator.SetTrigger("airbourne");
				PlayMakerFSM.BroadcastEvent("Trick");
			}
			else {
				StartCoroutine(Grounded());
				dirtParticleSystem.enableEmission = true;
				spriteAnimator.SetTrigger("landing");
			}
		}

		if ((Input.touchCount > 0) || (Input.GetMouseButton(0))) {
			if (moving) {
				rigidbody2D.AddForce(downForce * rigidbody2D.mass);
			}
			else {
				rigidbody2D.isKinematic = false;
				rigidbody2D.AddForce(startForce * rigidbody2D.mass);
				mob.SetActive(true);
				moving = true;
				PlayMakerFSM.BroadcastEvent("RemoveInstructions");
			}
		}

		if (moving && !gameOver) {
			if (isGrounded && !boardScrape.isPlaying && Time.timeScale != 0) boardScrape.Play();

			Vector2 velocity = rigidbody2D.velocity;

			velocity = new Vector2(Mathf.Max(velocity.x, baseSpeed), Mathf.Max (velocity.y, minVerticalSpeed));
			rigidbody2D.velocity = velocity;
		}
	}

	private void IsGrounded() {
		hitInformation = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y +
                            circleCollider.center.y - circleCollider.radius), new Vector2(0, -1), 
                            Mathf.Infinity, raycastMask);

		if (hitInformation.collider != null) {
			distanceToGround = (new Vector2(transform.position.x, transform.position.y)- hitInformation.point).magnitude;
			if (distanceToGround < deltaGround) {
				isGrounded = true;
				surfaceNormal = hitInformation.normal;
			}
			else {
				isGrounded = false;
				surfaceNormal = Vector2.up;
			}
		}
		else {
			isGrounded = false;
			distanceToGround = Mathf.Infinity;
			surfaceNormal = Vector2.up;
		}
	}

	void MatchRotation() {
		float targetAngle = Vector2.Angle(Vector2.up, surfaceNormal);
		if (surfaceNormal.x > 0) targetAngle *= -1;

		angle = Mathf.Lerp(angle, targetAngle, Time.deltaTime * .8f);

		float localAngle = Vector2.Angle(
			new Vector2(spritePivot.up.x, spritePivot.up.y), surfaceNormal);

		
		if (localAngle > 1f) {
			if (surfaceNormal.x - spritePivot.up.x > 0) localAngle *= -1;

			spritePivot.Rotate(new Vector3(0, 0, 1), localAngle * Time.deltaTime * angleMatchTime);
		}
	}

	void ScaleCharacter() {
		// Assumes character size is usually 3
		float scale = mainCamera.orthographicSize * minimumRelativeSizeOnScreen / 3f;
		scale = Mathf.Max (1f, scale);
		spriteGameObject.transform.localScale = new Vector3(scale, scale, scale);
		cameraTarget.transform.position = transform.TransformPoint(new Vector3(0, 3, 0) * scale);
	}

	IEnumerator EndGame() {
		gameOver = true;
		boardScrape.PlayOneShot(endGameClip);
		while (isGrounded != true) yield return new WaitForSeconds(Time.deltaTime);

		BroadcastMessage("GameOver");
		boardScrape.Stop();
		rigidbody2D.isKinematic = true;
		foreach (Collider2D coll in GetComponents<Collider2D>()) {
			coll.enabled = false;
		}
		
		GA.API.Design.NewEvent("GameOver:Score", score.CurrentScore.points);
		GA.API.Design.NewEvent("GameOver:Distance", score.CurrentScore.distance);

		//PlayerPrefs.SetInt("HighScore", 
		spriteAnimator.SetTrigger("GameOver");
		
		yield return new WaitForSeconds(1);
		
		PlayMakerFSM.BroadcastEvent("GameOver");
	}
	
	IEnumerator AirTime() {
		float multiplier = baseAirTimeMultiplier;
		float lastMultiplierIncrease = Time.time;
		
		while (!isGrounded) {
			if (Time.time >= lastMultiplierIncrease + multiplierInterval) {
				score.AddMultiplier(multiplier);
				multiplier *= airMultiplierScaleFactor;
				lastMultiplierIncrease = Time.time;
			}
			
			yield return new WaitForSeconds(Time.deltaTime);
		}
	}
	
	IEnumerator Grounded() {
		float multiplier = baseGroundedMultiplier;
		float lastMultiplierDecrease = Time.time;
		
		while (isGrounded && !gameOver) {
			if (Time.time >= lastMultiplierDecrease + multiplierInterval) {
				score.AddMultiplier(multiplier);
				multiplier *= groundedMultiplierScaleFactor;
				lastMultiplierDecrease = Time.time;
			}
			
			yield return new WaitForSeconds(Time.deltaTime);
		}
	}

	void OnPause() {
		boardScrape.Pause();
	}
}
