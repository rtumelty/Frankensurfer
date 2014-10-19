// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Audio)]
	[HutongGames.PlayMaker.Tooltip("Mute/unmute the Audio Clip played by an Audio Source component on a Game Object.")]
	public class AudioMute : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(AudioSource))]
		[HutongGames.PlayMaker.Tooltip("The GameObject with an Audio Source component.")]
        public FsmOwnerDefault gameObject;
		
        [RequiredField]
        [HutongGames.PlayMaker.Tooltip("Check to mute, uncheck to unmute.")]
		public FsmBool mute;

		public override void Reset()
		{
			gameObject = null;
			mute = false;
		}

		public override void OnEnter()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go != null)
			{
				var audio = go.audio;
				if (audio != null)
				{
					audio.mute = mute.Value;
				}
			}
			
			Finish();
		}
	}
}