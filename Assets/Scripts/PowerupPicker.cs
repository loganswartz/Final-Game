using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupPicker : MonoBehaviour {

    // This script picks a random powerup when a player collects a powerup
    // Script belongs on actual pickups

    private bool activated = false;
    private Vector3 startingScale;
    private string[] powerUps = { "drink", "egg", "pigeon", "cardboard", "ski" };//, "cone", };

    public GameObject powerupImg;

	// Use this for initialization
	void Start () {
        startingScale = transform.localScale;
	}

	// Update is called once per frame
	void Update () {
        // Constant rotation animation
        transform.Rotate(0, 1, 0);

        // If the powerup has been collected, but is still visible, play shrinking
        // animation and disable the powerup. Then start a coroutine to reset.
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

    // When player collects, activate the powerup and assign a random powerup to
    // the player and the HUD
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            activated = true;
            if (other.gameObject.GetComponent<CharControlNew>().powerup == "")
            {
                int powerup = Random.Range(0, powerUps.Length);
                other.gameObject.GetComponent<CharControlNew>().powerup = powerUps[powerup];
                powerupImg.GetComponent<changePowerImg>().switchImg(powerup + 1);
            }
        }
    }

    // Coroutine to disable PU after playing collection animation
    public IEnumerator kill()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
    }

    // Coroutine to reset PU after being collected
    public IEnumerator reset()
    {
        yield return new WaitForSeconds(5);
        transform.localScale = startingScale;
        activated = false;
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
    }
}
