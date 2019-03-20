using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class changePowerImg : MonoBehaviour {

    public Sprite drink;
    public Sprite egg;
    public Sprite pigeon;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void switchImg(int img)
    {
        switch (img)
        {
            case 0: GetComponent<Image>().sprite = null;
                break;
            case 1: GetComponent<Image>().sprite = drink;
                break;
            case 2: GetComponent<Image>().sprite = egg;
                break;
            case 3: GetComponent<Image>().sprite = pigeon;
                break;
        }
    }
}
