// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Camera)]
	[HutongGames.PlayMaker.Tooltip("Fade from a fullscreen Color. NOTE: Uses OnGUI so requires a PlayMakerGUI component in the scene.")]
	public class CameraFadeIn : FsmStateAction
	{
		[RequiredField]
        [HutongGames.PlayMaker.Tooltip("Color to fade from. E.g., Fade up from black.")]
		public FsmColor color;

		[RequiredField]
		[HasFloatSlider(0,10)]
        [HutongGames.PlayMaker.Tooltip("Fade in time in seconds.")]
		public FsmFloat time;

        [HutongGames.PlayMaker.Tooltip("Event to send when finished.")]
		public FsmEvent finishEvent;

		[HutongGames.PlayMaker.Tooltip("Ignore TimeScale. Useful if the game is paused.")]
		public bool realTime;
		
		public override void Reset()
		{
			color = Color.black;
			time = 1.0f;
			finishEvent = null;
		}
		
		private float startTime;
		private float currentTime;
		private Color colorLerp;
		
		public override void OnEnter()
		{
			startTime = FsmTime.RealtimeSinceStartup;
			currentTime = 0f;
			colorLerp = color.Value;
		}

		public override void OnUpdate()
		{
			if (realTime)
			{
				currentTime = FsmTime.RealtimeSinceStartup - startTime;
			}
			else
			{
				currentTime += Time.deltaTime;
			}
			
			colorLerp = Color.Lerp(color.Value, Color.clear, currentTime/time.Value);
			
			if (currentTime > time.Value)
			{
				if (finishEvent != null)
				{
				    Fsm.Event(finishEvent);
				}
				
				Finish();
			}
		}
		
		public override void OnGUI()
		{
			var guiColor = GUI.color;
			GUI.color = colorLerp;
			GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), ActionHelpers.WhiteTexture);
			GUI.color = guiColor;
		}
		
		
	}
}