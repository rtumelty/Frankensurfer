// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[HutongGames.PlayMaker.Tooltip("Applies an explosion Force to all Game Objects with a Rigid Body inside a Radius.")]
	public class Explosion : FsmStateAction
	{
		[RequiredField]
		[HutongGames.PlayMaker.Tooltip("The world position of the center of the explosion.")]
        public FsmVector3 center;

		[RequiredField]
        [HutongGames.PlayMaker.Tooltip("The strength of the explosion.")]
		public FsmFloat force;

		[RequiredField]
        [HutongGames.PlayMaker.Tooltip("The radius of the explosion. Force falls of linearly with distance.")]
		public FsmFloat radius;

        [HutongGames.PlayMaker.Tooltip("Applies the force as if it was applied from beneath the object. This is useful since explosions that throw things up instead of pushing things to the side look cooler. A value of 2 will apply a force as if it is applied from 2 meters below while not changing the actual explosion position.")]
		public FsmFloat upwardsModifier;

        [HutongGames.PlayMaker.Tooltip("The type of force to apply.")]
		public ForceMode forceMode;

		[UIHint(UIHint.Layer)]
		public FsmInt layer;
		
        [UIHint(UIHint.Layer)]
        [HutongGames.PlayMaker.Tooltip("Layers to effect.")]
		public FsmInt[] layerMask;
		
        [HutongGames.PlayMaker.Tooltip("Invert the mask, so you effect all layers except those defined above.")]
		public FsmBool invertMask;
		
        [HutongGames.PlayMaker.Tooltip("Repeat every frame while the state is active.")]
        public bool everyFrame;

		public override void Reset()
		{
			center = null;
			upwardsModifier = 0f;
			forceMode = ForceMode.Force;
			everyFrame = false;
		}

        public override void Awake()
        {
            Fsm.HandleFixedUpdate = true;
        }

		public override void OnEnter()
		{
			DoExplosion();
			
			if (!everyFrame)
			{
			    Finish();
			}		
		}

		public override void OnFixedUpdate()
		{
			DoExplosion();
		}

		void DoExplosion()
		{
			var colliders = Physics.OverlapSphere(center.Value, radius.Value);
			
			foreach (var hit in colliders)
			{
				if (hit.rigidbody != null && ShouldApplyForce(hit.gameObject))
				{
					hit.rigidbody.AddExplosionForce(force.Value, center.Value, radius.Value, upwardsModifier.Value, forceMode);
				}
			}
		}
		
		bool ShouldApplyForce(GameObject go)
		{
			var mask = ActionHelpers.LayerArrayToLayerMask(layerMask, invertMask.Value);
			
			return ((1 << go.layer) & mask) > 0;
		}
	}
}