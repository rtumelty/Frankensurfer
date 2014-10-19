// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[HutongGames.PlayMaker.Tooltip("Unparents all children from the Game Object.")]
	public class DetachChildren : FsmStateAction
	{
		[RequiredField]
        [HutongGames.PlayMaker.Tooltip("GameObject to unparent children from.")]
		public FsmOwnerDefault gameObject;

		public override void Reset()
		{
			gameObject = null;
		}

		public override void OnEnter()
		{
			DoDetachChildren(Fsm.GetOwnerDefaultTarget(gameObject));
			
			Finish();
		}

		static void DoDetachChildren(GameObject go)
		{
			if (go != null)
			{
				go.transform.DetachChildren();
			}
		}
	}
}