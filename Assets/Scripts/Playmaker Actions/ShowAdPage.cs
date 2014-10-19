// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("AdManager")]
	[Tooltip("Shows a pop-up advertisment")]
	public class ShowAdPage : FsmStateAction
	{		
		[RequiredField]
        [Tooltip("Show RevMob ad. Random if all networks are enabled.")]
		public bool revMob;
		
		[RequiredField]
		[Tooltip("Show Chartboost ad. Random if all networks are enabled.")]
		public bool chartboost; 

		[Tooltip("Event to send if playing in editor / standalone.")]
		public FsmEvent editorEvent;

		public override void Reset()
		{
			revMob = false;
			chartboost = false;
			editorEvent = null;
		}

		public override void OnEnter()
		{
#if UNITY_EDITOR
			Finish ();
			Fsm.Event(editorEvent);
#elif UNITY_IPHONE || UNITY_ANDROID
			if (AdManager.Instance != null)
				DoShowAdPage();
#else
			Finish ();
			Fsm.Event(editorEvent);
#endif
		}

		void DoShowAdPage() {
			if (revMob && chartboost) {
				AdManager.Instance.ShowFullscreenAd(AdManager.AdType.Random);
			} else if (revMob) {
				AdManager.Instance.ShowFullscreenAd(AdManager.AdType.RevMob);
			} else if (chartboost) {
				AdManager.Instance.ShowFullscreenAd(AdManager.AdType.Chartboost);
			}

			Finish();
		}
	}
}