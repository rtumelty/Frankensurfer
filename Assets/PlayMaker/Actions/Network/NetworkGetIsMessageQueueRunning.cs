// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[HutongGames.PlayMaker.Tooltip("Get if network messages are enabled or disabled.\n\nIf disabled no RPC call execution or network view synchronization takes place")]
	public class NetworkGetIsMessageQueueRunning : FsmStateAction
	{
		[HutongGames.PlayMaker.Tooltip("Is Message Queue Running. If this is disabled no RPC call execution or network view synchronization takes place")]
		[UIHint(UIHint.Variable)]
		public FsmBool result;

		public override void Reset()
		{
			result = null;
		}

		public override void OnEnter()
		{
			result.Value = Network.isMessageQueueRunning;
			
			Finish();
		}
	}
}

#endif