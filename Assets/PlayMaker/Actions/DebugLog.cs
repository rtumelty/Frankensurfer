// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Debug)]
	[HutongGames.PlayMaker.Tooltip("Sends a log message to the PlayMaker Log Window.")]
	public class DebugLog : FsmStateAction
	{
        [HutongGames.PlayMaker.Tooltip("Info, Warning, or Error.")]
		public LogLevel logLevel;

        [HutongGames.PlayMaker.Tooltip("Text to print to the PlayMaker log window.")]
		public FsmString text;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			text = "";
		}

		public override void OnEnter()
		{
			if (!string.IsNullOrEmpty(text.Value))
				ActionHelpers.DebugLog(Fsm, logLevel, text.Value);

			Finish();
		}
	}
}