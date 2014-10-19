// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Debug)]
	[HutongGames.PlayMaker.Tooltip("Logs the value of a Game Object Variable in the PlayMaker Log Window.")]
	public class DebugGameObject : FsmStateAction
	{
        [HutongGames.PlayMaker.Tooltip("Info, Warning, or Error.")]
        public LogLevel logLevel;

		[UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip("Prints the value of a GameObject variable in the PlayMaker log window.")]
		public FsmGameObject gameObject;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			gameObject = null;
		}

		public override void OnEnter()
		{
			string text = "None";
			
			if (!gameObject.IsNone)
			{
				text = gameObject.Name + ": " + gameObject;
			}
			
			ActionHelpers.DebugLog(Fsm, logLevel, text);
			
			Finish();
		}
	}
}