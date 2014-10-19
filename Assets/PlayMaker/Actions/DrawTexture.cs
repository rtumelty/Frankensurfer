// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUI)]
	[HutongGames.PlayMaker.Tooltip("Draws a GUI Texture. NOTE: Uses OnGUI so you need a PlayMakerGUI component in the scene.")]
	public class DrawTexture : FsmStateAction
	{
		[RequiredField]
        [HutongGames.PlayMaker.Tooltip("Texture to draw.")]
		public FsmTexture texture;
		
		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Tooltip("Rectangle on the screen to draw the texture within. Alternatively, set or override individual properties below.")]
		[Title("Position")]
		public FsmRect screenRect;

        [HutongGames.PlayMaker.Tooltip("Left screen coordinate.")]
		public FsmFloat left;

        [HutongGames.PlayMaker.Tooltip("Top screen coordinate.")]
        public FsmFloat top;

        [HutongGames.PlayMaker.Tooltip("Width of texture on screen.")]
        public FsmFloat width;

        [HutongGames.PlayMaker.Tooltip("Height of texture on screen.")]
		public FsmFloat height;

		[HutongGames.PlayMaker.Tooltip("How to scale the image when the aspect ratio of it doesn't fit the aspect ratio to be drawn within.")]
		public ScaleMode scaleMode;
	
		[HutongGames.PlayMaker.Tooltip("Whether to alpha blend the image on to the display (the default). If false, the picture is drawn on to the display.")]
		public FsmBool alphaBlend;
		
		[HutongGames.PlayMaker.Tooltip("Aspect ratio to use for the source image. If 0 (the default), the aspect ratio from the image is used. Pass in w/h for the desired aspect ratio. This allows the aspect ratio of the source image to be adjusted without changing the pixel width and height.")]
		public FsmFloat imageAspect;
		
		[HutongGames.PlayMaker.Tooltip("Use normalized screen coordinates (0-1)")]
		public FsmBool normalized;

		private Rect rect;
		
		public override void Reset()
		{
			texture = null;
			screenRect = null;
			left = 0;
			top = 0;
			width = 1;
			height = 1;
			scaleMode = ScaleMode.StretchToFill;
			alphaBlend = true;
			imageAspect = 0;
			normalized = true;
		}

		public override void OnGUI()
		{
			if (texture.Value == null)
			{
				return;
			}
			
			rect = !screenRect.IsNone ? screenRect.Value : new Rect();
			
			if (!left.IsNone) rect.x = left.Value;
			if (!top.IsNone) rect.y = top.Value;
			if (!width.IsNone) rect.width = width.Value;
			if (!height.IsNone) rect.height = height.Value;
			
			if (normalized.Value)
			{
				rect.x *= Screen.width;
				rect.width *= Screen.width;
				rect.y *= Screen.height;
				rect.height *= Screen.height;
			}
			
			GUI.DrawTexture(rect, texture.Value, scaleMode, alphaBlend.Value, imageAspect.Value);
		}
	}
}