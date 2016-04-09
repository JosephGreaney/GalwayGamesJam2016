using UnityEngine;
using System.Collections;

public class Enemy : Entity {

    private Rigidbody2D m_RigidBody2D;       // the enemies rigidbody2d
    protected GameObject player;               // player reference
    protected bool playerSeen;                 // if the player has been seen or not
    protected bool m_FacingRight;
    protected float distance;                // the distance to the player

    public float fireRange = 2f;
    public float movespeed = 4f;             // the movespeed of the enemy
    public float maxDistance = 2f;           // maximum distance that the enemy will stay from the player

	// Use this for initialization
	void Awake ()
    {
        m_FacingRight = true;
	    player = GameObject.FindGameObjectWithTag("Player");
        distance = fireRange * 2;
    }
	
	// Update is called once per frame
	new public void Update ()
    {
        base.Update();
        //Search for player
        if (playerSeen)
        {
            //moves the enemy towards the player.
            moveTowards(player.transform.position);
        }
	}


    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            playerSeen = true;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            playerSeen = false;
        }
    }

    /**
     *  Move the enemy towards a given Vector3 destination
     */
    protected void moveTowards(Vector3 destination)
    {
        distance = destination.x - transform.position.x;
        float dir = Mathf.Sign(distance);

        if (dir == 1) //Facing right
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            m_FacingRight = true;
        }
        else if (dir == -1) //Facing left
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            m_FacingRight = false;
        }

        if (distance > maxDistance || distance < -maxDistance)
            transform.position += new Vector3(dir, 0, 0) * Time.deltaTime * movespeed;

    }


    protected bool PlayerInRange()
    {
        distance = player.transform.position.x - transform.position.x;
        return distance <= fireRange ? true : false;
    }
}
