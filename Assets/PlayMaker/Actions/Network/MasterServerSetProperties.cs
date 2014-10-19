// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[HutongGames.PlayMaker.Tooltip("Set the IP address, port, update rate and dedicated server flag of the master server.")]
	public class MasterServerSetProperties : FsmStateAction
	{
		[HutongGames.PlayMaker.Tooltip("Set the IP address of the master server.")]
		public FsmString ipAddress;
		
		[HutongGames.PlayMaker.Tooltip("Set the connection port of the master server.")]
		public FsmInt port;
		
		[HutongGames.PlayMaker.Tooltip("Set the minimum update rate for master server host information update. Default is 60 seconds.")]
		public FsmInt updateRate;
		
		[HutongGames.PlayMaker.Tooltip("Set if this machine is a dedicated server.")]
		public FsmBool dedicatedServer;
		
		public override void Reset()
		{
			ipAddress = "127.0.0.1";
			port = 10002;
			updateRate = 60;
			dedicatedServer = false;	
		}

		public override void OnEnter()
		{
			SetMasterServerProperties();
			
			Finish();
		}

		void SetMasterServerProperties()
		{			
				MasterServer.ipAddress = ipAddress.Value;
			
				MasterServer.port = port.Value;
			
				MasterServer.updateRate = updateRate.Value;
			
				MasterServer.dedicatedServer = dedicatedServer.Value;			
		}
	}
}

#endif