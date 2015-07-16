using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class PointerListener : MonoBehaviour, IPointerDownHandler, IDeselectHandler {
	GameController gc;

	public void OnPointerDown(PointerEventData p){	
		if(gc==null)FindGameController();
		Debug.Log("ChatInput selected via mouse");
		if(!gc.chatState)gc.ChangeChatState();
	}
	public void OnDeselect(BaseEventData b){
		if(gc==null)FindGameController();
		Debug.Log("ChatInput deselected");
		if(gc.chatState)gc.ChangeChatState();
	}
	void FindGameController(){
		gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
	}
}