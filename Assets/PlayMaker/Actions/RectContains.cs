// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Rect)]
	[HutongGames.PlayMaker.Tooltip("Tests if a point is inside a rectangle.")]
	public class RectContains : FsmStateAction
	{
		[RequiredField]
		[HutongGames.PlayMaker.Tooltip("Rectangle to test.")]
		public FsmRect rectangle;

		[HutongGames.PlayMaker.Tooltip("Point to test.")]
		public FsmVector3 point;

		[HutongGames.PlayMaker.Tooltip("Specify/override X value.")]
		public FsmFloat x;

		[HutongGames.PlayMaker.Tooltip("Specify/override Y value.")]
		public FsmFloat y;

		//[ActionSection("")]

		[HutongGames.PlayMaker.Tooltip("Event to send if the Point is inside the Rectangle.")]
		public FsmEvent trueEvent;

		[HutongGames.PlayMaker.Tooltip("Event to send if the Point is outside the Rectangle.")]
		public FsmEvent falseEvent;

		[UIHint(UIHint.Variable)]
		[HutongGames.PlayMaker.Tooltip("Store the result in a variable.")]
		public FsmBool storeResult;

		//[ActionSection("")]

		[HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			rectangle = new FsmRect { UseVariable = true };
			point = new FsmVector3 { UseVariable = true };
			x = new FsmFloat { UseVariable = true };
			y = new FsmFloat { UseVariable = true };
			storeResult = null;
			trueEvent = null;
			falseEvent = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoRectContains();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoRectContains();
		}

		void DoRectContains()
		{
			if (rectangle.IsNone)
			{
				return;
			}

			// get point to test from inputs

			var testPoint = point.Value;

			if (!x.IsNone)
			{
				testPoint.x = x.Value;
			}

			if (!y.IsNone)
			{
				testPoint.y = y.Value;
			}

			// do results

			var contained = rectangle.Value.Contains(testPoint);

			storeResult.Value = contained;

			Fsm.Event(contained ? trueEvent : falseEvent);
		}
	}
}