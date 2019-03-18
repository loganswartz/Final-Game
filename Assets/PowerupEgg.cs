using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupEgg : MonoBehaviour
{

    public Transform target;
    public bool prop = false;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!prop)
        {
            
        }
        else
        {
            transform.position = target.position;
        }
    }
}
