using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class NetworkManager : MonoBehaviour {

	GameController gc;

	public bool offlineMode = false;

	public Text logText;
	void Start () {
		gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		Connect();
	}
	
	void Connect(){
		if(offlineMode){
			PhotonNetwork.offlineMode = true;
			OnJoinedLobby();
		}else{
			PhotonNetwork.ConnectUsingSettings("v001");
		}


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
		ins.GetComponent<FirstPersonController>().enabled = true;
		//ins.GetComponent<CharacterController>().enabled = true;
		gc.LockCursor();
	}
}
