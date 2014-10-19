// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using System;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Time)]
	[HutongGames.PlayMaker.Tooltip("Gets system date and time info and stores it in a string variable. An optional format string gives you a lot of control over the formatting (see online docs for format syntax).")]
	public class GetSystemDateTime : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Tooltip("Store System DateTime as a string.")]
		public FsmString storeString;
		
		[HutongGames.PlayMaker.Tooltip("Optional format string. E.g., MM/dd/yyyy HH:mm")]
		public FsmString format;

		[HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			storeString = null;
			format = "MM/dd/yyyy HH:mm";
		}

		public override void OnEnter()
		{
			storeString.Value = DateTime.Now.ToString(format.Value);

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			storeString.Value = DateTime.Now.ToString(format.Value);
		}
	}
}

