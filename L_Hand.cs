using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_Hand : MonoBehaviour {

	public float speed=0.3f;
	void Update () {
	
		if(Input.GetKey(KeyCode.D)){
			transform.Translate (Vector2.right * speed);
			}

		if(Input.GetKey(KeyCode.A)){
			transform.Translate (-Vector2.right * speed);
			}

		if(Input.GetKey(KeyCode.W)){
			transform.Translate (Vector2.up * speed);
			}

		if(Input.GetKey(KeyCode.S)){
			transform.Translate (-Vector2.up * speed);
		}
	}
}