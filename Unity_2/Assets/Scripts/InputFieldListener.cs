﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class InputFieldListener : MonoBehaviour{
	InputField mainInputField;
	Text text;
	PlayerData Pd;
	GameController gc;
	public void Start()
	{
		Pd = GameObject.Find("PlayerData").GetComponent<PlayerData>();
		mainInputField = GetComponent<InputField>();
		text = mainInputField.transform.Find("Text").GetComponent<Text>();
		//Adds a listener to the main input field and invokes a method when the value changes.
	//	mainInputField.onValueChange.AddListener (delegate {ValueChangeCheck ();});
		mainInputField.onEndEdit.AddListener (delegate {ValueChangeCheck ();});

	}
	
	// Invoked when the value of the text field changes.

	public void ValueChangeCheck()
	{
//Debug.Log("ValueChangeCheck: "+text.text);
		Pd.playerName = text.text;
	}
}
