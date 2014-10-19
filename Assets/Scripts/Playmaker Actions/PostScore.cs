using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Facebook")]
	[Tooltip("Post score to Facebook")]
	public class PostScore : FsmStateAction
	{		
		[RequiredField]
		public FsmInt score;

		public override void OnEnter()
		{
			if (FacebookManager.Instance != null)
				FacebookManager.PostScore(score.Value);
			Finish();
		}

	}
}