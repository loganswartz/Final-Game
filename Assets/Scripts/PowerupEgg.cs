using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupEgg : MonoBehaviour
{

    public Transform target;
    public bool prop = false;
    public GameObject eggBroke;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Instantiate(eggBroke, transform.position, Quaternion.Euler(-90, 0, 0));
            Destroy(this.gameObject);
        }
    }
}
