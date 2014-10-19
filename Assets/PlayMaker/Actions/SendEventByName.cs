// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[HutongGames.PlayMaker.Tooltip("Sends an Event by name after an optional delay. NOTE: Use this over Send Event if you store events as string variables.")]
	public class SendEventByName : FsmStateAction
	{
		[HutongGames.PlayMaker.Tooltip("Where to send the event.")]
		public FsmEventTarget eventTarget;
		
		[RequiredField]
		[HutongGames.PlayMaker.Tooltip("The event to send. NOTE: Events must be marked Global to send between FSMs.")]
		public FsmString sendEvent;
		
		[HasFloatSlider(0, 10)]
		[HutongGames.PlayMaker.Tooltip("Optional delay in seconds.")]
		public FsmFloat delay;
		
		[HutongGames.PlayMaker.Tooltip("Repeat every frame. Rarely needed.")]
		public bool everyFrame;

		DelayedEvent delayedEvent;

		public override void Reset()
		{
			eventTarget = null;
			sendEvent = null;
			delay = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			if (delay.Value < 0.001f)
			{
				Fsm.Event(eventTarget, sendEvent.Value);
				Finish();
			}
			else
			{
				delayedEvent = Fsm.DelayedEvent(eventTarget, FsmEvent.GetFsmEvent(sendEvent.Value), delay.Value);
			}
		}

		public override void OnUpdate()
		{
			if (!everyFrame)
			{
				if (DelayedEvent.WasSent(delayedEvent))
				{
					Finish();
				}
			}
		}
	}
}