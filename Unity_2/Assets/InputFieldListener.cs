using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class InputFieldListener : MonoBehaviour {


	InputField mainInputField;
	Text text;
	GameController gc;
	public void Start()
	{
		gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		mainInputField = GetComponent<InputField>();
		text = mainInputField.GetComponentInChildren<Text>();
		//Adds a listener to the main input field and invokes a method when the value changes.
		mainInputField.onValueChange.AddListener (delegate {ValueChangeCheck ();});
	}
	
	// Invoked when the value of the text field changes.
	public void ValueChangeCheck()
	{
		gc.nameInputChanged(text.text);
	}
}
