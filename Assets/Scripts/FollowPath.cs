using UnityEngine;
using System.Collections;

public class FollowPath : MonoBehaviour{
    
    public Path PathToFollow;
    
    public int CurrentNode = 0;
    public float speed;
    private float Distance = 1.0f;
    public float rotationSpeed = 3.0f;
    public string whichPath;
    private AI AIScript;
    private bool start = false;
    
    Vector3 current_position;
    
    void Start()
    {
        AIScript = GetComponent<AI>();
        PathToFollow = GameObject.Find (whichPath).GetComponent<Path>();
        StartCoroutine(beginWait());
    }
    
    void Update()
    {

        if (!AIScript.ragdoll && start)
        {
            float distance = Vector3.Distance(PathToFollow.path_elements[CurrentNode].position, transform.position);
            transform.position = Vector3.MoveTowards(transform.position, PathToFollow.path_elements[CurrentNode].position, Time.deltaTime * speed);

            var rotation = Quaternion.LookRotation(PathToFollow.path_elements[CurrentNode].position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

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

    public IEnumerator beginWait()
    {
        yield return new WaitForSeconds(3);
        start = true;
    }
}