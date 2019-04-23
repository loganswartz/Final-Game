using UnityEngine;
using System.Collections;

public class GetPath : MonoBehaviour{
    
    public GameObject[] Paths;
    
    void Start ()
    {
        int num = Random.Range (0, Paths.Length);
        transform.position = Paths [num].transform.position;
        FollowPath yourPath = GetComponent<FollowPath> ();
        yourPath.whichPath = Paths [num].name;
    }
}
    