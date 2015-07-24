using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerData : MonoBehaviour {

	public string playerName;
	public string playerClass;
	Text classText;
	NetworkPlayerSelect netManager;

	void Start(){
	//	selectedClass = enumClass.Fighter;
		netManager = GameObject.Find("NetManager").GetComponent<NetworkPlayerSelect>();
		playerClass = "Fighter";
		classText = GameObject.Find("Canvas").transform.FindChild("TextEnterClass").GetComponent<Text>();
		if(classText==null)Debug.LogError("classText is null");
	}
	public void SetName(string s){
		playerName = s;
	}

	public void SetClass(string s){
		playerClass = s;
		classText.text = s;
	}

}
