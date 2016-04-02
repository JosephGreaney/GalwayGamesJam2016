using UnityEngine;
using System.Collections;

public class Enemy : Entity {

    private GameObject player;               // player reference
    private bool playerSeen;                 // if the player has been seen or not
    private Rigidbody2D m_RigidBody2D;       // the enemies rigidbody2d
    private bool m_FacingRight;
    private SpriteRenderer m_SpriteRenderer; // Sprite renderer attached to enemy
    
    public float movespeed = 4f;       // the movespeed of the enemy
    public float maxDistance = 1.5f;       // maximum distance that the enemy will stay from the player

	// Use this for initialization
	void Start ()
    {
        m_FacingRight = true;
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
	    player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Search for player
        if (playerSeen)
        {
            //moves the enemy towards the player.
            moveTowards(player.transform.position);
        }
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
    void moveTowards(Vector3 destination)
    {
        float distance = destination.x - transform.position.x;
        float dir = Mathf.Sign(distance);
        float absDist = Mathf.Abs(distance);

        //
        if (dir == 1) //Facing right
            m_SpriteRenderer.flipX = false;
        else if (dir == -1) //Facing left
            m_SpriteRenderer.flipX = true;

        if (distance > maxDistance || distance < -maxDistance)
            transform.position += new Vector3(dir, 0, 0) * Time.deltaTime * movespeed;

    }
}
