// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUILayout)]
	[HutongGames.PlayMaker.Tooltip("GUILayout Box.")]
	public class GUILayoutBox : GUILayoutAction
	{
		[HutongGames.PlayMaker.Tooltip("Image to display in the Box.")]
		public FsmTexture image;

		[HutongGames.PlayMaker.Tooltip("Text to display in the Box.")]
		public FsmString text;

		[HutongGames.PlayMaker.Tooltip("Optional Tooltip string.")]
		public FsmString tooltip;

		[HutongGames.PlayMaker.Tooltip("Optional GUIStyle in the active GUISkin.")]
		public FsmString style;

		public override void Reset()
		{
			base.Reset();
			text = "";
			image = null;
			tooltip = "";
			style = "";
		}
		
		public override void OnGUI()
		{
			if (string.IsNullOrEmpty(style.Value))
			{
				GUILayout.Box(new GUIContent(text.Value, image.Value, tooltip.Value), LayoutOptions);
			}
			else
			{
				GUILayout.Box(new GUIContent(text.Value, image.Value, tooltip.Value), style.Value, LayoutOptions);
			}
		}
	}
}