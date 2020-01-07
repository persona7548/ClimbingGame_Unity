using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
	int level;
	
	void Start()
	{
		level = 1;
	}

	void OnStartClick()
	{
		SceneManager.LoadScene("Stage_"+level.ToString());
	}
	void OnBackClick()
	{
		SceneManager.LoadScene("Start");
	}
	void OnNextClick()
	{
		level += 1;
	}
	void OnPreviousClick()
	{
		if (level >=2)
			level -= 1;

	}
}

