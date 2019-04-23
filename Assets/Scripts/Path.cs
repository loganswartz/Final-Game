using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class Path : MonoBehaviour
{
    public Color color = Color.red;
    public List<Transform> path_elements = new List<Transform> ();
    Transform[] Nodes;
 
    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Nodes = GetComponentsInChildren<Transform>();
        path_elements.Clear();
        
        foreach (Transform element in Nodes)
        {
            if (element != this.transform)
            {
                path_elements.Add(element);
            }
        }
        for (int i = 0; i < path_elements.Count; i++)
        {
            Vector3 position = path_elements[i].position;
            if (i > 0)
            {
                Vector3 previous = path_elements[i - 1].position;                    
                Gizmos.DrawLine(previous, position);  
                Gizmos.DrawWireSphere(position, 0.3f);
            }
        }
    }
 
 
}
 
 