using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler,IEndDragHandler, IPointerEnterHandler, IPointerExitHandler{

	public Transform parentToReturnTo = null;
	public enum Slot {WEAPON1, WEAPON2, ENGINE, MODULE, WM, INV};
	public Slot type;
	public string infoText;
	string infoText_default = "<size=25>INFOCARD TITLE</size>" +
		"\n *1111111111" +
			"\n *2222222222";
	public Text infobox_text;

	public void Start(){
		if(infoText=="")infoText = infoText_default;

	}

	public void OnBeginDrag(PointerEventData e){
		parentToReturnTo = transform.parent;
		transform.SetParent(transform.parent.parent);
		GetComponent<CanvasGroup>().blocksRaycasts = false;
	}
	public void OnDrag(PointerEventData e){
		this.transform.position = e.position;
	}
	public void OnEndDrag(PointerEventData e){
//		Debug.Log(e.pointerCurrentRaycast);
		transform.SetParent(parentToReturnTo);
		GetComponent<CanvasGroup>().blocksRaycasts = true;
	}
	public void OnPointerEnter(PointerEventData e){
	//	Debug.Log("PointerEnter");
		infobox_text.text = infoText;
	}
	public void OnPointerExit(PointerEventData e){
	//	Debug.Log("PointerExit");
		infobox_text.text = "";
	}
}
