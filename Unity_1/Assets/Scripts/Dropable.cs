using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Dropable : MonoBehaviour , IDropHandler, IPointerEnterHandler, IPointerExitHandler{
	public Draggable.Slot typeOfDrop;
	public int capacity;
	public void OnPointerEnter(PointerEventData e){
//		Debug.Log("Pointer entered");
	}
	public void OnPointerExit(PointerEventData e){
//		Debug.Log("Pointer exited");
	}
	public void OnDrop(PointerEventData e){
		Draggable d = e.pointerDrag.GetComponent<Draggable>();
		if(d!=null){
			if(d.type == typeOfDrop || typeOfDrop == Draggable.Slot.INV){
				//Drag on zone with > 1 capacityy
				if(transform.childCount<capacity){
					d.parentToReturnTo = this.transform;
				}
				//Switch for one-slot
				else if(capacity == 1&&transform.childCount == 1){
					Draggable switchExisting = GetComponentInChildren<Draggable>();
					switchExisting.SetParent(d.parentToReturnTo);
					d.parentToReturnTo = this.transform;
				}

			}
		
		}
	}
}
