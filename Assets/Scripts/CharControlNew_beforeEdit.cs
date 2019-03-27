using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharControlNew_beforeEdit : MonoBehaviour {

	public Animator anim;
	public Camera cam;
	public CharacterController cc;

    private Vector3 MoveDirection;
    private Vector3 LookDir;
    private Quaternion LookAt;

    public bool ragdoll = false;
    public Rigidbody[] bodies;
    private bool canRagdoll = true;

    private Rigidbody rb;
    private float speed = 0.5f;
    private float speedLimit =  10 ;
    private string dir;

    public GameObject hand;
    public GameObject hand2;
    public GameObject gameManager;
    public string powerup = "";
    public GameObject runnerInFront;

    private GameObject prop;
    private GameObject prop2;
    public GameObject powerupImage;
    public GameObject drinkPowerup;
    public GameObject eggPowerup;
    public GameObject pigeonPowerup;
    public GameObject cardboardPowerup;
    public GameObject skiPowerup;

    public Transform[] children;
    public List<Vector3> resetPositions;
    public List<Quaternion> resetRotations;

    public GameObject mesh;
    SkinnedMeshRenderer meshRend;
    public Material[] skins;
    public Material[] clothes;
    public Material[] origMats;
    int currSkin;
    int currClothes;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
		cc = GetComponent<CharacterController> ();
		anim = GetComponent<Animator>();
        resetPositions = new List<Vector3>();
        
        bodies = GetComponentsInChildren<Rigidbody>();
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            resetPositions.Add(child.transform.position);
            resetRotations.Add(child.transform.rotation);
            if (child.gameObject.name == "LHand")
            {
                hand = child.gameObject;
            } else if (child.gameObject.name == "LHand_001")
            {
                hand2 = child.gameObject;
            }
        }

        meshRend = mesh.GetComponent<SkinnedMeshRenderer>();
        currSkin = PlayerPrefs.GetInt("Current Skin");
        currClothes = PlayerPrefs.GetInt("Current Clothes");

        origMats = meshRend.materials;
        Debug.Log(currClothes);
        origMats[0] = clothes[currClothes];
        origMats[1] = skins[currSkin];
        origMats[2] = skins[currSkin];
        meshRend.materials = origMats;

    }

    // Update is called once per frame
    void FixedUpdate () {

        // Determine which direction to run in relative to camera, and speed
        if (Input.GetKey(KeyCode.W)) {
            anim.SetBool("run", true);
            if (speed <= speedLimit) {
                speed *= 1.05f;
            }
            else
            {
                speed /= 1.05f;
            }
            if (Input.GetKey (KeyCode.A)) {
                dir = "NE";
            } else if (Input.GetKey (KeyCode.D)) {
                dir = "NW";
            } else {
                dir = "N";
            }

        } else if (Input.GetKey (KeyCode.S)) {
            anim.SetBool("run", true);
            if (speed <= speedLimit)
            {
                speed *= 1.05f;
            }
            else
            {
                speed /= 1.05f;
            }
            if (Input.GetKey (KeyCode.A)) {
                dir = "SE";
			} else if (Input.GetKey (KeyCode.D)) {
                dir = "SW";
			} else {
                dir = "S";
            }
        } else if (Input.GetKey (KeyCode.A)) {
            if (speed <= speedLimit)
            {
                speed *= 1.05f;
            }
            else
            {
                speed /= 1.05f;
            }
            dir = "E";
		} else if (Input.GetKey (KeyCode.D)) {
            if (speed <= speedLimit)
            {
                speed *= 1.05f;
            }
            else
            {
                speed /= 1.05f;
            }
            dir = "W";
			anim.SetBool ("run", true);
		} else {
            if (speed >= 0.75f)
            {
                speed /= 1.05f;
            }
        }

        // If speed is at a certain level, move forward; otherwise stay still 
        if (speed > 0.75f)
        {
            anim.speed = speed / 10;
            MoveDirection = transform.forward * speed * Time.deltaTime;
            anim.SetBool("run", true);
        }
        else
        {
            anim.speed = 1;
            anim.SetBool("run", false);
            MoveDirection = Vector3.zero;
        }


        // Determine which way to rotate the character
        if (!ragdoll)
        {
            switch (dir)
            {
                case "N":
                    LookDir = cam.transform.forward * speed * Time.deltaTime;
                    break;
                case "NE":
                    LookDir = (cam.transform.forward - cam.transform.right) * speed * Time.deltaTime;
                    break;
                case "NW":
                    LookDir = (cam.transform.forward + cam.transform.right) * speed * Time.deltaTime;
                    break;
                case "S":
                    LookDir = -cam.transform.forward * speed * Time.deltaTime;
                    break;
                case "SE":
                    LookDir = (-cam.transform.forward - cam.transform.right) * speed * Time.deltaTime;
                    break;
                case "SW":
                    LookDir = (-cam.transform.forward + cam.transform.right) * speed * Time.deltaTime;
                    break;
                case "E":
                    LookDir = -cam.transform.right * speed * Time.deltaTime;
                    break;
                case "W":
                    LookDir = cam.transform.right * speed * Time.deltaTime;
                    break;
            }
        }


        // Gravity exists
        if (!cc.isGrounded) {
			cc.Move (-transform.up * 10 * Time.deltaTime);
		}

		cc.Move (MoveDirection);

		if (LookDir != Vector3.zero) {
			LookAt = Quaternion.LookRotation (LookDir);
		}

        // If a powerup is held, throw it when pressing space
        if (powerup != "" && Input.GetKey("space"))
        {
            powerupImage.GetComponent<changePowerImg>().switchImg(0);
            if (powerup == "drink")
            {
                anim.SetBool("throw", true);
                StartCoroutine(disableThrow("throw"));

                prop = Instantiate(drinkPowerup, hand.transform.position, transform.rotation);
                prop.GetComponent<CapsuleCollider>().enabled = false;
                prop.GetComponent<PowerupDrink>().prop = true;
                prop.GetComponent<PowerupDrink>().target = hand.transform;
                runnerInFront = gameManager.GetComponent<GameManager>().getInFront(this.gameObject);
                StartCoroutine(spawnPowerup(drinkPowerup));
            } else if (powerup == "egg")
            {
                anim.SetBool("drop", true);
                StartCoroutine(disableThrow("drop"));

                prop = Instantiate(eggPowerup, hand.transform.position, transform.rotation);
                prop.GetComponent<SphereCollider>().enabled = false;
                prop.GetComponent<PowerupEgg>().prop = true;
                prop.GetComponent<PowerupEgg>().target = hand.transform;
                StartCoroutine(spawnPowerup(eggPowerup));
            }
            else if (powerup == "pigeon")
            {
                anim.SetBool("throw", true);
                StartCoroutine(disableThrow("throw"));

                prop = Instantiate(pigeonPowerup, hand.transform.position, transform.rotation);
                prop.GetComponent<BoxCollider>().enabled = false;
                prop.GetComponent<PowerupPigeon>().prop = true;
                prop.GetComponent<PowerupPigeon>().target = hand;
                runnerInFront = gameManager.GetComponent<GameManager>().getInFront(this.gameObject);
                StartCoroutine(spawnPowerup(pigeonPowerup));
            }
            else if (powerup == "cardboard")
            {
                anim.SetBool("throw", true);
                StartCoroutine(disableThrow("throw"));

                prop = Instantiate(cardboardPowerup, hand.transform.position, transform.rotation);
                prop.GetComponent<BoxCollider>().enabled = false;
                prop.GetComponent<powerupCardboard>().prop = true;
                prop.GetComponent<powerupCardboard>().target = hand;
                StartCoroutine(spawnPowerup(cardboardPowerup));
            }
            else if (powerup == "ski")
            {
                anim.SetBool("ski", true);

                prop = Instantiate(skiPowerup, hand.transform.position, transform.rotation);
                prop.GetComponent<PowerupSki>().target = hand;
                prop2 = Instantiate(skiPowerup, hand2.transform.position, transform.rotation);
                prop2.GetComponent<PowerupSki>().target = hand2;
                speedLimit = 20;
                StartCoroutine(disableSki());

            }
            powerup = "";
        }

        Quaternion LookRotationLimit = Quaternion.Euler (transform.rotation.eulerAngles.x, LookAt.eulerAngles.y, transform.rotation.eulerAngles.z);
		transform.rotation = Quaternion.Slerp (transform.rotation, LookRotationLimit, 0.025f);
		
	}


    // Ragdoll is toggled by disabling animators for every body part, and adding physics to every rigidbody. Disabling is done in the reverse.
    void toggleRagdoll()
    {
        if (!ragdoll)
        {
            ragdoll = true;
            for (var i = 0; i < bodies.Length; i++)
            {
                bodies[i].isKinematic = false;
            }
            this.gameObject.GetComponent<Animator>().enabled = false;
            StartCoroutine(ragdollEnable());
            StartCoroutine(ragdollDisable());

        }
        else
        {
            ragdoll = false;
            for (var i = 0; i < bodies.Length; i++)
            {
                bodies[i].isKinematic = true;
            }
            Debug.Log(resetPositions);
            children = GetComponentsInChildren<Transform>();
            for (int i =0;i<resetPositions.Count;i++)
            {
                children[i].transform.position = resetPositions[i];
                children[i].transform.rotation = resetRotations[i];

            }
            this.gameObject.GetComponent<CharacterController>().enabled = true;
            this.gameObject.GetComponent<Animator>().enabled = true;
            canRagdoll = true;
        }
    }

    void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.tag == "RagdollCollider" && canRagdoll)
        {
            canRagdoll = false;
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
            toggleRagdoll();
        }
    }

    public IEnumerator ragdollEnable()
    {
        yield return new WaitForSeconds(.5f);
        this.gameObject.GetComponent<CharacterController>().enabled = false;
    
    }

    public IEnumerator ragdollDisable()
    {
        yield return new WaitForSeconds(5);
        this.gameObject.GetComponent<BoxCollider>().enabled = true;
        toggleRagdoll();
    }

    public IEnumerator spawnPowerup(GameObject powerup)
    {
        yield return new WaitForSeconds(0.5f);
        if (powerup.name == "Drink")
        {
            GameObject pu = Instantiate(powerup, transform.position + (transform.up * 1.75f) + transform.forward, transform.rotation);
            Destroy(prop);
            pu.GetComponent<PowerupDrink>().target = runnerInFront.transform;
        } else if (powerup.name == "Egg")
        {
            GameObject pu = Instantiate(powerup, hand.transform.position, transform.rotation);
            Destroy(prop);
        }
        else if (powerup.name == "Pigeon")
        {
            GameObject pu = Instantiate(powerup, transform.position + (transform.up * 1.75f) + transform.forward, transform.rotation);
            Destroy(prop);
            pu.GetComponent<PowerupPigeon>().target = runnerInFront;
        }
        else if (powerup.name == "Cardboard")
        {
            GameObject pu = Instantiate(powerup, transform.position + (transform.up * 1.75f) + transform.forward, transform.rotation);
            Destroy(prop);
        }
    }

    public IEnumerator disableThrow(string animBool)
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool(animBool, false);
    }

    public IEnumerator disableSki()
    {
        yield return new WaitForSeconds(5);
        Destroy(prop);
        Destroy(prop2);
        anim.SetBool("ski", false);
        speedLimit = 10;
    }
}
