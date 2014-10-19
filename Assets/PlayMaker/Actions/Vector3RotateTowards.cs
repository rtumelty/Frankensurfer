// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Vector3)]
    [HutongGames.PlayMaker.Tooltip("Rotates a Vector3 direction from Current towards Target.")]
    public class Vector3RotateTowards : FsmStateAction
    {
        [RequiredField]
        public FsmVector3 currentDirection;
        [RequiredField]
        public FsmVector3 targetDirection;
        [RequiredField]
        [HutongGames.PlayMaker.Tooltip("Rotation speed in degrees per second")]
        public FsmFloat rotateSpeed;
        [RequiredField]
        [HutongGames.PlayMaker.Tooltip("Max Magnitude per second")]
        public FsmFloat maxMagnitude;
        public override void Reset()
        {
            currentDirection = new FsmVector3 { UseVariable = true };
            targetDirection = new FsmVector3 { UseVariable = true };
            rotateSpeed = 360;
            maxMagnitude = 1;
        }

        public override void OnUpdate()
        {
            currentDirection.Value = Vector3.RotateTowards(currentDirection.Value, targetDirection.Value, rotateSpeed.Value * Mathf.Deg2Rad * Time.deltaTime, maxMagnitude.Value);
        }
    }
}

