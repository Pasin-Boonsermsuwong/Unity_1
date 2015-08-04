using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HealthSimple : NetworkBehaviour {

	public int maxHP;
	[SyncVar]public int armor;
	
	[SyncVar (hook = "OnHealthChanged")]int curHP;

	public Slider slider;
	public GameObject deathExplosion;

	void Start () {
		UpdateSlider();
		curHP = maxHP;
	}
	
	public void TakeDamage(int amount,string sourceName,string sourceWeapon){
		TakeDamage(amount, sourceName, sourceWeapon,"");
	}
	public void TakeDamage(int amount,string sourceName,string sourceWeapon,string specialTag){
		TakeDamage(amount, sourceName, sourceWeapon,specialTag,0);
	}
	public void TakeDamage(int amount,string sourceName,string sourceWeapon,string specialTag,float specialTagParam){
		if(!isServer)return;
		if(amount > 0){
			//ARMOR MAXES AT 90% DAMAGE REDUCTION
			amount = Mathf.Max(amount - armor,(int)Mathf.Round(amount * 0.1f));
		}
		Debug.Log("TakeDamage: "+amount);
		curHP = Mathf.Min (curHP - amount,maxHP);
		if(curHP<=0){
			StartCoroutine("RpcDeathExplosion");
			NetworkServer.Destroy(gameObject);
		}
	}

	void OnHealthChanged(int h){
		curHP = h;
		UpdateSlider();
	}

	void UpdateSlider(){
		if(!slider.IsActive())return;
		if(isLocalPlayer){
			Debug.LogError("Shouldn't be here");
		}else{
			slider.value = curHP/(float)maxHP;
		}
	}

	[ClientRpc]
	void RpcDeathExplosion(){
		Instantiate(deathExplosion, transform.position, transform.rotation);
	}
	
}
