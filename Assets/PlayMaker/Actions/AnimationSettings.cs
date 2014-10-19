// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Animation)]
	[HutongGames.PlayMaker.Tooltip("Set the Wrap Mode, Blend Mode, Layer and Speed of an Animation.\nNOTE: Settings are applied once, on entering the state, NOT continuously. To dynamically control an animation's settings, use Set Animation Speede etc.")]
	public class AnimationSettings : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Animation))]
		[HutongGames.PlayMaker.Tooltip("A GameObject with an Animation Component.")]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[UIHint(UIHint.Animation)]
		[HutongGames.PlayMaker.Tooltip("The name of the animation.")]
		public FsmString animName;
		
		[HutongGames.PlayMaker.Tooltip("The behavior of the animation when it wraps.")]
		public WrapMode wrapMode;
		
		[HutongGames.PlayMaker.Tooltip("How the animation is blended with other animations on the Game Object.")]
		public AnimationBlendMode blendMode;
		
		[HasFloatSlider(0f, 5f)]
		[HutongGames.PlayMaker.Tooltip("The speed of the animation. 1 = normal; 2 = double speed...")]
		public FsmFloat speed;
		
		[HutongGames.PlayMaker.Tooltip("The animation layer")]
		public FsmInt layer;

		public override void Reset()
		{
			gameObject = null;
			animName = null;
			wrapMode = WrapMode.Loop;
			blendMode = AnimationBlendMode.Blend;
			speed = 1.0f;
			layer = 0;
		}

		public override void OnEnter()
		{
			DoAnimationSettings();
			
			Finish();
		}

		void DoAnimationSettings()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null || string.IsNullOrEmpty(animName.Value))
			{
				return;
			}

			if (go.animation == null)
			{
				LogWarning("Missing animation component: " + go.name);
				return;
			}

			var anim = go.animation[animName.Value];

			if (anim == null)
			{
				LogWarning("Missing animation: " + animName.Value);
				return;
			}

			anim.wrapMode = wrapMode;
			anim.blendMode = blendMode;
			
			if (!layer.IsNone)
			{
				anim.layer = layer.Value;
			}
			
			if (!speed.IsNone)
			{
				anim.speed = speed.Value;
			}
		}
	}
}