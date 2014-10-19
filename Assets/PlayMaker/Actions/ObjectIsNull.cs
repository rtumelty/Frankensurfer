// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[HutongGames.PlayMaker.Tooltip("Tests if an Object Variable has a null value.")]
	public class ObjectIsNull : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip("The Object variable to test.")]
		public FsmObject fsmObject;

        [HutongGames.PlayMaker.Tooltip("Event to send if the Object is null.")]
		public FsmEvent isNull;

        [HutongGames.PlayMaker.Tooltip("Event to send if the Object is NOT null.")]
		public FsmEvent isNotNull;

		[UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip("Store the result in a bool variable.")]
		public FsmBool storeResult;

        [HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			fsmObject = null;
			isNull = null;
			isNotNull = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoIsObjectNull();
			
			if (!everyFrame)
			{
			    Finish();
			}
		}

		public override void OnUpdate()
		{
			DoIsObjectNull();
		}
		
		void DoIsObjectNull()
		{
			var oIsNull = fsmObject.Value == null;

			if (storeResult != null)
			{
			    storeResult.Value = oIsNull;
			}

			Fsm.Event(oIsNull ? isNull : isNotNull);
		}
	}
}