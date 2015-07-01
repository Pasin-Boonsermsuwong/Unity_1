using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class NetworkManager : MonoBehaviour {

	GameController gc;

	public Text logText;
	void Start () {
		gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		Connect();
	}
	
	void Connect(){
		PhotonNetwork.ConnectUsingSettings("v001");

	}

	void Update(){
		string s = PhotonNetwork.connectionStateDetailed.ToString();
		logText.text = s;
//		Debug.Log(s);
	}

	void OnJoinedLobby(){
		PhotonNetwork.JoinRoom("Main");
	}

	void OnPhotonJoinRoomFailed(){
		PhotonNetwork.CreateRoom("Main");
	}

	void OnJoinedRoom(){
		SpawnMyPlayer();

	}

	void SpawnMyPlayer(){
		GameObject ins = PhotonNetwork.Instantiate("CharFighter",new Vector3(Random.Range(-50,50),50,Random.Range(-50,50)),Quaternion.identity,0) as GameObject;
		ins.transform.FindChild("FirstPersonCharacter").gameObject.SetActive(true);
		ins.GetComponent<Gun>().enabled = true;
		ins.GetComponent<RigidbodyFirstPersonController>().enabled = true;
		gc.LockCursor();
	}
}
