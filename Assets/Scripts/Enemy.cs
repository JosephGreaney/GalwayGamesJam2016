using UnityEngine;
using System.Collections;

public class Enemy : Entity {

    private GameObject player;          // player reference
    private bool playerSeen;            // if the player has been seen or not
    private Rigidbody2D m_RigidBody2D;  // the enemies rigidbody2d
    
    public float movespeed = 10f;       // the movespeed of the enemy

	// Use this for initialization
	void Start ()
    {
	    player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Search for player
        if (playerSeen)
        {
            moveTowards(player.transform.position);
        }
        //if player is seen

        //  move towards player

        //  if player is in hitrange
        //      attack
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            playerSeen = true;
        }
    }

    void moveTowards(Vector3 destination)
    {
        Vector3 dir = destination - transform.position;

    }
}
