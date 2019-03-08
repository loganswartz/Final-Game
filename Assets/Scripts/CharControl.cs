using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharControl : MonoBehaviour {

	public Animator anim;
	public Camera cam;
	public CharacterController cc;
	private Vector3 MoveDirection;
	private Quaternion LookAt;
    public bool ragdoll = false;
    public Rigidbody[] bodies;

    public Transform[] children;
    public List<Transform> resetPositions;

	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController> ();
		anim = GetComponent<Animator>();
        resetPositions = new List<Transform>();
        bodies = GetComponentsInChildren<Rigidbody>();
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            resetPositions.Add(child.transform);
        }
    }

    // Update is called once per frame
    void Update () {

		if (Input.GetKey (KeyCode.W)) {
			anim.SetBool ("run", true);
			if (Input.GetKey (KeyCode.A)) {
				MoveDirection = (cam.transform.forward - cam.transform.right) * 4 * Time.deltaTime;
			} else if (Input.GetKey (KeyCode.D)) {
				MoveDirection = (cam.transform.forward + cam.transform.right) * 4 * Time.deltaTime;
			} else {
				MoveDirection = cam.transform.forward * 4 * Time.deltaTime;
			}
		} else if (Input.GetKey (KeyCode.S)) {
			anim.SetBool ("run", true);
			if (Input.GetKey (KeyCode.A)) {
				MoveDirection = (-cam.transform.forward - cam.transform.right) * 4 * Time.deltaTime;
			} else if (Input.GetKey (KeyCode.D)) {
				MoveDirection = (-cam.transform.forward + cam.transform.right) * 4 * Time.deltaTime;
			} else {
				MoveDirection = -cam.transform.forward * 4 * Time.deltaTime;
			}
		} else if (Input.GetKey (KeyCode.A)) {
			MoveDirection = -cam.transform.right * 4 * Time.deltaTime;
			anim.SetBool ("run", true);
		} else if (Input.GetKey (KeyCode.D)) {
			MoveDirection = cam.transform.right * 4 * Time.deltaTime;
			anim.SetBool ("run", true);
		} else {
			MoveDirection = Vector3.zero;
			anim.SetBool ("run", false);
		}

		if (!cc.isGrounded) {
			cc.Move (-transform.up * 10 * Time.deltaTime);
		}
		cc.Move (MoveDirection);

		if (MoveDirection != Vector3.zero) {
			LookAt = Quaternion.LookRotation (MoveDirection);
		}

		Quaternion LookRotationLimit = Quaternion.Euler (transform.rotation.eulerAngles.x, LookAt.eulerAngles.y, transform.rotation.eulerAngles.z);
		transform.rotation = Quaternion.Slerp (transform.rotation, LookRotationLimit, 0.2f);
		
	}

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
                children[i].transform.localPosition = resetPositions[i].localPosition;
                children[i].transform.localRotation = resetPositions[i].localRotation;
                children[i].transform.localScale = resetPositions[i].localScale;
                children[i].transform.position = resetPositions[i].position;
                children[i].transform.rotation = resetPositions[i].rotation;
                if (children[i].gameObject.name == "MidTorso")
                {
                    Debug.Log(resetPositions[i].position);
                    Debug.Log(children[i].position);
                }

            }
            this.gameObject.GetComponent<CharacterController>().enabled = true;
            this.gameObject.GetComponent<Animator>().enabled = true;
        }
    }

    void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.tag == "RagdollCollider")
        {
            Debug.Break();
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
        toggleRagdoll();
    }
}
