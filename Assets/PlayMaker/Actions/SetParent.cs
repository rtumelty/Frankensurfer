// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[HutongGames.PlayMaker.Tooltip("Sets the Parent of a Game Object.")]
	public class SetParent : FsmStateAction
	{
		[RequiredField]
		[HutongGames.PlayMaker.Tooltip("The Game Object to parent.")]
		public FsmOwnerDefault gameObject;
		
		[HutongGames.PlayMaker.Tooltip("The new parent for the Game Object.")]
		public FsmGameObject parent;

		[HutongGames.PlayMaker.Tooltip("Set the local position to 0,0,0 after parenting.")]
		public FsmBool resetLocalPosition;

		[HutongGames.PlayMaker.Tooltip("Set the local rotation to 0,0,0 after parenting.")]
		public FsmBool resetLocalRotation;

		public override void Reset()
		{
			gameObject = null;
			parent = null;
			resetLocalPosition = null;
			resetLocalRotation = null;
		}

		public override void OnEnter()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);

			if (go != null)
			{
				go.transform.parent = parent.Value == null ? null : parent.Value.transform;

				if (resetLocalPosition.Value)
				{
					go.transform.localPosition = Vector3.zero;
				}

				if (resetLocalRotation.Value)
				{
					go.transform.localRotation = Quaternion.identity;
				}
			}
			
			Finish();
		}
	}
}