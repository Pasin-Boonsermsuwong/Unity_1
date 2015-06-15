using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
	public GameObject hazard;
	public GameObject powerup;
	public Vector3 spawnValues;
	public int hazardCount;
	public int hazardInc;
	public float spawnWait;
	public float spawnWaitDec;
	public float scale;
	public float scaleInc;
	public float speed;
	public float speedInc;
	public float startWait;
	public float waveWait;
	
	public Text scoreText;
	public Text restartText;
	public Text gameOverText;
	public Text waveText;
	//Pause stuff
	public GameObject pauseObject;
//	public Text pauseText;
	public Canvas invCanvas;


	bool gameOver;
	bool restart;
	public static bool pause;
	int score;
	int wave;

	void Start ()
	{
		pauseObject.SetActive(false);
	//	pauseText.enabled = false;
		invCanvas.enabled = false;
		gameOver = false;
		restart = false;
		restartText.text = "";
		gameOverText.text = "";
		score = 0;
		UpdateScore ();
		wave = 1;
		StartCoroutine (SpawnWaves ());
		StartCoroutine (SpawnPowerups());
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
				invCanvas.enabled = true;
			}
			if(!pause){
				Time.timeScale = 1;
				pauseObject.SetActive(false);
				invCanvas.enabled = false;
			}

		}
	}

	Vector3 randomSpawnPosition(){
		int ran = Random.Range(0,3);
		Vector3 spawnPosition = new Vector3(0,0,0);
		switch (ran)
		{
		case 3:
			spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
			break;
		case 2:
			spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, -spawnValues.z);
			break;
		case 1:
			spawnPosition = new Vector3 (spawnValues.x, spawnValues.y, Random.Range (-spawnValues.z, spawnValues.z));
			break;
		case 0:
			spawnPosition = new Vector3 (-spawnValues.x, spawnValues.y, Random.Range (-spawnValues.z, spawnValues.z));
			break;
		}
		return spawnPosition;
	}
	IEnumerator SpawnPowerups ()
	{
		yield return new WaitForSeconds (startWait);
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
				(new Vector3 (Random.Range (-14, 14), spawnValues.y,Random.Range (-14, 14)) - spawnPosition).normalized
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
	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds (startWait);
		while (true)
		{
			waveText.text = "Wave: "+wave;
			for (int i = 0;i<4;i++) {
				waveText.color = Color.red;
				yield return new WaitForSeconds (waveWait/16);
				waveText.color = Color.white;
				yield return new WaitForSeconds (waveWait/16);
			}
			for (int i = 0; i < hazardCount; i++){
				//	GameObject hazard = hazards [Random.Range (0, hazards.Length)];
				Vector3 spawnPosition = randomSpawnPosition();
				Instantiate (hazard,
					         spawnPosition,
					         Quaternion.Slerp(
								transform.rotation,
								Quaternion.LookRotation(
									(new Vector3 (Random.Range (-14, 14), spawnValues.y,Random.Range (-14, 14)) - spawnPosition).normalized
									),
									360
								)
							);
				yield return new WaitForSeconds (spawnWait);
			}
			//PREPARE NEXT WAVE
			spawnWait -= spawnWaitDec;
			spawnWait = Mathf.Max(spawnWait,0.05f);
			hazardCount += hazardInc;
			scale += scaleInc;
			speed += speedInc;
			yield return new WaitForSeconds (waveWait/2);
			if (gameOver)
			{
				restartText.text = "Press 'R' for Restart";
				restart = true;
				break;
			}
			wave++;
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
}