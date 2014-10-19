// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[HutongGames.PlayMaker.Tooltip("Get the last ping time to the given player in milliseconds. \n" +
		"If the player can't be found -1 will be returned. Pings are automatically sent out every couple of seconds.")]
	public class NetworkGetLastPing : FsmStateAction
	{
		[ActionSection("Setup")]

		[RequiredField]
		[HutongGames.PlayMaker.Tooltip("The Index of the player in the network connections list.")]
		[UIHint(UIHint.Variable)]
		public FsmInt playerIndex;
		
		[HutongGames.PlayMaker.Tooltip("The player reference is cached, that is if the connections list changes, the player reference remains.")]
		public bool cachePlayerReference = true;
		
		public bool everyFrame;
		
		[ActionSection("Result")]

		[RequiredField]
		[HutongGames.PlayMaker.Tooltip("Get the last ping time to the given player in milliseconds.")]
		[UIHint(UIHint.Variable)]
		public FsmInt lastPing;

		[HutongGames.PlayMaker.Tooltip("Event to send if the player can't be found. Average Ping is set to -1.")]
		public FsmEvent PlayerNotFoundEvent;

		[HutongGames.PlayMaker.Tooltip("Event to send if the player is found (pings back).")]
		public FsmEvent PlayerFoundEvent;
		
		
		private NetworkPlayer _player;
		
		public override void Reset()
		{
			playerIndex = null;
			lastPing = null;
			PlayerNotFoundEvent = null;
			PlayerFoundEvent = null;
			cachePlayerReference = true;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			if (cachePlayerReference){
				_player = Network.connections[playerIndex.Value];
			}
			
			GetLastPing();
			
			if(!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			GetLastPing();
		}
		
		void GetLastPing()
		{
		
			if (!cachePlayerReference){

				_player = Network.connections[playerIndex.Value];
			}
			
			int _lastPing = Network.GetLastPing(_player);
			lastPing.Value = _lastPing;
			
			if (_lastPing ==-1 && PlayerNotFoundEvent != null){
				Fsm.Event(PlayerNotFoundEvent);
			}
			
			if (_lastPing!=-1 && PlayerFoundEvent !=null)
			{
				Fsm.Event(PlayerFoundEvent);
			}
			
			
		}
	}
}

#endif