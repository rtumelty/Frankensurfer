using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Facebook;        // Must be specified to use HTTpMethod class
using Facebook.MiniJSON;

public class FacebookManager : Singleton<FacebookManager>
{
	enum Permission {
		Waiting,
		Allowed,
		NotAllowed
	}

	struct UserData {
		public string userId;
		public string firstName;
		public Texture2D profilePic;
		public int highScore;
	}

	static bool isEnabled;
	static UserData playerData;
	static Permission permissionStatus;

	[SerializeField] UITexture[] profilePictures;
	[SerializeField] Texture defaultProfilePicture;
	[SerializeField] UILabel usernameLabel;
	[SerializeField] GameObject[] loginButtons;
	[SerializeField] GameObject[] logoutButtons;

	public new void Awake()
	{
		isEnabled = false;
		permissionStatus = Permission.Waiting;

		DontDestroyOnLoad(gameObject);
	}
	
	public void Start()
	{
		// Must call FB.Init Once
		FB.Init(SetInitFB, OnHideUnity);	
	}

	private void SetInitFB()
	{
		// This method method will be called withing the callback received by FB.Init()
		isEnabled = true;

		Debug.Log ("Facebook API: Initialized");

		if (FB.IsLoggedIn == true) {
			Debug.Log ("Facebook API: Already logged in");
			OnLogin();
		}
		else {
			foreach (GameObject button in Instance.logoutButtons) {
				button.SetActive(false);
			}
		}
	}
	
	private void OnHideUnity(bool isUnityVisible)
	{
		// This method method will be called within the callback received each time Unity gets or loses Focus (True/false)
		if (!isUnityVisible) {
			Debug.Log ("Facebook API: Unity hidden by API");
			Time.timeScale = 0;
		}
		else {
			Debug.Log ("Facebook API: Unity unhidden");
			Time.timeScale = 1;
		} 
	}	
	
	static void LoginCallback(FBResult result)
	{
		if (result.Error != null)
		{
			Debug.Log("Facebook API: Received login error :: " + result.Error.ToString());
		}
		else
		{
			if (FB.IsLoggedIn)
			{
				OnLogin();
			}
			else
			{
				// Case login failed (because of cancelling for example)
				Debug.Log("Facebook API: Login unsuccessful - " + result.Text);

			}
		}
	}

	static void OnLogin() {
		Debug.Log ("Facebook API: Logged in");
		playerData.userId = FB.UserId;
		
		foreach (GameObject button in Instance.loginButtons) {
			button.SetActive(false);
		}
		foreach (GameObject button in Instance.logoutButtons) {
			button.SetActive(true);
		}

		if (Application.internetReachability != NetworkReachability.NotReachable) {
			// Request user data
			FB.API("/me?fields=id,first_name", HttpMethod.GET, ReceivedUserData);

			// Request profile image
			FB.API("/me/picture?redirect=0&height=300&type=normal&width=300", HttpMethod.GET, ReceivedProfilePicture);
		}
		else {
			playerData.firstName = PlayerPrefs.GetString("firstName");
			if (playerData.firstName != "") {
				Instance.usernameLabel.text = "Played by " + playerData.firstName;
				Instance.usernameLabel.UpdateNGUIText();
			}

			string base64 = PlayerPrefs.GetString("profilePicture");
			if (base64 != "") {
				byte[] imageData = Convert.FromBase64String(base64);
				playerData.profilePic.LoadImage(imageData);

				SetProfileImages();
			}
		}
	}

	static void OnLogout() {
		PlayerPrefs.SetString("firstName", "");
		PlayerPrefs.SetString("profilePicture", "");

		Instance.usernameLabel.text = "Played by You";

		foreach (UITexture texture in Instance.profilePictures) {
			texture.mainTexture = Instance.defaultProfilePicture;
		}
		
		foreach (GameObject button in Instance.logoutButtons) {
			button.SetActive(false);
		}
		foreach (GameObject button in Instance.loginButtons) {
			button.SetActive(true);
		}
	}
	
	static void ReceivedUserData(FBResult response) {
		if (response.Error != null)
		{
			Debug.Log("Facebook API: Received callback get user data error :: " + response.Error.ToString());
		}
		else {
			Debug.Log(response.Text);
			Dictionary<string, object> dict = Json.Deserialize(response.Text) as Dictionary<string,object>;
			Debug.Log(dict.Count);

			object first_name;
			if (dict.TryGetValue("first_name", out first_name)) {
				playerData.firstName = first_name as string;
				PlayerPrefs.SetString("firstName", playerData.firstName);

				Instance.usernameLabel.text = "Played by " + playerData.firstName;
				Instance.usernameLabel.UpdateNGUIText();
			}
			else {
				Debug.Log("Facebook API: Data doesn't parse as user data - " + response.Text);
			}
		}
		
	}

	static void ReceivedProfilePicture(FBResult response) {
		if (response.Error != null)
		{
			Debug.Log("Facebook API: Receive callback get profile picture error :: " + response.Error.ToString());
		}
		else {
			Dictionary<string, object> dict = Json.Deserialize(response.Text) as Dictionary<string,object>;
			
			object responseData;
			if (dict.TryGetValue("data", out responseData)) {
				Dictionary<string,object> data = responseData as Dictionary<string,object>;
				
				object value;
				if (data.TryGetValue("is_silhouette", out value)) {
					if ((bool) value == true) {
						Debug.Log ("Facebook API: user uses default silhouette");
						playerData.profilePic = null;
						return;
					} else {
						if (data.TryGetValue("url", out value)) {
							Debug.Log ("Facebook API: retrieving profile picture");
							Instance.StartCoroutine("DownloadProfilePicture", value);
						}
						else
							Debug.Log ("Facebook API: could not retrieve url field");
					}
				}
				else 
					Debug.Log ("Facebook API: could not retrieve is_silhouette field");
			}
			else {
				Debug.LogWarning("Facebook API: Data doesn't parse as profile picture response - " + response.Text);
			}
		}
	}

	IEnumerator DownloadProfilePicture(string url) {
		Debug.Log ("Facebook API: profile picture download started");
		WWW www = new WWW(url);

		yield return www;

		if (www.error != "") {
			Debug.Log ("Facebook API: profile picture download finished");
			playerData.profilePic = www.texture;

			byte[] imageData = www.texture.EncodeToPNG();
			PlayerPrefs.SetString("profilePicture", Convert.ToBase64String(imageData));

			SetProfileImages();
		} 
		else {
			Debug.Log ("Facebook API: profile picture download error - " + www.error);
		}
	}

	static void SetProfileImages() {
		if (playerData.profilePic != null) {
			foreach (UITexture texture in Instance.profilePictures) {
				if (texture != null)
					texture.mainTexture = playerData.profilePic;
			}
		}
	}

	public static void Login() {
		if (!FB.IsLoggedIn) {
			Debug.Log ("Facebook API: Requesting login permissions");
			FB.Login("publish_actions", LoginCallback);
		}
	}

	public static void PostScore(int score) {
		if (FB.IsLoggedIn) {
			Debug.Log ("Facebook API: Posting score");
			Dictionary<string, string> scoreData = new Dictionary<string, string>() {{"score", score.ToString() }};
			FB.API("/me/scores", HttpMethod.POST, ScorePosted, scoreData);
		}
	}

	static void ScorePosted(FBResult response) {
		if (response.Error != null)
		{
			Debug.Log("Facebook API: Received callback score post error :: " + response.Error.ToString());
		}
		else {
			Debug.Log("Facebook API: score posted");
			PlayMakerFSM.BroadcastEvent("ScorePosted");
		}
	}

	static void HasPermission(FBResult response) {
		if (response.Error != null)
		{
			Debug.Log("Facebook API: Receive callback get permissions error :: " + response.Error.ToString());
		}
		else {
			Dictionary<string, object> dict = Json.Deserialize(response.Text) as Dictionary<string,object>;

			object permissionsList;
			if (dict.TryGetValue("data", out permissionsList)) {
				Dictionary<string,int> data = permissionsList as Dictionary<string,int>;

				int permission;
				if (data.TryGetValue("publish_actions", out permission)) {
					if (permission == 1) permissionStatus = Permission.Allowed;
					else permissionStatus = Permission.NotAllowed;
				}
			}
			else {
				Debug.LogWarning("Facebook API: Data doesn't parse as permissions - " + response.Text);
				permissionStatus = Permission.NotAllowed;
			}
		}
	}
}