using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class menuController : MonoBehaviour {

    // This script controls features of the menu

    public int currSkin = 0;
    public int currClothes = 0;
    public Material[] skins;
    public Material[] clothes;
    public Material[] origMats;
    public GameObject dummy;
    public Renderer dummyRend;

    public bool fadeout = false;
    public bool fadein = false;
    public bool movetoSR = false;
    public GameObject sceneChange;
    public GameObject menu1;
    public GameObject menu2;
    Quaternion rot1;
    Quaternion rot2;

	// Use this for initialization
	void Start () {
        dummyRend = dummy.GetComponent<Renderer>();
        origMats = dummyRend.materials;
        Quaternion origRot = transform.rotation;

        transform.eulerAngles = new Vector3(-2.094f, -3.809f, 0);
        rot1 = transform.rotation;
        transform.eulerAngles = new Vector3(-2.094f, -26.456f, 0);
        rot2 = transform.rotation;

        transform.rotation = origRot;
    }
	
	// Update is called once per frame
	void Update () {
        // dummyRend.materials[0] = clothes[currClothes];
        //dummyRend.materials[1] = skins[currSkin];
        //dummyRend.materials[2] = skins[currSkin];

        // Lerp between two material values. This is for the customization screen.
        origMats[0] = clothes[currClothes];
        origMats[1] = skins[currSkin];
        origMats[2] = skins[currSkin];
        dummyRend.materials[0].Lerp(dummyRend.materials[0], origMats[0], Time.deltaTime * 8);
        dummyRend.materials[1].Lerp(dummyRend.materials[1], origMats[1], Time.deltaTime * 8);
        dummyRend.materials[2].Lerp(dummyRend.materials[2], origMats[2], Time.deltaTime * 8);

        // If fadeout state is active, fade to black
        if (fadeout)
        {
            sceneChange.GetComponent<Image>().color = Color.Lerp(sceneChange.GetComponent<Image>().color, new Color(0, 0, 0, 1), Time.deltaTime * 8);
        }
        // If fade in state is active, fade out black
        if (fadein)
        {
            sceneChange.GetComponent<Image>().color = Color.Lerp(sceneChange.GetComponent<Image>().color, new Color(0, 0, 0, 0), Time.deltaTime * 8);
        }
        // If ShowRoom state active, set the position to be in the showroom and
        // lerp to the new pos and rotation
        if (movetoSR)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(-37.21f, 2.99f, 5.18f), Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot2, Time.deltaTime);
        }

    }


    // These two functions cycle the skin and clothing of the player
    public void incrementSkin()
    {
        if (currSkin < 3)
        {
            currSkin++;
        } else
        {
            currSkin = 0;
        }
    }

    public void incrementClothes()
    {
        if (currClothes < 3)
        {
            currClothes++;
        }
        else
        {
            currClothes = 0;
        }

    }

    // Setup fadeout state
    public void fadeOut()
    {
        fadein = false;
        fadeout = true;
        menu1.SetActive(false);
        StartCoroutine(fadeBack());
    }

    // Setup fadein state
    public void fadeIn()
    {
        fadein = true;
        fadeout = false;
    }

    // This is called when the player hits the play button, setting up the showroom
    // requirements and states
    public IEnumerator fadeBack()
    {
        yield return new WaitForSeconds(1);
        menu2.SetActive(true);
        transform.position = new Vector3(-36.698f, 2.99f, 6.57f);
        transform.rotation = rot1;
        movetoSR = true;
        //moveToShowroom();
        fadeIn();
    }

    // Start game; save chosen customizations and load scene
    public void initGame()
    {
        PlayerPrefs.SetInt("Current Skin", currSkin);
        PlayerPrefs.SetInt("Current Clothes", currClothes);
        SceneManager.LoadScene("Scenes/MainScene");
    }
}
