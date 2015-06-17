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
			if(typeOfDrop == Draggable.Slot.INV){
				//Drag on zone with > 1 capacity
				if(transform.childCount<capacity){
					d.parentToReturnTo = this.transform;
				}
			}
			else if(d.type == typeOfDrop &&//Drop on to empty mount slot
			   capacity == 1&&
			   transform.childCount == 0){
				d.parentToReturnTo = this.transform;
//				Debug.Log ("Child after drop: "+transform.childCount);

			}
			else if(d.type == typeOfDrop &&//Drop on to occupied mount slot and switch
			        capacity == 1&&
			        transform.childCount == 1){	
				Draggable switchExisting = GetComponentInChildren<Draggable>();
				switchExisting.transform.SetParent(d.parentToReturnTo);
				d.parentToReturnTo = this.transform;

			}
		}
	}
}
