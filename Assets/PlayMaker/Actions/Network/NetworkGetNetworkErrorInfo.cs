// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[HutongGames.PlayMaker.Tooltip("Get the network OnFailedToConnect or MasterServer OnFailedToConnectToMasterServer connection error message.")]
	public class NetworkGetOnFailedToConnectProperties : FsmStateAction
	{		
		
		
		[HutongGames.PlayMaker.Tooltip("Error label")]
		[UIHint(UIHint.Variable)]
		public FsmString errorLabel;
		
		[HutongGames.PlayMaker.Tooltip("No error occurred.")]
		public FsmEvent NoErrorEvent;
		[HutongGames.PlayMaker.Tooltip("We presented an RSA public key which does not match what the system we connected to is using.")]
		public FsmEvent RSAPublicKeyMismatchEvent;
		[HutongGames.PlayMaker.Tooltip("The server is using a password and has refused our connection because we did not set the correct password.")]
		public FsmEvent InvalidPasswordEvent;
		[HutongGames.PlayMaker.Tooltip("onnection attempt failed, possibly because of internal connectivity problems.")]
		public FsmEvent ConnectionFailedEvent;
		[HutongGames.PlayMaker.Tooltip("The server is at full capacity, failed to connect.")]
		public FsmEvent TooManyConnectedPlayersEvent;
		[HutongGames.PlayMaker.Tooltip("We are banned from the system we attempted to connect to (likely temporarily).")]
		public FsmEvent ConnectionBannedEvent;
		[HutongGames.PlayMaker.Tooltip("We are already connected to this particular server (can happen after fast disconnect/reconnect).")]
		public FsmEvent AlreadyConnectedToServerEvent;
		[HutongGames.PlayMaker.Tooltip("Cannot connect to two servers at once. Close the connection before connecting again.")]
		public FsmEvent AlreadyConnectedToAnotherServerEvent;
		[HutongGames.PlayMaker.Tooltip("Internal error while attempting to initialize network interface. Socket possibly already in use.")]
		public FsmEvent CreateSocketOrThreadFailureEvent;
		[HutongGames.PlayMaker.Tooltip("Incorrect parameters given to Connect function.")]
		public FsmEvent IncorrectParametersEvent;
		[HutongGames.PlayMaker.Tooltip("No host target given in Connect.")]
		public FsmEvent EmptyConnectTargetEvent;
		[HutongGames.PlayMaker.Tooltip("Client could not connect internally to same network NAT enabled server.")]
		public FsmEvent InternalDirectConnectFailedEvent;
		[HutongGames.PlayMaker.Tooltip("The NAT target we are trying to connect to is not connected to the facilitator server.")]
		public FsmEvent NATTargetNotConnectedEvent;
		[HutongGames.PlayMaker.Tooltip("Connection lost while attempting to connect to NAT target.")]
		public FsmEvent NATTargetConnectionLostEvent;
		[HutongGames.PlayMaker.Tooltip("NAT punchthrough attempt has failed. The cause could be a too restrictive NAT implementation on either endpoints.")]
		public FsmEvent NATPunchthroughFailedEvent;

		
		public override void Reset ()
		{
			errorLabel = null;
			NoErrorEvent = null;
			RSAPublicKeyMismatchEvent = null;
			InvalidPasswordEvent = null;
			ConnectionFailedEvent = null;
			TooManyConnectedPlayersEvent = null;
			ConnectionBannedEvent = null;
			AlreadyConnectedToServerEvent = null;
			AlreadyConnectedToAnotherServerEvent = null;
			CreateSocketOrThreadFailureEvent = null;
			IncorrectParametersEvent = null;
			EmptyConnectTargetEvent = null;
			InternalDirectConnectFailedEvent = null;
			NATTargetNotConnectedEvent = null;
			NATTargetConnectionLostEvent = null;
			NATPunchthroughFailedEvent = null;
		}
		
		
		public override void OnEnter ()
		{
			doGetNetworkErrorInfo();
			
			Finish();
		}
		
		void doGetNetworkErrorInfo()
		{
			NetworkConnectionError _networkConnectionError = Fsm.EventData.ConnectionError;
		/* How do you check that Fsm.EventData.ConnectionError is indeed set, since it's an enum?
			if (_networkConnectionError == null) {
				LogError ("NetworkConnectionError data is null");
				return;
			}
		*/
			errorLabel.Value = _networkConnectionError.ToString();
			
			switch (_networkConnectionError) {
			case NetworkConnectionError.NoError:
				if (NoErrorEvent != null) {
					Fsm.Event (NoErrorEvent);
				}
				break;
			case NetworkConnectionError.RSAPublicKeyMismatch:
				if (RSAPublicKeyMismatchEvent != null) {
					Fsm.Event (RSAPublicKeyMismatchEvent);
				}
				break;
			case NetworkConnectionError.InvalidPassword:
				if (InvalidPasswordEvent != null) {
					Fsm.Event (InvalidPasswordEvent);
				}
				break;
			case NetworkConnectionError.ConnectionFailed:
				if (ConnectionFailedEvent != null) {
					Fsm.Event (ConnectionFailedEvent);
				}
				break;
			case NetworkConnectionError.TooManyConnectedPlayers:
				if (TooManyConnectedPlayersEvent != null) {
					Fsm.Event (TooManyConnectedPlayersEvent);
				}
				break;
			case NetworkConnectionError.ConnectionBanned:
				if (ConnectionBannedEvent != null) {
					Fsm.Event (ConnectionBannedEvent);
				}
				break;
			case NetworkConnectionError.AlreadyConnectedToServer:
				if (AlreadyConnectedToServerEvent != null) {
					Fsm.Event (AlreadyConnectedToServerEvent);
				}
				break;
			case NetworkConnectionError.AlreadyConnectedToAnotherServer:
				if (AlreadyConnectedToAnotherServerEvent != null) {
					Fsm.Event (AlreadyConnectedToAnotherServerEvent);
				}
				break;
			case NetworkConnectionError.CreateSocketOrThreadFailure:
				if (CreateSocketOrThreadFailureEvent != null) {
					Fsm.Event (CreateSocketOrThreadFailureEvent);
				}
				break;
			case NetworkConnectionError.IncorrectParameters:
				if (IncorrectParametersEvent != null) {
					Fsm.Event (IncorrectParametersEvent);
				}
				break;
			case NetworkConnectionError.EmptyConnectTarget:
				if (EmptyConnectTargetEvent != null) {
					Fsm.Event (EmptyConnectTargetEvent);
				}
				break;
			case NetworkConnectionError.InternalDirectConnectFailed:
				if (InternalDirectConnectFailedEvent != null) {
					Fsm.Event (InternalDirectConnectFailedEvent);
				}
				break;
			case NetworkConnectionError.NATTargetNotConnected:
				if (NATTargetNotConnectedEvent != null) {
					Fsm.Event (NATTargetNotConnectedEvent);
				}
				break;
			case NetworkConnectionError.NATTargetConnectionLost:
				if (NATTargetConnectionLostEvent != null) {
					Fsm.Event (NATTargetConnectionLostEvent);
				}
				break;
			case NetworkConnectionError.NATPunchthroughFailed:
				if (NATPunchthroughFailedEvent != null) {
					Fsm.Event (NoErrorEvent);
				}
				break;
				
			}

		}
		
	}
}

#endif