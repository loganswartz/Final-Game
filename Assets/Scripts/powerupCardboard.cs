using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerupCardboard : MonoBehaviour {

    // Script controlling cardboard powerup

    public GameObject target;
    public bool prop = false;
    public bool move = true;
    public bool audio = true;

	// Use this for initialization
	void Start () {
        //StartCoroutine(kill());
	}
	
	// Update is called once per frame
	void Update () {
		
        // If prop, follow hand. Otherwise, fly forward and land on ground
        if (target != null && target.name != "Hand" && prop)
        {
            Transform[] children = target.GetComponentsInChildren<Transform>();
            foreach (var child in children)
            {
                if (child.name == "Hand")
                {
                    target = child.gameObject;

                }
            }
        }

        if (!prop)
        {
            if (audio)
            {
                GetComponent<AudioSource>().Play();
                audio = false;
            }
            if (move)
            {
                transform.position = transform.position + target.transform.forward + transform.up * Time.deltaTime * 0.9f;
                transform.Rotate(transform.rotation.x + 5, transform.rotation.y, transform.rotation.z, Space.Self);
            }
        }
        else
        {
            transform.position = target.transform.position;
            transform.rotation = target.transform.rotation;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            move = false;
        }
    }

    public IEnumerator kill()
    {
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }
}
