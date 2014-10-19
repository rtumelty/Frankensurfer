// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[HutongGames.PlayMaker.Tooltip("Get the number of connected players.\n\nOn a client this returns 1 (the server).")]
	public class NetworkGetConnectionsCount : FsmStateAction
	{
		[HutongGames.PlayMaker.Tooltip("Number of connected players.")]
		[UIHint(UIHint.Variable)]
		public FsmInt connectionsCount;
		
		[HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			connectionsCount = null;
			everyFrame = true;
		}

		public override void OnEnter()
		{
			connectionsCount.Value = Network.connections.Length;
			
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			connectionsCount.Value = Network.connections.Length;
		}

	}
}

#endif