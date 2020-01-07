using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCtrl : MonoBehaviour
{
	private IEnumerator Coroutine;
	private LaserScript laserScript;
	private GameObject newLaser;
	public GameObject LaserPrefab;

	Vector3 goVector;
	bool isLaserOn;
	bool isZeroPoint = false;
	int count;
	float laserMoveTime; //레이저가 올라가는 시간
	float moveDistance; //레이저의 이동거리

	void Awake()
	{
		switch (tag)
		{
			case "Up":
				goVector = Vector3.up;
				break;
			case "Down":
				goVector = Vector3.down;
				break;
			case "Right":
				goVector = Vector3.right;
				break;
			case "Left":
				goVector = Vector3.left;
				break;
			default:
				break;
		}
	}

	void Start()
	{
		GetComponent<MeshRenderer>().enabled = false;
		moveDistance = GameManager.instance.laseMoveDistance;
		LaserSetting();
		count = 0;
	}

	void Update()
	{
		laserMoveTime = GameManager.instance.laserMoveTime;
		if (GameManager.instance.isStart && !isLaserOn) // 스테이지 시작체크
		{
			isLaserOn = true;
			LaserOn();
		}
		else if (!(GameManager.instance.isStart) && isLaserOn)  //스테이지 종료 체크
		{
			isLaserOn = false;
			LaserOff();
		}
	}

	public void LaserSetting()
	{
		laserMoveTime = GameManager.instance.laserMoveTime;
		isLaserOn = false;

		Coroutine = UpLaser();
		newLaser = Instantiate(LaserPrefab);
		laserScript = newLaser.GetComponent<LaserScript>();
		laserScript.trail = false;
		laserScript.firePoint = transform.Find("StartPoint").gameObject;
		laserScript.endPoint = transform.Find("EndPoint").gameObject;
		newLaser.SetActive(true);
		laserScript.ShootLaser(512);
	}

	public void TouchBall()
	{
		for (int i = 0; i < 5; i++)
			if (count >= 0)
			{
				transform.Translate(goVector * -moveDistance);
				count--;
			}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("ZeroPoint"))
			isZeroPoint = true;
		else if (!other.CompareTag("Hold_"))
			GameManager.instance.outBody++;
	}
	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("ZeroPoint"))
		{
			isZeroPoint = false;
		}
		else if (!other.CompareTag("Hold_"))
			GameManager.instance.outBody--;
	}

	public IEnumerator UpLaser()
	{
		while (true)
		{
			if (!isZeroPoint)
			{
				transform.Translate(goVector * moveDistance);
				count++;
				yield return new WaitForSeconds(laserMoveTime);
			}
			else
				yield return null;
		}
	}
	public IEnumerator ResetLaser()
	{
		while (count > 0)
		{
			transform.Translate(goVector * -moveDistance);
			yield return new WaitForSeconds(0.001f);
			count--;
		}
	}

	public void LaserOn()
	{
		StopCoroutine(Coroutine);
		Coroutine = UpLaser();
		StartCoroutine(Coroutine);
	}

	public void LaserOff()
	{
		StopCoroutine(Coroutine);
		Coroutine = ResetLaser();
		StartCoroutine(Coroutine);
	}
}