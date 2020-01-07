using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
	public static Sphere instance;
	public List<GameObject> Lasers = new List<GameObject>();
	string tagTemp;
	AudioSource audioSource;
	private void Awake()
	{
		instance = this;
	}
	void Start()
	{
		for (int i = 0; i < 4; i++)
			Lasers[i] = GameObject.Find("Laser").transform.GetChild(i).gameObject;
		tag = "R_Hand";
		GetComponent<MeshRenderer>().material.color = Color.blue;
		audioSource = GetComponent<AudioSource>();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(tag))
		{
			audioSource.Play();
			GetComponent<ParticleSystem>().Play();
			if (GameManager.instance.targetScoreCheck > 0)
				GameManager.instance.targetScoreCheck--;
			GameManager.instance.score++;

			int r = Random.Range(-1, 1) * 300;
			if (r == 0)
				r = 300;

			int power = Random.Range(-1, 1) * 300;
			if (power == 0)
				power = 300;
			this.GetComponent<Rigidbody>().AddForce(new Vector3(r, power, 0));

			for (int i = 0; i < 4; i++)
				Lasers[i].GetComponent<LaserCtrl>().TouchBall();

			tag = "CoolTime";
		
			if (other.CompareTag("R_Hand"))
			{
				tagTemp = "L_Hand";
				GetComponent<MeshRenderer>().material.color = Color.green;
			}
			else
			{
				tagTemp = "R_Hand";
				GetComponent<MeshRenderer>().material.color = Color.blue;
			}
			Invoke("CoolTime", 0.5f);
		}
	}

	void CoolTime() {
		tag = tagTemp;
	}
}  