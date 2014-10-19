// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[HutongGames.PlayMaker.Tooltip("Gets the number of children that a GameObject has.")]
	public class GetChildCount : FsmStateAction
	{
		[RequiredField]
        [HutongGames.PlayMaker.Tooltip("The GameObject to test.")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip("Store the number of children in an int variable.")]
		public FsmInt storeResult;

		public override void Reset()
		{
			gameObject = null;
			storeResult = null;
		}

		public override void OnEnter()
		{
			DoGetChildCount();
			
			Finish();
		}

		void DoGetChildCount()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
			    return;
			}

			storeResult.Value = go.transform.childCount;
		}
	}
}