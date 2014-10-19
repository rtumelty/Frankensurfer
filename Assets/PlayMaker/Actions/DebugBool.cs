// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Debug)]
	[HutongGames.PlayMaker.Tooltip("Logs the value of a Bool Variable in the PlayMaker Log Window.")]
	public class DebugBool : FsmStateAction
	{
        [HutongGames.PlayMaker.Tooltip("Info, Warning, or Error.")]
        public LogLevel logLevel;
		
        [UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip("Prints the value of a Bool variable in the PlayMaker log window.")]
		public FsmBool boolVariable;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			boolVariable = null;
		}

		public override void OnEnter()
		{
			var text = "None";

			if (!boolVariable.IsNone)
			{
				text = boolVariable.Name + ": " + boolVariable.Value;
			}
			
			ActionHelpers.DebugLog(Fsm, logLevel, text);
						
			Finish();
		}
	}
}