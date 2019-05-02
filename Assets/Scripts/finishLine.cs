using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class finishLine : MonoBehaviour {

    public int p1 = 0;
    public int p2 = 0;
    public int p3 = 0;

    public Text lap;
    public Text win;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        if (p1 == 6 ^ p2 == 3 ^ p3 == 3)
        {
            if (p1 == 6)
            {
                win.gameObject.SetActive(true);
            } else
            {
                win.gameObject.SetActive(true);
                win.text = "You lose...";
            }
            StartCoroutine(done());
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "playermodel")
        {
            p1++;
            if (p1 == 1 || p1 == 2)
            {
                lap.text = "2";
            } else
            {
                lap.text = "3";
            }
        } else if (other.name == "playermodel (1)")
        {
            p2++;
        } else if (other.name == "playermodel (2)")
        {
            p3++;
        }
    }

    private IEnumerator done()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("MenuScene");

    }
}
