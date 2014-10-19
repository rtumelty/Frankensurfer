// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Character)]
	[HutongGames.PlayMaker.Tooltip("Modify various character controller settings.\n'None' leaves the setting unchanged.")]
	public class ControllerSettings : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(CharacterController))]
        [HutongGames.PlayMaker.Tooltip("The GameObject that owns the CharacterController.")]
		public FsmOwnerDefault gameObject;

		[HutongGames.PlayMaker.Tooltip("The height of the character's capsule.")]
		public FsmFloat height;

		[HutongGames.PlayMaker.Tooltip("The radius of the character's capsule.")]
		public FsmFloat radius;

		[HutongGames.PlayMaker.Tooltip("The character controllers slope limit in degrees.")]
		public FsmFloat slopeLimit;

		[HutongGames.PlayMaker.Tooltip("The character controllers step offset in meters.")]
		public FsmFloat stepOffset;

		[HutongGames.PlayMaker.Tooltip("The center of the character's capsule relative to the transform's position")]
		public FsmVector3 center;

		[HutongGames.PlayMaker.Tooltip("Should other rigidbodies or character controllers collide with this character controller (By default always enabled).")]
		public FsmBool detectCollisions;

        [HutongGames.PlayMaker.Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		// cache so we can get new controller only when it changes.
		
		GameObject previousGo; 
		CharacterController controller;

		public override void Reset()
		{
			gameObject = null;
			height = new FsmFloat { UseVariable = true };
			radius = new FsmFloat { UseVariable = true };
			slopeLimit = new FsmFloat { UseVariable = true };
			stepOffset = new FsmFloat { UseVariable = true };
			center = new FsmVector3 { UseVariable = true };
			detectCollisions = new FsmBool { UseVariable = true };
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoControllerSettings();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoControllerSettings();
		}


		void DoControllerSettings()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}

			if (go != previousGo)
			{
				controller = go.GetComponent<CharacterController>();
				previousGo = go;
			}

			if (controller != null)
			{
				if (!height.IsNone) controller.height = height.Value;
				if (!radius.IsNone) controller.radius = radius.Value;
				if (!slopeLimit.IsNone) controller.slopeLimit = slopeLimit.Value;
				if (!stepOffset.IsNone) controller.stepOffset = stepOffset.Value;
				if (!center.IsNone) controller.center = center.Value;
				if (!detectCollisions.IsNone) controller.detectCollisions = detectCollisions.Value;
			}
		}
	}
}
