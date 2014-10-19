using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Facebook")]
	[Tooltip("Log in to Facebook via API")]
	public class Login : FsmStateAction
	{		
		public override void OnEnter()
		{
			if (FacebookManager.Instance != null)
				FacebookManager.Login();
			Finish();
		}
		
	}
}