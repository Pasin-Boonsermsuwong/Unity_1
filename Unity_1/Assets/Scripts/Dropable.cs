using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Dropable : MonoBehaviour , IDropHandler{
	public Draggable.Slot typeOfDrop;
	public void OnDrop(PointerEventData e){
		Draggable d = e.pointerDrag.GetComponent<Draggable>();
		if(d!=null){
			if(d.type == typeOfDrop || typeOfDrop == Draggable.Slot.INV){
				d.parentToReturnTo = this.transform;
			}
		
		}
	}
}
