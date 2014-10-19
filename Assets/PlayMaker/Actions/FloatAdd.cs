// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[HutongGames.PlayMaker.Tooltip("Adds a value to a Float Variable.")]
	public class FloatAdd : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip("The Float variable to add to.")]
		public FsmFloat floatVariable;

		[RequiredField]
        [HutongGames.PlayMaker.Tooltip("Amount to add.")]
		public FsmFloat add;

        [HutongGames.PlayMaker.Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

        [HutongGames.PlayMaker.Tooltip("Used with Every Frame. Adds the value over one second to make the operation frame rate independent.")]
		public bool perSecond;

		public override void Reset()
		{
			floatVariable = null;
			add = null;
			everyFrame = false;
			perSecond = false;
		}

		public override void OnEnter()
		{
			DoFloatAdd();
			
			if (!everyFrame)
			{
			    Finish();
			}
		}

		public override void OnUpdate()
		{
			DoFloatAdd();
		}
		
		void DoFloatAdd()
		{
			if (!perSecond)
			{
			    floatVariable.Value += add.Value;
			}
			else
			{
			    floatVariable.Value += add.Value * Time.deltaTime;
			}
		}
	}
}