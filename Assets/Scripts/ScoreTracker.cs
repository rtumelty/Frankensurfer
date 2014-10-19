using HutongGames.PlayMaker;
using UnityEngine;
using System.Collections;

public class ScoreTracker : Singleton<ScoreTracker> {
	Score currentScore;
	Score highScore;

	PlayMakerFSM stateManager;
	ParticleSystem multiplierParticles;

	public Score CurrentScore { 
		get {
			return currentScore;
		}
	}

	float startX;
	[SerializeField] GameObject surfer;

	// Use this for initialization
	void Start () {
		currentScore = new Score();
		GetHighScore();

		Setup();

		startX = surfer.transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		currentScore.distance = surfer.transform.position.x - startX;
		UpdateGUI();
	}

	void Setup() {
		if (multiplierParticles == null) {
			GameObject particlesGameObject = GameObject.Find("MultiplierParticles");
			if (particlesGameObject != null)
				multiplierParticles = particlesGameObject.GetComponent<ParticleSystem>() as ParticleSystem;
		}

		if (surfer == null) surfer = gameObject;
		
		if (stateManager == null)
			stateManager = GameObject.Find("_StateManager").GetComponent<PlayMakerFSM>() as PlayMakerFSM;
	}

	public void AddMultiplier(float multiplier) {
		currentScore.multiplier += multiplier;
		currentScore.multiplier = Mathf.Clamp(currentScore.multiplier, 1, Mathf.Infinity);

		if (multiplierParticles == null) Setup();

		if (multiplier > 0) multiplierParticles.Play();
	}

	public void AddPoints(int addedPoints, Pickup.PickupType type) {
		currentScore.points += addedPoints;

		switch (type) {
		case Pickup.PickupType.Headstone:
			currentScore.headstones++;
			break;
		case Pickup.PickupType.Crossbones:
			currentScore.crossbones++;
			break;
		case Pickup.PickupType.Skull:
			currentScore.skulls++;
			break;
		}
	}

	void UpdateGUI() {
		if (stateManager == null) Setup();

		if (stateManager != null) {
			stateManager.FsmVariables.GetFsmInt("Score").Value = currentScore.points;
			stateManager.FsmVariables.GetFsmString("Points").Value = currentScore.points.ToString();
			stateManager.FsmVariables.GetFsmString("Multiplier").Value = "x" + currentScore.multiplier.ToString("F1");
			stateManager.FsmVariables.GetFsmString("Distance").Value = Mathf.FloorToInt(currentScore.distance).ToString();
			stateManager.FsmVariables.GetFsmString("Headstones").Value = currentScore.headstones.ToString();
			stateManager.FsmVariables.GetFsmString("Crossbones").Value = currentScore.crossbones.ToString();
			stateManager.FsmVariables.GetFsmString("Skulls").Value = currentScore.skulls.ToString();
		} else Debug.LogError("State manager not loaded");
	}

	void GameOver() {
		SaveHighScore();
	}

	void SaveHighScore() {
		if (currentScore.points > highScore.points) {
			highScore = currentScore;
			
			PlayerPrefs.SetInt("points", currentScore.points);
			PlayerPrefs.SetFloat("multiplier", currentScore.multiplier);
			PlayerPrefs.SetFloat("distance", (int)currentScore.distance);
			PlayerPrefs.SetInt("headstones", currentScore.headstones);
			PlayerPrefs.SetInt("crossbones", currentScore.crossbones);
			PlayerPrefs.SetInt("skulls", currentScore.skulls);
		}
	}

	void GetHighScore() {
		if (highScore == null) highScore = new Score();

		highScore.points = PlayerPrefs.GetInt("points");
		highScore.multiplier = PlayerPrefs.GetFloat("multiplier");
		highScore.distance = PlayerPrefs.GetFloat("distance");
		highScore.headstones = PlayerPrefs.GetInt("headstones");
		highScore.crossbones = PlayerPrefs.GetInt("crossbones");
		highScore.skulls = PlayerPrefs.GetInt("skulls");

	}
}

[System.Serializable]
public class Score {
	public int points;
	public float multiplier = 1;
	public float distance;
	public int skulls;
	public int crossbones;
	public int headstones;
}