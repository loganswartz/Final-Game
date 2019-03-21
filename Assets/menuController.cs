using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class menuController : MonoBehaviour {

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
        origMats[0] = clothes[currClothes];
        origMats[1] = skins[currSkin];
        origMats[2] = skins[currSkin];
        dummyRend.materials[0].Lerp(dummyRend.materials[0], origMats[0], Time.deltaTime * 8);
        dummyRend.materials[1].Lerp(dummyRend.materials[1], origMats[1], Time.deltaTime * 8);
        dummyRend.materials[2].Lerp(dummyRend.materials[2], origMats[2], Time.deltaTime * 8);

        if (fadeout)
        {
            sceneChange.GetComponent<Image>().color = Color.Lerp(sceneChange.GetComponent<Image>().color, new Color(0, 0, 0, 1), Time.deltaTime * 8);
        }
        if (fadein)
        {
            sceneChange.GetComponent<Image>().color = Color.Lerp(sceneChange.GetComponent<Image>().color, new Color(0, 0, 0, 0), Time.deltaTime * 8);
        }
        if (movetoSR)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(-37.21f, 2.99f, 5.18f), Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot2, Time.deltaTime);
        }

    }

    //public void moveToShowroom()
    //{
    //    transform.position = new Vector3(-37.21f, 2.99f, 5.18f);
    //    transform.eulerAngles = new Vector3(-2.094f, -26.456f, 0);
   // }

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

    public void fadeOut()
    {
        fadein = false;
        fadeout = true;
        StartCoroutine(fadeBack());
    }

    public void fadeIn()
    {
        fadein = true;
        fadeout = false;
    }

    public IEnumerator fadeBack()
    {
        yield return new WaitForSeconds(1);
        transform.position = new Vector3(-37.21f, 2.99f, 5.18f);
        transform.rotation = rot1;
        movetoSR = true;
        //moveToShowroom();
        fadeIn();
    }
}
