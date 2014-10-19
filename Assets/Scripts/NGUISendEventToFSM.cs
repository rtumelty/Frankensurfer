using HutongGames.PlayMaker;
using UnityEngine;
using System;
using System.Collections;

public class NGUISendEventToFSM : MonoBehaviour
{
	//public PlayMakerFSM targetFSM;
	public string onClickEvent; 
	
	void OnClick()
	{//if (null == targetFSM) throw new Exception("null == targetFSM");
		if (string.IsNullOrEmpty(onClickEvent)) throw new Exception("string.IsNullOrEmpty(onClickEvent)");
		
		//targetFSM.Fsm.Event(onClickEvent);
		PlayMakerFSM.BroadcastEvent(onClickEvent);
	}
	
	void PlayNamedEvent(string eventName = null)
	{
		if (string.IsNullOrEmpty(eventName)) eventName = onClickEvent;
		
		//if (null == targetFSM) throw new Exception("null == targetFSM");
		if (string.IsNullOrEmpty(eventName)) throw new Exception("string.IsNullOrEmpty(onClickEvent)");
		
		//targetFSM.Fsm.Event(onClickEvent);
		PlayMakerFSM.BroadcastEvent(eventName);
	}
}