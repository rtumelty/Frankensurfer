// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Debug)]
	[HutongGames.PlayMaker.Tooltip("Logs the value of an Integer Variable in the PlayMaker Log Window.")]
	public class DebugInt : FsmStateAction
	{
        [HutongGames.PlayMaker.Tooltip("Info, Warning, or Error.")]
        public LogLevel logLevel;

		[UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip("Prints the value of an Int variable in the PlayMaker log window.")]
		public FsmInt intVariable;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			intVariable = null;
		}

		public override void OnEnter()
		{
			string text = "None";
			
			if (!intVariable.IsNone)
			{
				text = intVariable.Name + ": " + intVariable.Value;
			}

			ActionHelpers.DebugLog(Fsm, logLevel, text);

			Finish();
		}
	}
}