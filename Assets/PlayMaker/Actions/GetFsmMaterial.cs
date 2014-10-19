﻿// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[HutongGames.PlayMaker.Tooltip("Get the value of a Material Variable from another FSM.")]
	public class GetFsmMaterial : FsmStateAction
	{
		[RequiredField]
		[HutongGames.PlayMaker.Tooltip("The GameObject that owns the FSM.")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmName)]
		[HutongGames.PlayMaker.Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;

		[RequiredField]
		[UIHint(UIHint.FsmMaterial)]
		public FsmString variableName;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmMaterial storeValue;

		[HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
		public bool everyFrame;

		GameObject goLastFrame;
		protected PlayMakerFSM fsm;

		public override void Reset()
		{
			gameObject = null;
			fsmName = "";
			variableName = "";
			storeValue = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetFsmVariable();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetFsmVariable();
		}

		void DoGetFsmVariable()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}

			// only get the fsm component if go has changed

			if (go != goLastFrame)
			{
				goLastFrame = go;
				fsm = ActionHelpers.GetGameObjectFsm(go, fsmName.Value);
			}

			if (fsm == null || storeValue == null)
			{
				return;
			}

			var fsmVar = fsm.FsmVariables.GetFsmMaterial(variableName.Value);

			if (fsmVar != null)
			{
				storeValue.Value = fsmVar.Value;
			}
		}

	}
}