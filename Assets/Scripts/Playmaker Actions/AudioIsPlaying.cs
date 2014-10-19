// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Audio)]
	[Tooltip("Checks if audio source is playing.")]
	public class AudioIsPlaying : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(AudioSource))]
		[Tooltip("The GameObject with an AudioSource component.")]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Event to send if AudioSource is playing.")]
		public FsmEvent playingEvent;
		
		[Tooltip("Event to send if AudioSource is not playing.")]
		public FsmEvent notPlayingEvent;

		public bool everyFrame;

		private AudioSource audio;
				
		public override void Reset()
		{
			gameObject = null;
			playingEvent = null;
			playingEvent = null;
			notPlayingEvent = null;
		}

		public override void OnEnter()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go != null)
			{
				// cache the AudioSource component
				
				audio = go.audio;

				if (audio == null) {
					ActionHelpers.DebugLog(Fsm, LogLevel.Error, "No AudioSource on specified GameObject");
					Finish();
				}
			}

			DoCheckAudioPlaying();
			
			// Finish if not everyFrame
			if (!everyFrame)
				Finish();
		}
		
		public override void OnUpdate ()
		{
			DoCheckAudioPlaying();
		}

		void DoCheckAudioPlaying() {
			if (audio == null)
			{
				Finish();
			}
			else
			{
				Debug.Log(audio + " " + audio.isPlaying);
				if (audio.isPlaying)
				{
					if (playingEvent != null)
						Fsm.Event(playingEvent);
				}
				else
				{
					if (notPlayingEvent != null)
						Fsm.Event(notPlayingEvent);
				}
			}
		}
	}
}