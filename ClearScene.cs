using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ClearScene : MonoBehaviour {

	public Text Score;
	public Text Death;
	public Text TotalTime;
	public Text BestScore;

	public int totalScore;
	int bestScore;
	void Start () {
		bestScore = PlayerPrefs.GetInt("BestScore");
		totalScore = 100000 - (int)(GameManager.totalDeath*5000 + GameManager.totalPlayTime * 500);
		if (bestScore < totalScore)
			PlayerPrefs.SetInt("BestScore", totalScore);

		Score.text = "Score : " + totalScore.ToString();
		Death.text = "Death : " + GameManager.totalDeath.ToString()+"회";
		TotalTime.text = "Total Play Time : " + GameManager.totalPlayTime.ToString("0.0")+"초";
		BestScore.text = "Best Score : " + bestScore;
		
	}
	public void OnclickStart()
	{
		SceneManager.LoadScene("Start");
	}
}
