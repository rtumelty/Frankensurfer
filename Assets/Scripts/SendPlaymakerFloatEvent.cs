using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class SendPlaymakerFloatEvent : MonoBehaviour {
	public enum TriggerType {
		Enable,
		Disable,
		OnTrigger,
		OnTrigger2D,
		OnCollision,
		OnCollision2D,
		OnDestroy
	}

	[SerializeField] TriggerType trigger;
	[SerializeField] string eventName;
	[SerializeField] float value;

	void SendEvent () {
		Fsm.EventData.FloatData = value;
		PlayMakerFSM.BroadcastEvent(eventName);
		gameObject.SetActive(false);
	}
	
	void OnEnable() {
		if (trigger == TriggerType.Enable) SendEvent();
	}
	
	void OnDisable() {
		if (trigger == TriggerType.Disable) SendEvent();
	}
	
	void OnDestroy() {
		if (trigger == TriggerType.OnDestroy) SendEvent();
	}
	
	void OnTriggerEnter(Collider c) {
		if (trigger == TriggerType.OnTrigger) SendEvent();
	}
	
	void OnCollisionEnter(Collision c) {
		if (trigger == TriggerType.OnCollision) SendEvent();
	}
	
	void OnTriggerEnter2D(Collider2D c) {
		if (trigger == TriggerType.OnTrigger2D) SendEvent();
	}
	
	void OnCollisionEnter2D(Collision2D c) {
		if (trigger == TriggerType.OnCollision2D) SendEvent();
	}
}
