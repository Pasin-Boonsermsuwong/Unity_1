using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OutlineBlinker : MonoBehaviour {

	Outline outline;
	bool cswitch;
	public Color color;
	bool started;

	void Start () {
		outline = GetComponent<Outline>();
		StartCoroutine(StartBlinker());
		started = true;
	}

	IEnumerator StartBlinker(){
	///	Debug.Log("StartBlinker");
		while(true){
			if(cswitch){
				outline.effectColor = Color.white;
			}else{
				outline.effectColor = color;
			}
			cswitch = !cswitch;
			yield return new WaitForSeconds(0.2f);
		}
	}

	void OnEnable(){
	//	Debug.Log("OnEnable");
		if(started)StartCoroutine(StartBlinker());
	}
}
