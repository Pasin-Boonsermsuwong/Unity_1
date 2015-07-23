using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerData : MonoBehaviour {

	public string playerName;
	public string playerClass;
	Text classText;

	void Start(){
	//	selectedClass = enumClass.Fighter;
		playerClass = "Fighter";
		classText = GameObject.Find("Canvas").transform.FindChild("TextEnterClass").GetComponent<Text>();
		if(classText==null)Debug.LogError("classText is null");
	}
	public void SetName(string s){
		playerName = s;
	}

	public void SetClass(string s){
	//	Debug.Log("SetClass");
		playerClass = s;
	//	selectedClass = s;
		classText.text = s;
	}

}
