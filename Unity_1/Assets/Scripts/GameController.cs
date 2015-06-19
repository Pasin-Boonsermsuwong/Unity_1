﻿	using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
	public GameObject player;

	public GameObject hazard;
	public GameObject powerup;
	public Vector3 spawnValues;
	public int hazardCount;
	public int hazardInc;
	public Text scoreText;
	public Text restartText;
	public Text gameOverText;
	public Text sectorText;
	//Pause stuff
	public GameObject pauseObject;

	//Main Canvas
	bool gameOver;
	bool restart;
	public static bool pause;
	int score;
	int wave;

	//Sector
	float boundaryX;
	float boundaryZ;
	float boundaryInnerX;//for spawn target
	float boundaryInnerZ;
	float boundarySpawnX;//for spawn position
	float boundarySpawnZ;

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
		GameObject bgGameObject = GameObject.FindWithTag("Background");
		background = bgGameObject.GetComponent<Renderer>();
		boundaryX = bgGameObject.transform.localScale.x/2;
		boundaryZ = bgGameObject.transform.localScale.y/2;
		boundaryInnerX = boundaryX * 0.7f;	
		boundaryInnerZ = boundaryZ * 0.7f;
		boundarySpawnX = boundaryX + 50;	
		boundarySpawnZ = boundaryZ + 50;
		player = GameObject.FindWithTag("Player");
	//	background.material.color = Color.red;

//		Debug.Log("BG: "+background);
		sectorX = 0;
		sectorY = 0;
		SwitchSector(0,0);
		pauseObject.SetActive(false);
	//	pauseText.enabled = false;
	//	invCanvas.enabled = false;
		gameOver = false;
		restart = false;
		restartText.text = "";
		gameOverText.text = "";
		score = 0;
		UpdateScore ();
		wave = 1;


	//	StartCoroutine (SpawnWaves ());
		StartCoroutine (SpawnPowerups());
	}
	void FixedUpdate(){
		//Spawn enemies
		if(!gameOver){
			for(int i = 0;i<sectorEnemy.Length;i++){
				if(!Data.chance(sectorEnemyChance[i]))continue;
				Vector3 spawnPosition = randomSpawnPosition();
				Instantiate (hazard,
				             spawnPosition,
				             Quaternion.Slerp(
					transform.rotation,
					Quaternion.LookRotation(
					(new Vector3 (Random.Range (-boundaryInnerX, boundaryInnerX), 0,Random.Range (-boundaryInnerZ, boundaryInnerZ)) - spawnPosition).normalized
					),
					360
					)
				             );
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
		if(Input.GetKeyDown("p")){
			pause = !pause;
			if(pause){
				Time.timeScale = 0;
				pauseObject.SetActive(true);
	//			invCanvas.enabled = true;
			}
			if(!pause){
				Time.timeScale = 1;
				pauseObject.SetActive(false);
	//			invCanvas.enabled = false;
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

	IEnumerator SpawnPowerups ()
	{
		while (true)
		{
			yield return new WaitForSeconds (Random.Range(5,15));
		//	Debug.Log("Powerup spawned");
			Vector3 spawnPosition = randomSpawnPosition();
			Instantiate (powerup,
			             spawnPosition,
			             Quaternion.Slerp(
				transform.rotation,
				Quaternion.LookRotation(
				(new Vector3 (Random.Range (-boundaryInnerX, boundaryInnerX), 0,Random.Range (-boundaryInnerZ, boundaryInnerZ)) - spawnPosition).normalized
				),
				360
				)
			             );
			if (gameOver)
			{
				break;
			}
		}
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
	public void SwitchSector (int dx,int dy){
		sectorX += dx;
		sectorY += dy;
		sectorText.text = "Sector: "+sectorX+" , "+sectorY;

		//Calculate difficulty based on distance
		difficultyMax = Mathf.Sqrt(Mathf.Pow(sectorX,2)+Mathf.Pow(sectorX,3));
		difficultyMin = Mathf.Max(difficultyMax-20,0);
		Debug.Log("DifficultyMin = "+difficultyMin+" DifficultyMax = "+difficultyMax);

		// create sectorEnemy[] and chance array
		zone = ZoneData.getRandomZone(difficultyMin,difficultyMax);
		string[] zoneEnemies = ZoneData.getZoneEnemyInfo(zone);
		int numberOfEnemyTypes = 0;
		for(int i = 1;i<zoneEnemies.Length;i++){
			if(zoneEnemies[i]==null)break;
			numberOfEnemyTypes++;
		}
		sectorEnemy = new GameObject[numberOfEnemyTypes];
		sectorEnemyChance = new float[numberOfEnemyTypes];
		for(int i = 0;i<numberOfEnemyTypes;i++){
			sectorEnemy[i] = GameObject.Find(zoneEnemies[i+1]);
			sectorEnemyChance[i] = float.Parse(zoneEnemies[i+6]);
			Debug.Log("sectorEnemy[i]: "+sectorEnemy[i]);
			Debug.Log("sectorEnemyChance[i]: "+sectorEnemyChance[i]);
		}
		Debug.Log("Number of enemies type: "+sectorEnemy.Length);

	}
}