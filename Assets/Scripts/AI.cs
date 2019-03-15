using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {

    public bool ragdoll = false;
    public Rigidbody[] bodies;
    public Transform[] children;
    public List<Vector3> resetPositions;
    public List<Quaternion> resetRotations;

    // Use this for initialization
    void Start () {
        resetPositions = new List<Vector3>();
        bodies = GetComponentsInChildren<Rigidbody>();
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            resetPositions.Add(child.transform.position);
        }
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            resetRotations.Add(child.transform.rotation);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
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
            for (int i = 0; i < resetPositions.Count; i++)
            {
                children[i].transform.position = resetPositions[i];
                children[i].transform.rotation = resetRotations[i];

            }
            this.gameObject.GetComponent<Animator>().enabled = true;
        }
    }

    void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.tag == "RagdollCollider")
        {
            toggleRagdoll();
        }
    }

    public IEnumerator ragdollEnable()
    {
        yield return new WaitForSeconds(.5f);
    }

    public IEnumerator ragdollDisable()
    {
        yield return new WaitForSeconds(5);
        toggleRagdoll();
    }
}
