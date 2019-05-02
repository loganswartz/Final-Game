﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupDrink : MonoBehaviour {

    public Transform target;
    private Transform childTarget;
    private Vector3 finalPosition;
    private Vector3 direction;
    private Rigidbody rb;
    private bool following = true;
    public bool prop = false;
    public AudioSource can;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update () {
        if (!prop)
        {
            if (target != null && childTarget == null)
            {
                //transform.position = Vector3.Lerp(transform.position, target.GetChild(target.childCount-2).transform.position, Time.deltaTime * 3);// 
                Transform[] children = target.GetComponentsInChildren<Transform>();
                foreach (var child in children)
                {
                    if (child.name == "Head")
                    {
                        childTarget = child;

                        finalPosition = child.transform.position;
                        direction = transform.position - finalPosition;
                    }
                }
            }
            if (target != null && following)
            {
                transform.position = Vector3.MoveTowards(transform.position, finalPosition, Time.deltaTime * 40);
                transform.Rotate(3, 3, 3);
            }
            if (transform.position == finalPosition)
            {
                following = false;
                rb.isKinematic = false;
                rb.AddForce(-direction * 250);
            }
        } else
        {
            transform.position = target.position;
            transform.rotation = target.rotation;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "AI" || collision.tag == "Player")
        {
            can.Play();
            following = false;
            rb.isKinematic = false;
            rb.AddExplosionForce(300, transform.position-Vector3.left, 5, 3);
        }
    }
}
