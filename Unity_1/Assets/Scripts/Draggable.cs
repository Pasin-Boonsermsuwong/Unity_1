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
	public GameObject infobox_panel;
//	public Transform defaultItemParent;

	public void Start(){
		if(infoText=="")infoText = infoText_default;
		else infobox_text.text = infoText;
		infobox_panel.SetActive(false);
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
		transform.SetParent(parentToReturnTo);
	//	if(GetComponentInParent<Dropable>().capacity==1){	//if the new parent is a mount, update mount on player
			transform.GetComponent<mountPlayerCaller>().UpdateMount();
	//	}
	
		GetComponent<CanvasGroup>().blocksRaycasts = true;
	}
	//Show infobox
	public void OnPointerEnter(PointerEventData e){
		infobox_panel.SetActive(true);
	//	defaultItemParent = this.transform;
	//	infobox_panel.transform.SetParent(this.transform.parent);
//		infobox_text.text = infoText;
	}
	public void OnPointerExit(PointerEventData e){
		infobox_panel.SetActive(false);
	//	infobox_panel.transform.SetParent(defaultItemParent);
	//	infobox_text.text = "";
	}
	public void SetParent(Transform parent){
		transform.SetParent(parent);
	}
}
