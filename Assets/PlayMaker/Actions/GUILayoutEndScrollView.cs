// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUILayout)]
	[HutongGames.PlayMaker.Tooltip("Close a group started with GUILayout Begin ScrollView.")]
	public class GUILayoutEndScrollView : FsmStateAction
	{
		public override void OnGUI()
		{
			GUILayout.EndScrollView();
		}
	}
}