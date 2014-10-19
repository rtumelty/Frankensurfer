// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[HutongGames.PlayMaker.Tooltip("Finds the Child of a GameObject by Name.\nNote, you can specify a path to the child, e.g., LeftShoulder/Arm/Hand/Finger. If you need to specify a tag, use GetChild.")]
	public class FindChild : FsmStateAction
	{
		[RequiredField]
        [HutongGames.PlayMaker.Tooltip("The GameObject to search.")]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[HutongGames.PlayMaker.Tooltip("The name of the child. Note, you can specify a path to the child, e.g., LeftShoulder/Arm/Hand/Finger")]
		public FsmString childName;
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Tooltip("Store the child in a GameObject variable.")]
		public FsmGameObject storeResult;

		public override void Reset()
		{
			gameObject = null;
			childName = "";
			storeResult = null;
		}

		public override void OnEnter()
		{
			DoFindChild();
			
			Finish();
		}

		void DoFindChild()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}

			var transform = go.transform.FindChild(childName.Value);
			storeResult.Value = transform != null ? transform.gameObject : null;
		}
	}
}