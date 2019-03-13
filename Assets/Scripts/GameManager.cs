using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public List<GameObject> players;

	// Use this for initialization
	void Start () {
        players = new List<GameObject>();

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("AI"))
        {
            players.Add(player);
        }
        players.Add(GameObject.FindGameObjectWithTag("Player"));
	}
	
	// Update is called once per frame
	void Update () {
        bool pos_changed = false;
        GameObject old_leader = null;
        GameObject new_leader = null;
        int old_leader_pos = 0;

        for (int i = 0; i < players.Count - 1; i++)
        {
            Vector3 directionToTarget = players[i].transform.position - players[i + 1].transform.position;
            float angle = Vector3.Angle(transform.forward, directionToTarget);
            float distance = Vector3.Distance(players[i].transform.position, players[i + 1].transform.position);

            if (Mathf.Abs(angle) > 90 && distance < 10) {
                old_leader = players[i];
                old_leader_pos = i;
                new_leader = players[i + 1];
                pos_changed = true;
                break;
            }
        }

        if (pos_changed)
        {
            players[old_leader_pos] = new_leader;
            players[old_leader_pos + 1] = old_leader;
        }
	}

    public GameObject getInFront(GameObject player)
    {
        for (int i = 0;i < players.Count; i++)
        {
            if (players[i].name == player.name)
            {
                return players[i-1];
            }
        }
        return null;
    }
}
