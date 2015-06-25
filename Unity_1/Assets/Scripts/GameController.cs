﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
	Transform myTransform;
	public GameObject player;
	public GameObject powerup;

	//In-game HUD
	public Text scoreText;
	public Text restartText;
	public Text gameOverText;
	public Text sectorText;
	public GameObject inventoryPanel;

	//Pause stuff
	public GameObject pauseObject;
	public static bool pause;

	//Main Canvas
	bool gameOver;
	bool restart;
	int score;

	//Sector
	float boundaryX;
	float boundaryZ;
	
	//for spawn position (outside actual boundary)
	float boundarySpawnX;
	float boundarySpawnZ;

	//for spawned to move toward to
	float boundaryInnerX;
	float boundaryInnerZ;

	int sectorX;
	int sectorY;
	Renderer background ;
	float difficultyMin;
	float difficultyMax;
	int zone;

	GameObject[] sectorEnemy;
	float[] sectorEnemyChance;



	void Start ()
	{
		myTransform = GetComponent<Transform>();
		GameObject bgGameObject = GameObject.FindWithTag("Background");
		background = bgGameObject.GetComponent<Renderer>();
		boundaryX = bgGameObject.transform.localScale.x/2;
		boundaryZ = bgGameObject.transform.localScale.y/2;
		boundaryInnerX = boundaryX * 0.7f;	
		boundaryInnerZ = boundaryZ * 0.7f;
		boundarySpawnX = boundaryX + 50;	
		boundarySpawnZ = boundaryZ + 50;
		player = GameObject.FindWithTag("Player");
		sectorX = 0;
		sectorY = 0;
		SwitchSector(0,0);
		pauseObject.SetActive(false);
		gameOver = false;
		restart = false;
		restartText.text = "";
		gameOverText.text = "";
		score = 0;
		UpdateScore ();
	}
	void FixedUpdate(){

		//SPAWN ENEMIES

		if(!gameOver){
			for(int i = 0;i<sectorEnemy.Length;i++){
				if(!Data.chance(sectorEnemyChance[i]))continue;
				Vector3 spawnPosition = randomSpawnPosition();
				Instantiate (sectorEnemy[i], spawnPosition, Quaternion.LookRotation(
					(new Vector3 (Random.Range (-boundaryInnerX, boundaryInnerX), 0,Random.Range (-boundaryInnerZ, boundaryInnerZ)) - spawnPosition).normalized
					));
			}
		}
	}
	void Update ()
	{
		if (restart)
		{
			if (Input.GetKeyDown (KeyCode.R))
			{
				Application.LoadLevel (Application.loadedLevel);
			}
		}

		//PAUSE
		if(Input.GetButtonDown("Pause")){
			pause = !pause;
			if(pause){
				Time.timeScale = 0;
				pauseObject.SetActive(true);
			}
			if(!pause){
				Time.timeScale = 1;
				pauseObject.SetActive(false);
			}

		}

		if (gameOver&&!restart)
		{
			restartText.text = "Press 'R' for Restart";
			restart = true;
		}
	}
	
	Vector3 randomSpawnPosition(){
		int ran = Random.Range(0,3);
		Vector3 spawnPosition = new Vector3(0,0,0);
		switch (ran)
		{
		case 3:
			spawnPosition = new Vector3 (Random.Range (-boundarySpawnX, boundarySpawnX), 0, boundarySpawnZ);
			break;
		case 2:
			spawnPosition = new Vector3 (Random.Range (-boundarySpawnX, boundarySpawnX), 0, -boundarySpawnZ);
			break;
		case 1:
			spawnPosition = new Vector3 (boundarySpawnX, 0, Random.Range (-boundarySpawnZ, boundarySpawnZ));
			break;
		case 0:
			spawnPosition = new Vector3 (-boundarySpawnX, 0, Random.Range (-boundarySpawnZ, boundarySpawnZ));
			break;
		}
		return spawnPosition;
	}

	public void AddScore (int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore ();
	}
	
	void UpdateScore ()
	{
		scoreText.text = "Score: " + score;
	}
	
	public void GameOver ()
	{
		GetComponent<AudioSource>().Stop();
		gameOverText.text = "Game Over!";
		gameOver = true;
	}

	public void insertItem(int ID){
		inventoryPanel.GetComponent<InventoryController>().InsertItem(ID);
	}

	public void SwitchSector (int dx,int dy){

		//Destroy all existing enemy / loot
		GameObject[] g = GameObject.FindGameObjectsWithTag("Enemy");
		for(int i = 0;i<g.Length;i++){
			Destroy(g[i]);
		}
		GameObject[] g2 = GameObject.FindGameObjectsWithTag("Loot");
		for(int i = 0;i<g2.Length;i++){
			Destroy(g2[i]);
		}

		//Change sector number
		sectorX += dx;
		sectorY += dy;


		//Calculate difficulty based on distance
		difficultyMax = Mathf.Sqrt(Mathf.Pow(sectorX,2)+Mathf.Pow(sectorY,2));
		difficultyMin = Mathf.Max(difficultyMax/1.3f-10,0);
		Debug.Log("DifficultyMin = "+difficultyMin+" DifficultyMax = "+difficultyMax);

		//Create sectorEnemy[] and sectorEnemyChance[] array
		zone = ZoneData.getRandomZone(difficultyMin,difficultyMax);
		string[] zoneData = ZoneData.getZoneEnemyInfo(zone);
		int numberOfEnemyTypes = 0;
		for(int i = 1;i<6;i++){
			if(zoneData[i]==null)break;
			numberOfEnemyTypes++;
		}
		sectorEnemy = new GameObject[numberOfEnemyTypes];
		sectorEnemyChance = new float[numberOfEnemyTypes];
		for(int i = 0;i<numberOfEnemyTypes;i++){
			sectorEnemy[i] = (GameObject)Resources.Load("Prefabs/Enemy/"+zoneData[i+1]);
			sectorEnemyChance[i] = float.Parse(zoneData[i+6]);
		}
	//	Debug.Log("Number of enemies type: "+sectorEnemy.Length);
	//	Debug.Log("Number of enemies typeL: "+sectorEnemyChance.Length);

		//Set background
		background.sharedMaterial.SetTexture("_MainTex",(Texture)Resources.Load("Textures/"+zoneData[11]));
		background.sharedMaterial.SetTextureScale("_MainTex",new Vector2(float.Parse(zoneData[12]),float.Parse(zoneData[13])));

		//Change sector text
		sectorText.text = "Sector: ("+sectorX+" , "+sectorY+") - "+zoneData[14];

	}
}