// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[HutongGames.PlayMaker.Tooltip("Destroys a Game Object.")]
	public class DestroyObject : FsmStateAction
	{
		[RequiredField]
		[HutongGames.PlayMaker.Tooltip("The GameObject to destroy.")]
		public FsmGameObject gameObject;

		[HasFloatSlider(0, 5)]
		[HutongGames.PlayMaker.Tooltip("Optional delay before destroying the Game Object.")]
		public FsmFloat delay;

		[HutongGames.PlayMaker.Tooltip("Detach children before destroying the Game Object.")]
		public FsmBool detachChildren;
		//public FsmEvent sendEvent;

		//DelayedEvent delayedEvent;

		public override void Reset()
		{
			gameObject = null;
			delay = 0;
			//sendEvent = null;
		}

		public override void OnEnter()
		{
			var go = gameObject.Value;
			
			if (go != null)
			{
				if (delay.Value <= 0)
				{
				    Object.Destroy(go);
				}
				else
				{
				    Object.Destroy(go, delay.Value);
				}
	
				if (detachChildren.Value)
					go.transform.DetachChildren();
			}
			
			Finish();
			//delayedEvent = new DelayedEvent(Fsm, sendEvent, delay.Value);
		}

		public override void OnUpdate()
		{
			//delayedEvent.Update();
		}

	}
}