// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[HutongGames.PlayMaker.Tooltip("Divides one Float by another.")]
	public class FloatDivide : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip("The float variable to divide.")]
		public FsmFloat floatVariable;

		[RequiredField]
		[HutongGames.PlayMaker.Tooltip("Divide the float variable by this value.")]
        public FsmFloat divideBy;

        [HutongGames.PlayMaker.Tooltip("Repeate every frame. Useful if the variables are changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			floatVariable = null;
			divideBy = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			floatVariable.Value /= divideBy.Value;
			
			if (!everyFrame)
			{
			    Finish();
			}
		}

		public override void OnUpdate()
		{
			floatVariable.Value /= divideBy.Value;
		}
	}
}