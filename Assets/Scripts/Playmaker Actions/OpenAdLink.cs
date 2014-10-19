// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("AdManager")]
	[Tooltip("Opens a RevMob ad link")]
	public class OpenAdLink : FsmStateAction
	{		
		public override void OnEnter()
		{
#if UNITY_IPHONE || UNITY_ANDROID
//			if (AdManager.Instance != null)
//				AdManager.Instance.OpenLink();
#endif
			Finish();
		}

	}
}