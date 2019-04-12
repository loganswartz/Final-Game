using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharControlNew : MonoBehaviour {

    // This is the main script that allows for player control

	public Animator anim;
	public Camera cam;
	public CharacterController cc;

    private Vector3 MoveDirection;
    private Vector3 LookDir;
    private Quaternion LookAt;
    public bool moveCam = true;
    public float holdTime;
    public float startTime;

    public bool ragdoll = false;
    public Rigidbody[] bodies;
    private bool canRagdoll = true;

    private Rigidbody rb;
    public float speed = 0.5f;
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

    public AudioSource fall;
    public AudioSource running;
    public AudioSource skiing;

    // Use this for initialization
    void Start () {
        running.volume = 0;
        rb = GetComponent<Rigidbody>();
		cc = GetComponent<CharacterController> ();
		anim = GetComponent<Animator>();
        resetPositions = new List<Vector3>();

        // Find and save the player's hands
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

        // Set player skin + clothing to selected colors
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

        // If ski powerup active, set speed to 18
        if (anim.GetBool("ski"))
        {
            speed = 18;
        }

        // If W held, move forward
        // Animation speed tied to move speed
        // Acceleration and deceleration is exponential, to give the feeling
        // of inertial and momentum.
        if (Input.GetKey(KeyCode.W)) {
            if (running.volume < 1)
            {
                running.volume += 0.1f;
            }
            moveCam = true;
            anim.SetBool("run", true);
            if (speed <= speedLimit) {
                speed *= 1.05f;
            }
            else
            {
                speed /= 1.05f;
            }

        // If S held, slow down (as if applying brakes). This will slow at a
        // faster pace than simply letting go
        } else if (Input.GetKey (KeyCode.S)) {

            anim.SetBool("run", true);
            if (speed >= 0.75f)
            {
                speed /= 1.05f;
            }
        }
        else
        {
            if (running.volume > 0)
            {
                running.volume -= 0.01f;
            }
            if (speed >= 0.75f)
            {
                speed /= 1.005f;
            }
        }

        // If A or D held down, we'll turn the character. We also rotate the
        // camera to always be behind the player, but not immediately -- that
        // would be disorienting. So we wait for the character to turn a certain
        // amount before we start rotating the camera as well
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            startTime = Time.time;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveCam = false;
            if (speed > 0.75 && !ragdoll)
            {
                transform.Rotate(0.0f, (-6 + (speed / 2)), 0.0f);
                holdTime = Time.time;
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveCam = false;
            if (speed > 0.75 && !ragdoll)
            {
                transform.Rotate(0.0f, (6 - (speed / 2)), 0.0f);
                holdTime = Time.time;
            }
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            holdTime = 0;
            startTime = 0;
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


        // Gravity exists
        if (!cc.isGrounded) {
			cc.Move (-transform.up * 10 * Time.deltaTime);
		}

        cc.Move(MoveDirection);


        // If a powerup is held, throw it when pressing space
        if (powerup != "" && Input.GetKey("space"))
        {
            // Set the image on the HUD to the correct powerup
            powerupImage.GetComponent<changePowerImg>().switchImg(0);

            // When we activate a powerup, there is an initial powerup "prop"
            // created, that's parented to the player's hand, during the throw
            // animation. After the animation completes, the actual powerup is
            // created and goes to do its powerup things. For example:
            if (powerup == "drink")
            {
                // Start throw animation
                anim.SetBool("throw", true);
                // Start coroutine that will disable throw animation once it finishes
                StartCoroutine(disableThrow("throw"));

                // Create the powerup prop (uses the same prefab as the actual), 
                // remove its collision, let it know that it's a prop (so that it
                // doesn't execute its script), and let it follow the hand.
                // Finally, find the player in front of us and start the coroutine
                // that will create the actual powerup.
                // This is the process that all powerups use.
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

                skiing.volume = 1;
                prop = Instantiate(skiPowerup, hand.transform.position, transform.rotation);
                prop.GetComponent<PowerupSki>().target = hand;
                prop2 = Instantiate(skiPowerup, hand2.transform.position, transform.rotation);
                prop2.GetComponent<PowerupSki>().target = hand2;
                StartCoroutine(disableSki());

            }
            // After throwing, set powerup to none.
            powerup = "";
        }
		
	}


    // Ragdoll is toggled by disabling animators for every body part, and adding physics to every rigidbody. Disabling is done in the reverse.
    void toggleRagdoll()
    {
        if (!ragdoll)
        {
            ragdoll = true;
            fall.Play();
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

    // Enable a ragdoll .5s after collision, to ensure that momentum isn't
    // immediately lost
    public IEnumerator ragdollEnable()
    {
        yield return new WaitForSeconds(.5f);
        this.gameObject.GetComponent<CharacterController>().enabled = false;
    
    }

    // Disable ragdoll after 5s
    public IEnumerator ragdollDisable()
    {
        yield return new WaitForSeconds(5);
        this.gameObject.GetComponent<BoxCollider>().enabled = true;
        toggleRagdoll();
    }

    // This is the coroutine that spawns the real powerup. Generally, it creates
    // the powerup, destroys the prop, and sets the powerup's target to the runner
    // in front. (Of course, this may change based on behavior of powerup).
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

    // Coroutine to disable throwing animation
    public IEnumerator disableThrow(string animBool)
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool(animBool, false);
    }

    // Coroutine to disable ski powerup
    public IEnumerator disableSki()
    {
        yield return new WaitForSeconds(5);
        Destroy(prop);
        Destroy(prop2);
        anim.SetBool("ski", false);
        skiing.volume = 0;
        speedLimit = 10;
    }
}
