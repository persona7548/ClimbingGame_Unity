using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	static public float totalPlayTime;
	static public int totalLife = 10;
	static public int totalScore;
	static public int totalDeath;
	public Text clearText;
	public Text targetScoreText;
	public Text countdown;
	public Text failText;
	public Text waitText;
	public Text startText;
	public Text timeText;
	public Text lvText;
	public AudioClip bgm;
	public AudioClip winSound;
	public Image hp;
	public Image RHandFollow;
	public Image LHandFollow;
	public GameObject prefabGenerator;
	[HideInInspector] public bool isStart = false;
	[HideInInspector] public float targetScoreCheck = 0;
	[HideInInspector] public int score = 0;
	[HideInInspector] public int outBody = 0;//필드 밖으로 나간 신체 체크
	AudioSource audioSource;
	GameObject Sphere;
	float playTime = 0;
	float count;
	float startCount = 3;
	int stageLevel;
	int speedCount = 0;
	
	/// 난이도 조절 부분///
	public float targetScore = 10; //스테이지를 넘어가기 위한 점수	
	public float laseMoveDistance = 0.05f;
	public float outCount = 3;//맵 밖으로 나갔을때 제한시간 조절
	public float laserMoveTime = 0.15f;
	

	void Awake()
	{
		instance = this;
	}
	void Start()
	{
		audioSource = GetComponent<AudioSource>();
		FindObject();
		clearText.enabled = false;
		targetScoreCheck = targetScore;
		outBody = 0;
		score = 0;
		isStart = false;
		stageLevel = SceneManager.GetActiveScene().buildIndex;
		lvText.text = "Stage Lv" + stageLevel.ToString();
		WaitStart();
		targetScoreText.text = "Touch : " + targetScoreCheck.ToString();
	}

	void Update()
	{
		KeySetting();
		if (isStart)
			PlayCheck();
		//else
		//	CheckKinect();
	}

	void CheckKinect()
	{

		if (KinectManager.Instance.IsUserDetected() && !isStart && outBody == 0)
		{
			waitText.enabled = false;
			startText.enabled = true;
			startCount -= Time.deltaTime;
			startText.text = startCount.ToString("0") + "초 후에 시작합니다.";
		}
		else
			startCount = 3;
		if (startCount <= 0)
			StartGame();
	}

	void WaitStart() 
	{
		if (totalLife == 0)
			SceneManager.LoadScene("Clear");
		failText.enabled = false;
		waitText.enabled = true;
		GameObject go = Instantiate(prefabGenerator) as GameObject;
		Sphere = go;
		Sphere.transform.position = new Vector3(0, 0, 0);
		startCount = 3;

		//////////////// 임시 시작코드//////////////////////
		Invoke("StartGame", 3);
	}

	void StartGame()
	{
		audioSource.PlayOneShot(bgm);
		playTime = 0;
		waitText.enabled = false;
		startText.enabled = false;
		Sphere.GetComponent<SphereCollider>().enabled = true;
		hp.enabled = true;
		isStart = true;
	}

	void ResetAll() 
	{
		audioSource.Stop();
		Destroy(Sphere);
		playTime = 0;
		score = 0;
		hp.enabled = false;
		countdown.enabled = false;
		failText.enabled = true;
		isStart = false;
		LHandFollow.enabled = false;
		RHandFollow.enabled = false;
		targetScoreCheck = targetScore;
		totalLife--;
		totalDeath++;
		totalScore += score;
		Invoke("WaitStart", 2);
	}

	void NextStageCheck()
	{
		if (targetScoreCheck == 0)
		{
			clearText.enabled = true;
			countdown.enabled = false;
			Sphere.GetComponent<SphereCollider>().enabled = false;
			isStart = false;
			audioSource.Stop();
			audioSource.PlayOneShot(winSound);
			Invoke("GoNextStage", 3);
		}
	}

	void GoNextStage()
	{
		totalScore += score;
		SceneManager.LoadScene(stageLevel + 1);
	}

	void PlayCheck() // 게임중에 지속적으로 체크해야될 목록
	{
		playTime += Time.deltaTime;
		totalPlayTime += Time.deltaTime;
		timeText.text = "Time: " + playTime.ToString("0.0");
		targetScoreText.text = "Touch : " + targetScoreCheck.ToString();
		NextStageCheck();
		LHandFollow.enabled = true;
		RHandFollow.enabled = true;

		//////////////// 임시 시작코드//////////////////////
		//LHandFollow.transform.position = GameObject.Find("HandLeftPLAYER").transform.position;
		//RHandFollow.transform.position = GameObject.Find("HandRightPLAYER").transform.position; 
		if (outBody == 0)
		{
			countdown.enabled = false;
			count = outCount;
		}
		else
		{
			count -= Time.deltaTime;
			countdown.enabled = true;
			countdown.text = "안쪽으로 들어오세요!\n" + (count + 1).ToString("0");
		}
		if (count < 0)
		{
			count = outCount;
			ResetAll();
		}
		if (Sphere != null)
		{
			hp.fillAmount = targetScoreCheck / targetScore;
			hp.transform.position = Sphere.transform.position;
		}
	}

	
	void KeySetting()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			SceneManager.LoadScene("Start");
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			totalLife--;
			totalDeath++;
			SceneManager.LoadScene(stageLevel);
		}
	}
	void FindObject()
	{
		clearText = GameObject.Find("Clear").GetComponent<Text>();
		targetScoreText = GameObject.Find("TargetScore").GetComponent<Text>();
		countdown = GameObject.Find("Countdown").GetComponent<Text>();
		failText = GameObject.Find("Fail").GetComponent<Text>();
		waitText = GameObject.Find("WaitStart").GetComponent<Text>();
		startText = GameObject.Find("Start").GetComponent<Text>();
		timeText = GameObject.Find("Playtime").GetComponent<Text>();
		lvText = GameObject.Find("StageLv").GetComponent<Text>();
		hp = GameObject.Find("HpImage").GetComponent<Image>();
		RHandFollow = GameObject.Find("R_Hand_Follow").GetComponent<Image>();
		LHandFollow = GameObject.Find("L_Hand_Follow").GetComponent<Image>();
	}
}
