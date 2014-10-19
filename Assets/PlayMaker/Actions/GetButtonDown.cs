// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Input)]
	[HutongGames.PlayMaker.Tooltip("Sends an Event when a Button is pressed.")]
	public class GetButtonDown : FsmStateAction
	{
		[RequiredField]
        [HutongGames.PlayMaker.Tooltip("The name of the button. Set in the Unity Input Manager.")]
		public FsmString buttonName;

        [HutongGames.PlayMaker.Tooltip("Event to send if the button is pressed.")]
		public FsmEvent sendEvent;

        [HutongGames.PlayMaker.Tooltip("Set to True if the button is pressed.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;
		
		public override void Reset()
		{
			buttonName = "Fire1";
			sendEvent = null;
			storeResult = null;
		}

		public override void OnUpdate()
		{
			var buttonDown = Input.GetButtonDown(buttonName.Value);
			
			if (buttonDown)
			{
			    Fsm.Event(sendEvent);
			}
			
			storeResult.Value = buttonDown;
		}
	}
}