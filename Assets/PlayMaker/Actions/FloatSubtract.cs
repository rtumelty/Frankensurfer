// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.
// Simple custom action by Tobbe Olsson - www.tobbeo.net

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[HutongGames.PlayMaker.Tooltip("Subtracts a value from a Float Variable.")]
	public class FloatSubtract : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip("The float variable to subtract from.")]
		public FsmFloat floatVariable;

		[RequiredField]
        [HutongGames.PlayMaker.Tooltip("Value to subtract from the float variable.")]
		public FsmFloat subtract;

        [HutongGames.PlayMaker.Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

        [HutongGames.PlayMaker.Tooltip("Used with Every Frame. Adds the value over one second to make the operation frame rate independent.")]
        public bool perSecond;

		public override void Reset()
		{
			floatVariable = null;
			subtract = null;
			everyFrame = false;
			perSecond = false;
		}

		public override void OnEnter()
		{
			DoFloatSubtract();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoFloatSubtract();
		}

		void DoFloatSubtract()
		{
			if (!perSecond)
			{
				floatVariable.Value -= subtract.Value;
			}
			else
			{
				floatVariable.Value -= subtract.Value * Time.deltaTime;
			}
		}
	}
}