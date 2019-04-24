using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPowerUp : MonoBehaviour {
	
	public GameObject powerUp;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < this.transform.childCount; i += 2) {
			var node = this.transform.GetChild(i);

			Vector3 pos = node.transform.position; 
			GameObject g = (GameObject)Instantiate(powerUp, pos,Quaternion.identity); 
		}
	}
}
