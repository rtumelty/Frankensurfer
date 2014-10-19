// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Transform)]
	[HutongGames.PlayMaker.Tooltip("Translates a Game Object. Use a Vector3 variable and/or XYZ components. To leave any axis unchanged, set variable to 'None'.")]
	public class Translate : FsmStateAction
	{
		[RequiredField]
		[HutongGames.PlayMaker.Tooltip("The game object to translate.")]
		public FsmOwnerDefault gameObject;
		
		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Tooltip("A translation vector. NOTE: You can override individual axis below.")]
		public FsmVector3 vector;
		
		[HutongGames.PlayMaker.Tooltip("Translation along x axis.")]
		public FsmFloat x;

		[HutongGames.PlayMaker.Tooltip("Translation along y axis.")]
		public FsmFloat y;

		[HutongGames.PlayMaker.Tooltip("Translation along z axis.")]
		public FsmFloat z;

		[HutongGames.PlayMaker.Tooltip("Translate in local or world space.")]
		public Space space;
		
		[HutongGames.PlayMaker.Tooltip("Translate over one second")]
		public bool perSecond;

		[HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
		public bool everyFrame;

		[HutongGames.PlayMaker.Tooltip("Perform the translate in LateUpdate. This is useful if you want to override the position of objects that are animated or otherwise positioned in Update.")]
		public bool lateUpdate;

        [HutongGames.PlayMaker.Tooltip("Perform the translate in FixedUpdate. This is useful when working with rigid bodies and physics.")]
        public bool fixedUpdate;	

		public override void Reset()
		{
			gameObject = null;
			vector = null;
			// default axis to variable dropdown with None selected.
			x = new FsmFloat { UseVariable = true };
			y = new FsmFloat { UseVariable = true };
			z = new FsmFloat { UseVariable = true };
			space = Space.Self;
			perSecond = true;
			everyFrame = true;
			lateUpdate = false;
		    fixedUpdate = false;
		}

        public override void Awake()
        {
            Fsm.HandleFixedUpdate = true;
        }

		public override void OnEnter()
		{
			if (!everyFrame && !lateUpdate && !fixedUpdate)
			{
				DoTranslate();
				Finish();
			}
		}

		public override void OnUpdate()
		{
			if (!lateUpdate && !fixedUpdate)
			{
				DoTranslate();
			}
		}

		public override void OnLateUpdate()
		{
			if (lateUpdate)
			{
				DoTranslate();
			}

			if (!everyFrame)
			{
				Finish();
			}
		}

        public override void OnFixedUpdate()
        {
            if (fixedUpdate)
            {
                DoTranslate();
            }

            if (!everyFrame)
            {
                Finish();
            }
        }

		void DoTranslate()
		{
			// init

			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}

			// Use vector if specified

			var translate = vector.IsNone ? new Vector3(x.Value, y.Value, z.Value) : vector.Value;

			// override any axis

			if (!x.IsNone) translate.x = x.Value;
			if (!y.IsNone) translate.y = y.Value;
			if (!z.IsNone) translate.z = z.Value;
		
			// apply
			
			if (!perSecond)
			{
				go.transform.Translate(translate, space);
			}
			else
			{
				go.transform.Translate(translate * Time.deltaTime, space);
			}
		}

	}
}