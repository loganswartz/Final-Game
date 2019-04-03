using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour {

    // This is sort of an overarching game controller script, that manages
    // multiple systems.

    // This script manages player positions by initializing an ordered array
    // of players at the start, and then each frame checking if, for each pair,
    // if the player that is supposed to be behind is still behind. If not, it
    // switches the pair. This only happens if the players are within a certain
    // distance, however, as the check is relative to the first player, and
    // we don't want it to get messed up if the track is a circle, for example

    public List<GameObject> players;

    public GameObject HUD;
    public GameObject Menu;

	// Use this for initialization
	void Start () {
        players = new List<GameObject>();

        // Find all players and add to queue in order of initial position
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

        // Every frame, check every pair of players in the queue. If the positions
        // have swapped, let the queue reflect that
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

        // If esc key pressed, bring up pause menu.
        if (Input.GetKey(KeyCode.Escape))
        {
            HUD.SetActive(false);
            Menu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    // Given a player, return the player in front of them according to queue
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

    // Load the main menu
    public void toMenu()
    {
        SceneManager.LoadScene("Scenes/MenuScene");
    }

    public void resume()
    {
        Time.timeScale = 1;
    }
}
