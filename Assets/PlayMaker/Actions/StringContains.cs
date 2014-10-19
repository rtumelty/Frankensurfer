// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[HutongGames.PlayMaker.Tooltip("Tests if a String contains another String.")]
	public class StringContains : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Tooltip("The String variable to test.")]
		public FsmString stringVariable;
		[RequiredField]
		[HutongGames.PlayMaker.Tooltip("Test if the String variable contains this string.")]
		public FsmString containsString;
		[HutongGames.PlayMaker.Tooltip("Event to send if true.")]
		public FsmEvent trueEvent;
		[HutongGames.PlayMaker.Tooltip("Event to send if false.")]
		public FsmEvent falseEvent;
		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Tooltip("Store the true/false result in a bool variable.")]
		public FsmBool storeResult;
		[HutongGames.PlayMaker.Tooltip("Repeat every frame. Useful if any of the strings are changing over time.")]
		public bool everyFrame;

		public override void Reset()
		{
			stringVariable = null;
			containsString = "";
			trueEvent = null;
			falseEvent = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoStringContains();
			
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoStringContains();
		}
		
		void DoStringContains()
		{
			if (stringVariable.IsNone || containsString.IsNone) return;
			
			var contains =  stringVariable.Value.Contains(containsString.Value);

			if (storeResult != null)
			{
				storeResult.Value = contains;
			}

			if (contains && trueEvent != null)
			{
				Fsm.Event(trueEvent);
				return;
			}

			if (!contains && falseEvent != null)
			{
				Fsm.Event(falseEvent);
			}

		}
		
	}
}