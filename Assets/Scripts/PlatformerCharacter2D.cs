using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlatformerCharacter2D : Entity
{
    [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] private float m_JumpForce = 600f;                  // Amount of force added when the player jumps.
    [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

    public float zoneDistance = 500;    // The distance between each zone in the level

    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .1f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    private bool m_Attacking;
    public bool warpCooldown;
    public int cdRemaining;
    public const int cd = 3;
    private Rigidbody2D m_Rigidbody2D;
    public RuntimeAnimatorController[] animator;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.

    private int currZone = 1;           // The current zone that the player is in

    private void Awake()
    {
        // Setting up references.
       // animator = GetComponent<Animator>();
        m_GroundCheck = transform.Find("GroundCheck");
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        type = EntityType.PLAYER;
        //animator.runtimeAnimatorController;
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
            Debug.Log("Player Grounded: " + colliders[i].gameObject);
        }

        transform.rotation = Quaternion.Euler(0, 0, 0);

    }

    public void Move(float move, bool jump, int warpDest)
    {
        // Check if the vertical speed will be below the limit
        // Only control the player if grounded or airControl is turned on
        moving = (Mathf.Abs(move) < 0.5f) ? false : true;
        base.Move(m_Grounded, jump, m_Rigidbody2D);
        //Debug.Log("move value: " + move);
        if (m_Grounded || m_AirControl)
        {
            // Move the character
            m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);
        }

        if (m_Grounded && jump)
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

        // If a warp destination has been set
        if (warpDest != -1)
            Warp(warpDest);
    }

    /**
     *  This function allows the player to warp between time zones
     *  based on the y-axis
     */
    void Warp(int destination)
    {
        if (!warpCooldown)
        {

            int warpTo = destination - currZone;
            // Where the player will be warped to.
            Vector3 targetDest = new Vector3(transform.position.x, (transform.position.y + (warpTo * zoneDistance)), transform.position.z);

            // Check if the destination is unoccupied
            if (!CheckIfOccupied(targetDest))
            {
                // If unoccupied move to that position
                gameObject.transform.position = targetDest;
                currZone = destination;
                warpCooldown = true;
                StartCoroutine("WarpCooldown");
            }
            else
            {
                Debug.Log("Collided");
            }
        }
       
    }

    IEnumerator WarpCooldown()
    {
        cdRemaining = cd;
        // suspend execution for 5 seconds
        for (int i = 1; i <= cd; i++)
        {
            yield return new WaitForSeconds(1);
            cdRemaining = cd - i;
        }
        warpCooldown = false;
    }

    /**
     *  Check if a space in the world is occupied by any colliders
     */
    bool CheckIfOccupied(Vector3 targetDest)
    {
        bool occupied = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(targetDest, 0.5f);
        // If you have collided with something
        if (colliders.Length > 0)
            occupied = true;
        return occupied;
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
    }

    void makeJump()
    {
        // Add a vertical force to the player.
        m_Grounded = false;
        //m_Anim.SetBool("Ground", false);
        m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
    }

    public void Attack(bool m_Attack)
    {
        if(m_Attack && !m_Attacking)
            base.Attack();
    }
}