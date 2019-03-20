using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupPigeon : MonoBehaviour {

    public GameObject target;
    private bool shrink;
    public bool prop;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (!prop)
        {
            if (target)
            {
                if (target.name != "Head")
                {
                    Transform[] children = target.GetComponentsInChildren<Transform>();
                    foreach (var child in children)
                    {
                        if (child.name == "Head")
                        {
                            target = child.gameObject;
                        }
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 12 * Time.deltaTime);
                    transform.rotation = Quaternion.LookRotation(transform.position - target.transform.position);
                }

            }

            if (shrink && transform.localScale.x > 0)
            {
                transform.localScale = transform.localScale - new Vector3(0.001f, 0.001f, 0.001f);
            }
            if (transform.localScale.x < 0.02f)
            {
                Destroy(this.gameObject);
            }
        } else
        {
            transform.position = target.transform.position;
        }

	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "AI")
        {
            StartCoroutine(enableShrink());
        }
    }

    public IEnumerator enableShrink()
    {
        yield return new WaitForSeconds(2);
        shrink = true;
    }

}
