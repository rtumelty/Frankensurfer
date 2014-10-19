// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUILayout)]
	[HutongGames.PlayMaker.Tooltip("GUILayout Label for a Float Variable.")]
	public class GUILayoutFloatLabel : GUILayoutAction
	{
		[HutongGames.PlayMaker.Tooltip("Text to put before the float variable.")]
		public FsmString prefix;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Tooltip("Float variable to display.")]
		public FsmFloat floatVariable;

		[HutongGames.PlayMaker.Tooltip("Optional GUIStyle in the active GUISKin.")]
		public FsmString style;

		public override void Reset()
		{
			base.Reset();
			prefix = "";
			style = "";
			floatVariable = null;
		}

		public override void OnGUI()
		{
			if (string.IsNullOrEmpty(style.Value))
			{
				GUILayout.Label(new GUIContent(prefix.Value + floatVariable.Value), LayoutOptions);
			}
			else
			{
				GUILayout.Label(new GUIContent(prefix.Value + floatVariable.Value), style.Value, LayoutOptions);
			}
		}
	}
}