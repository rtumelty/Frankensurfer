// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Transform)]
	[HutongGames.PlayMaker.Tooltip("Smoothly Rotates a Game Object so its forward vector points in the specified Direction.")]
	public class SmoothLookAtDirection : FsmStateAction
	{
		[RequiredField]
		[HutongGames.PlayMaker.Tooltip("The GameObject to rotate.")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[HutongGames.PlayMaker.Tooltip("The direction to smoothly rotate towards.")]
		public FsmVector3 targetDirection;

		[HutongGames.PlayMaker.Tooltip("Only rotate if Target Direction Vector length is greater than this threshold.")]
		public FsmFloat minMagnitude;
		
		[HutongGames.PlayMaker.Tooltip("Keep this vector pointing up as the GameObject rotates.")]
		public FsmVector3 upVector;
		
		[RequiredField]
		[HutongGames.PlayMaker.Tooltip("Eliminate any tilt up/down as the GameObject rotates.")]
		public FsmBool keepVertical;
		
		[RequiredField]
		[HasFloatSlider(0.5f,15)]
		[HutongGames.PlayMaker.Tooltip("How quickly to rotate.")]
		public FsmFloat speed;

		[HutongGames.PlayMaker.Tooltip("Perform in LateUpdate. This can help eliminate jitters in some situations.")]
		public bool lateUpdate;
		
		GameObject previousGo; // track game object so we can re-initialize when it changes.
		Quaternion lastRotation;
		Quaternion desiredRotation;
		
		public override void Reset()
		{
			gameObject = null;
			targetDirection = new FsmVector3 { UseVariable = true};
			minMagnitude = 0.1f;
			upVector = new FsmVector3 { UseVariable = true};
			keepVertical = true;
			speed = 5;
			lateUpdate = true;
		}

		public override void OnEnter()
		{
			previousGo = null;
		}

		public override void OnUpdate()
		{
			if (!lateUpdate)
			{
				DoSmoothLookAtDirection();
			}
		}

		public override void OnLateUpdate()
		{
			if (lateUpdate)
			{
				DoSmoothLookAtDirection();
			}
		}

		void DoSmoothLookAtDirection()
		{
			if (targetDirection.IsNone)
			{
				return;
			}
			
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}

			// re-initialize if game object has changed
			
			if (previousGo != go)
			{
				lastRotation = go.transform.rotation;
				desiredRotation = lastRotation;
				previousGo = go;
			}
			
			// desired direction

			var diff = targetDirection.Value;
			
			if (keepVertical.Value)
			{
				diff.y = 0;
			}
			
			// smooth look at

			if (diff.sqrMagnitude > minMagnitude.Value)
			{
				desiredRotation = Quaternion.LookRotation(diff, upVector.IsNone ? Vector3.up : upVector.Value);			
			}
			
			lastRotation = Quaternion.Slerp(lastRotation, desiredRotation, speed.Value * Time.deltaTime);	
			go.transform.rotation = lastRotation;			
		}
	}
}