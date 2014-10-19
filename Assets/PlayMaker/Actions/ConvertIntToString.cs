// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Convert)]
	[HutongGames.PlayMaker.Tooltip("Converts an Integer value to a String value with an optional format.")]
	public class ConvertIntToString : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Tooltip("The Int variable to convert.")]
		public FsmInt intVariable;
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Tooltip("A String variable to store the converted value.")]
		public FsmString stringVariable;
        
		[HutongGames.PlayMaker.Tooltip("Optional Format, allows for leading zeroes. E.g., 0000")]
        public FsmString format;
		
		[HutongGames.PlayMaker.Tooltip("Repeat every frame. Useful if the Int variable is changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			intVariable = null;
			stringVariable = null;
			everyFrame = false;
            format = null;
		}

		public override void OnEnter()
		{
			DoConvertIntToString();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			DoConvertIntToString();
		}
		
		void DoConvertIntToString()
		{
            if (format.IsNone || string.IsNullOrEmpty(format.Value))
            {
            	stringVariable.Value = intVariable.Value.ToString();
            }
            else
            {
            	stringVariable.Value = intVariable.Value.ToString(format.Value);
            }
		}
	}
}