using UnityEngine;
using System.Collections;

public class FollowPath : MonoBehaviour{
    
    public Path PathToFollow;
    
    public int CurrentNode = 0;
    public float speed;
    private float Distance = 1.0f;
    public float rotationSpeed = 3.0f;
    public string whichPath;
    
    Vector3 current_position;
    
    void Start()
    {
        PathToFollow = GameObject.Find (whichPath).GetComponent<Path>();
    }
    
    void Update()
    {
        float distance = Vector3.Distance (PathToFollow.path_elements [CurrentNode].position, transform.position);
        transform.position = Vector3.MoveTowards (transform.position, PathToFollow.path_elements [CurrentNode].position, Time.deltaTime * speed);
        
        var rotation = Quaternion.LookRotation (PathToFollow.path_elements [CurrentNode].position - transform.position);
        transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        
        if (distance <= Distance)
        {
            CurrentNode++;
        }
        
        if (CurrentNode >= PathToFollow.path_elements.Count)
        {
            CurrentNode = 0;
        }
    }
}