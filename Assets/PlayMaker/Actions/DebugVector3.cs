// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Debug)]
	[HutongGames.PlayMaker.Tooltip("Logs the value of a Vector3 Variable in the PlayMaker Log Window.")]
	public class DebugVector3 : FsmStateAction
	{
        [HutongGames.PlayMaker.Tooltip("Info, Warning, or Error.")]
		public LogLevel logLevel;

		[UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip("Prints the value of a Vector3 variable in the PlayMaker log window.")]
		public FsmVector3 vector3Variable;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			vector3Variable = null;
		}

		public override void OnEnter()
		{
			string text = "None";
			
			if (!vector3Variable.IsNone)
			{
				text = vector3Variable.Name + ": " + vector3Variable.Value;
			}

			ActionHelpers.DebugLog(Fsm, logLevel, text);

			Finish();
		}
	}
}