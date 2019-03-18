using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBroke : MonoBehaviour {

    private bool shrink = false;

	// Use this for initialization
	void Start () {
        transform.localScale = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.localScale.x < 0.146f && !shrink)
        {
            transform.localScale = transform.localScale += new Vector3(0.01f, 0.01f, 0.001f);
        }
        else if (!shrink)
        {
            transform.localScale = new Vector3(0.146f, 0.146f, 0.0058f);
        }

        if (shrink)
        {
            transform.localScale = transform.localScale -= new Vector3(0.01f, 0.01f, 0.001f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            shrink = true;
            StartCoroutine(Des());
        }
    }

    public IEnumerator Des()
    {
        yield return new WaitForSeconds(0.25f);
        Destroy(this.gameObject);
    }
}
