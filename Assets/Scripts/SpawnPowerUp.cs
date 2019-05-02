using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPowerUp : MonoBehaviour {
	
	public GameObject powerUp;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < this.transform.childCount; i+=2) {
			var node = this.transform.GetChild(i);
			Transform pos = node.transform;

            for (int j = -2; j < 2; j++)
            {
                //GameObject g = (GameObject)Instantiate(powerUp, node.transform.right * 2 * j , Quaternion.identity);
                GameObject g = (GameObject)Instantiate(powerUp, pos.position + pos.right * 2 * j , Quaternion.identity);
            }
		}
	}
}
