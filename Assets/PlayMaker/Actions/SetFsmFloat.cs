// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[HutongGames.PlayMaker.Tooltip("Set the value of a Float Variable in another FSM.")]
	public class SetFsmFloat : FsmStateAction
	{
		[RequiredField]
        [HutongGames.PlayMaker.Tooltip("The GameObject that owns the FSM.")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmName)]
		[HutongGames.PlayMaker.Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;

		[RequiredField]
		[UIHint(UIHint.FsmFloat)]
        [HutongGames.PlayMaker.Tooltip("The name of the FSM variable.")]
		public FsmString variableName;

		[RequiredField]
        [HutongGames.PlayMaker.Tooltip("Set the value of the variable.")]
		public FsmFloat setValue;

        [HutongGames.PlayMaker.Tooltip("Repeat every frame. Useful if the value is changing.")]
        public bool everyFrame;

		GameObject goLastFrame;
		PlayMakerFSM fsm;
		
		public override void Reset()
		{
			gameObject = null;
			fsmName = "";
			setValue = null;
		}

		public override void OnEnter()
		{
			DoSetFsmFloat();
			
			if (!everyFrame)
			{
			    Finish();
			}		
		}

		void DoSetFsmFloat()
		{
			if (setValue == null)
			{
			    return;
			}

			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
			    return;
			}
			
			if (go != goLastFrame)
			{
				goLastFrame = go;
				
				// only get the fsm component if go has changed
				
				fsm = ActionHelpers.GetGameObjectFsm(go, fsmName.Value);
			}			
			
			if (fsm == null)
			{
                LogWarning("Could not find FSM: " + fsmName.Value);
			    return;
			}
			
			var fsmFloat = fsm.FsmVariables.GetFsmFloat(variableName.Value);
			
			if (fsmFloat != null)
			{
			    fsmFloat.Value = setValue.Value;
			}
            else
            {
                LogWarning("Could not find variable: " + variableName.Value);
            }
		}

		public override void OnUpdate()
		{
			DoSetFsmFloat();
		}

	}
}

