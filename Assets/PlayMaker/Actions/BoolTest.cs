// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[HutongGames.PlayMaker.Tooltip("Sends Events based on the value of a Boolean Variable.")]
	public class BoolTest : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip("The Bool variable to test.")]
		public FsmBool boolVariable;

        [HutongGames.PlayMaker.Tooltip("Event to send if the Bool variable is True.")]
		public FsmEvent isTrue;

        [HutongGames.PlayMaker.Tooltip("Event to send if the Bool variable is False.")]
		public FsmEvent isFalse;

        [HutongGames.PlayMaker.Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		public override void Reset()
		{
			boolVariable = null;
			isTrue = null;
			isFalse = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			Fsm.Event(boolVariable.Value ? isTrue : isFalse);
			
			if (!everyFrame)
			{
			    Finish();
			}
		}
		
		public override void OnUpdate()
		{
			Fsm.Event(boolVariable.Value ? isTrue : isFalse);
		}
	}
}