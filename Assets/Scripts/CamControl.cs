﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour {

    // The script for the camera. The camera is not directly controllable,
    // but rather rotates to stay behind the player at all times. The camera
    // will rotate if the player is holding the W key and nothing else, or if
    // the player has passed some max turning angle (although this is based on
    // time holding the button, not actual angles).


	public Transform player;
	private Vector3 pos;
	private float rotx = 0.0f;
	private float roty = 0.0f;
	public Camera cam;


	// Use this for initialization
	void Start () {
		pos = new Vector3(player.position.x, player.position.y, player.position.z);
		cam = GetComponentInChildren<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
        // Follow player and get turning time
        float holdTime = player.GetComponent<CharControlNew>().startTime - player.GetComponent<CharControlNew>().holdTime;
		transform.position = player.position + pos;
        //rotx += Input.GetAxis ("Mouse Y") * 40 * Time.deltaTime;
        //rotx = Mathf.Clamp (rotx, -45, 45);
        //roty += Input.GetAxis("Mouse X") * 80 * Time.deltaTime;
        //transform.rotation = Quaternion.Euler(rotx, roty, 0.0f);
        //transform.rotation = new Quaternion(transform.rotation.x, player.rotation.y, 0.0f, transform.rotation.w);

        // If one of the above mentioned situations is true, begin rotating to
        // match player
        if (player.GetComponent<CharControlNew>().moveCam || Mathf.Abs(holdTime) > 0.5f || player.GetComponent<CharControlNew>().speed < 0.75f)
        {
            Vector3 eulerRot = new Vector3(rotx, player.transform.eulerAngles.y, 0.0f);
            Quaternion rot = Quaternion.Euler(eulerRot);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Mathf.SmoothStep(0.0f, 1.0f, 3)* Time.deltaTime);
            //transform.rotation = rot;
        }



    }
}
