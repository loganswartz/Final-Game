using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupPicker : MonoBehaviour {

    private bool activated = false;
    private Vector3 startingScale;
    private string[] powerUps = { "drink", "egg", "pigeon" };//, "cone", "cardboard", };
	// Use this for initialization
	void Start () {
        startingScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 1, 0);

        if (activated)
        {
            if (transform.localScale.y > 0)
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - 0.1f, transform.localScale.z);
                StartCoroutine(kill());
                StartCoroutine(reset());
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            activated = true;
            if (other.gameObject.GetComponent<CharControlNew>().powerup == "")
            {
                other.gameObject.GetComponent<CharControlNew>().powerup = powerUps[Random.Range(0, powerUps.Length)];
            }
        }
    }

    public IEnumerator kill()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
    }

    public IEnumerator reset()
    {
        yield return new WaitForSeconds(5);
        transform.localScale = startingScale;
        activated = false;
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
    }
}
