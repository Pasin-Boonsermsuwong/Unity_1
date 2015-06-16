using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Dropable : MonoBehaviour , IDropHandler, IPointerEnterHandler, IPointerExitHandler{
	public Draggable.Slot typeOfDrop;
	public void OnPointerEnter(PointerEventData e){
		Debug.Log("Pointer entered");
	}
	public void OnPointerExit(PointerEventData e){
		Debug.Log("Pointer exited");
	}
	public void OnDrop(PointerEventData e){
		Draggable d = e.pointerDrag.GetComponent<Draggable>();
		if(d!=null){
			if(d.type == typeOfDrop || typeOfDrop == Draggable.Slot.INV){
				d.parentToReturnTo = this.transform;
			}
		
		}
	}
}
