// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8)

using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[HutongGames.PlayMaker.Tooltip("Check if this machine has a public IP address.")]
	public class NetworkHavePublicIpAddress : FsmStateAction
	{	
		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Tooltip("True if this machine has a public IP address")]
		public FsmBool havePublicIpAddress;

		[HutongGames.PlayMaker.Tooltip("Event to send if this machine has a public IP address")]
		public FsmEvent publicIpAddressFoundEvent;

		[HutongGames.PlayMaker.Tooltip("Event to send if this machine has no public IP address")]
		public FsmEvent publicIpAddressNotFoundEvent;

		public override void Reset()
		{
			havePublicIpAddress = null;
			publicIpAddressFoundEvent = null;
			publicIpAddressNotFoundEvent =null;			
		}

		public override void OnEnter()
		{
			
			bool _publicIpAddress = Network.HavePublicAddress();
			
			havePublicIpAddress.Value = _publicIpAddress;

			if (_publicIpAddress && publicIpAddressFoundEvent != null)
			{
				Fsm.Event(publicIpAddressFoundEvent);
			}
			else if (!_publicIpAddress && publicIpAddressNotFoundEvent != null)
			{
				Fsm.Event(publicIpAddressNotFoundEvent);
			}

			Finish();
		}
	}
}

#endif