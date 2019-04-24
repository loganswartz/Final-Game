using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBroke : MonoBehaviour {

    // This script is applied to the *active* egg powerup -- eg, after it has
    // hit the ground.

    private bool shrink = false;

	// Use this for initialization
	void Start () {
        transform.localScale = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
        // play an initial animation to create the egg
        if (transform.localScale.x < 0.146f && !shrink)
        {
            transform.localScale = transform.localScale += new Vector3(0.01f, 0.01f, 0.001f);
        }
        else if (!shrink)
        {
            transform.localScale = new Vector3(0.146f, 0.146f, 0.0058f);
        }

        // play a shrinking animation if the egg has been hit
        if (shrink)
        {
            transform.localScale = transform.localScale -= new Vector3(0.01f, 0.01f, 0.001f);
        }
    }

    // When player hits the egg, begin shrinking animation and start coroutine
    // to destroy egg
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "AI")
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
