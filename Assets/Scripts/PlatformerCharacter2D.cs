using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlatformerCharacter2D : MonoBehaviour
{
    [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] private float m_JumpForce = 600f;                  // Amount of force added when the player jumps.
    [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        
    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.

    private int currZone = 1;           // The current zone that the player is in

    private int keys = 0;

    public float zoneDistance = 500;    // The distance between each zone in the level

    private void Awake()
    {
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        m_Grounded = false;
        // This is used to check if the feet of the player are touching ground.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                m_Grounded = true;
        }

        transform.rotation = Quaternion.Euler(0, 0, 0);

    }

	public void Move(float move, bool jump, int warpDest)
	{
		// Check if the vertical speed will be below the limit
        // Only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
            // Move the character
            m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);
        }

        if (m_Grounded &&jump)
        {
            makeJump();
        }

        //If the player should jump ...
        if (move > 0 && !m_FacingRight)
        {
            // ... flip the player.
            Flip();
                
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (move < 0 && m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }

        if (warpDest != -1)
        {
            warp(warpDest);
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            keys++;
        }
    }

    void makeJump()
    {
        // Add a vertical force to the player.
        m_Grounded = false;
        //m_Anim.SetBool("Ground", false);
        m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
    }

    /**
        *  This function allows the player to warp between time zones
        *  based on the y-axis.
        */
    void warp(int destination)
    {
        int warpTo = destination - currZone;
        // Where the player will be warped to.
        Vector3 targetDest = new Vector3(transform.position.x, (transform.position.y + (warpTo * zoneDistance)), transform.position.z);
        
        // Check if the destination is unoccupied
        if (!checkIfOccupied(targetDest))
        {
            // If unoccupied move to that position
            gameObject.transform.position = new Vector3(transform.position.x, (transform.position.y + (warpTo * zoneDistance)), transform.position.z);
            currZone = destination;
        }
        else
        {
            Debug.Log("Collided");
        }
        
    }

    /**
     *  Check if a space is occupied in the world by any colliders
     */
    bool checkIfOccupied(Vector3 targetDest)
    {
        bool occupied = false;
         Collider2D [] colliders = Physics2D.OverlapCircleAll(targetDest, 0.5f);
        //You have collided with something
        if (colliders.Length > 0)
            occupied = true;

        return occupied;
    }
}
