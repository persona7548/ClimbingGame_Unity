using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartScene : MonoBehaviour {

	public void OnclickStart()
	{
		GameManager.totalPlayTime = 0;
		GameManager.totalLife = 10;
		GameManager.totalScore = 0;
		GameManager.totalDeath = 0;
		Debug.Log("START");
		SceneManager.LoadScene(1);
	}
	public void OnclickEnd()
	{
		Application.Quit();
	}

}
