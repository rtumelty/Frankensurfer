// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Transform)]
	[HutongGames.PlayMaker.Tooltip("Rotates a Game Object so its forward vector points at a Target. The Target can be specified as a GameObject or a world Position. If you specify both, then Position specifies a local offset from the target object's Position.")]
	public class LookAt : FsmStateAction
	{
		[RequiredField]
		[HutongGames.PlayMaker.Tooltip("The GameObject to rotate.")]
		public FsmOwnerDefault gameObject;

		[HutongGames.PlayMaker.Tooltip("The GameObject to Look At.")]
		public FsmGameObject targetObject;
		
		[HutongGames.PlayMaker.Tooltip("World position to look at, or local offset from Target Object if specified.")]
		public FsmVector3 targetPosition;

		[HutongGames.PlayMaker.Tooltip("Rotate the GameObject to point its up direction vector in the direction hinted at by the Up Vector. See Unity Look At docs for more details.")]
		public FsmVector3 upVector;
		
		[HutongGames.PlayMaker.Tooltip("Don't rotate vertically.")]
		public FsmBool keepVertical;
		
		[Title("Draw Debug Line")]
		[HutongGames.PlayMaker.Tooltip("Draw a debug line from the GameObject to the Target.")]
		public FsmBool debug;

		[HutongGames.PlayMaker.Tooltip("Color to use for the debug line.")] 
		public FsmColor debugLineColor;

		[HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
		public bool everyFrame = true;

	    private GameObject go;
	    private GameObject goTarget;
	    private Vector3 lookAtPos;
	    private Vector3 lookAtPosWithVertical;
        
		public override void Reset()
		{
			gameObject = null;
			targetObject = null;
			targetPosition = new FsmVector3 { UseVariable = true};
			upVector = new FsmVector3 { UseVariable = true};
			keepVertical = true;
			debug = false;
			debugLineColor = Color.yellow;
			everyFrame = true;
		}

		public override void OnEnter()
		{
			DoLookAt();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnLateUpdate()
		{
			DoLookAt();
		}

		void DoLookAt()
		{
			if (!UpdateLookAtPosition())
			{
			    return;
			}
			
			go.transform.LookAt(lookAtPos, upVector.IsNone ? Vector3.up : upVector.Value);			
			
			if (debug.Value)
			{
				Debug.DrawLine(go.transform.position, lookAtPos, debugLineColor.Value);
			}
		}

        public bool UpdateLookAtPosition()
        {
            go = Fsm.GetOwnerDefaultTarget(gameObject);
            if (go == null)
            {
                return false;
            }

            goTarget = targetObject.Value;
            if (goTarget == null && targetPosition.IsNone)
            {
                return false;
            }

            if (goTarget != null)
            {
                lookAtPos = !targetPosition.IsNone ? goTarget.transform.TransformPoint(targetPosition.Value) : goTarget.transform.position;
            }
            else
            {
                lookAtPos = targetPosition.Value;
            }

            lookAtPosWithVertical = lookAtPos;

            if (keepVertical.Value)
            {
                lookAtPos.y = go.transform.position.y;
            }

            return true;
        }

        public Vector3 GetLookAtPosition()
        {
            return lookAtPos;
        }

        public Vector3 GetLookAtPositionWithVertical()
        {
            return lookAtPosWithVertical;
        }
	}
}