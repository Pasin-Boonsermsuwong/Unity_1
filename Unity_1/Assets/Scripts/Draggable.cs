using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler,IEndDragHandler{

	public Transform parentToReturnTo = null;
	public enum Slot {WEAPON1, WEAPON2, ENGINE, MODULE, WM, INV};
	public Slot type;
	public void OnBeginDrag(PointerEventData e){
		parentToReturnTo = transform.parent;
		transform.parent = transform.parent.parent;
		GetComponent<CanvasGroup>().blocksRaycasts = false;
	}
	public void OnDrag(PointerEventData e){
		this.transform.position = e.position;
	}
	public void OnEndDrag(PointerEventData e){
		transform.parent = parentToReturnTo;
		GetComponent<CanvasGroup>().blocksRaycasts = true;
	}
}
