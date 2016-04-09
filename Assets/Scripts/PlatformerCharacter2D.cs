using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
[RequireComponent(typeof(AudioSource))]

public class PlatformerCharacter2D : Entity
{
    [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] private float m_JumpForce = 600f;                  // Amount of force added when the player jumps.
    [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
    [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
    [SerializeField] private float m_StepInterval;

    //Joe's code start here
    public Projectile projectile;
    public WeaponType weapon;
    public enum WeaponType
    {
        CLUB, SHIELD, GUN
    };

    public GearType gear;
    public enum GearType
    {
        HELMET, JETPACK, NO_GEAR
    }
    //code end here

    private enum AnimationIndex
    {
        CLUB, CLUB_HELMET, CLUB_JETPACK,
        RAYGUN, RAYGUN_HELMET, RAYGUN_JETPACK,
        SHIELD, SHIELD_HELMET, SHIELD_JETPACK
    };
    public float zoneDistance = 500;    // The distance between each zone in the level

    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .1f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    public bool warpCooldown;
    public int cdRemaining;
    public int cd;

    public float step = 0;
    private AudioSource m_AudioSource;

    private Rigidbody2D m_Rigidbody2D;
    public RuntimeAnimatorController[] animatorControllers;
    public Sprite[] standingSprites;
    public Animator animator;
    public SpriteRenderer spriteRend;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.

    private int currZone = 1;           // The current zone that the player is in

    private void Awake()
    {
        weapon = WeaponType.GUN;
        gear = GearType.NO_GEAR;
        // Setting up references.
        animator = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        animatorControllers = Resources.LoadAll<RuntimeAnimatorController>("Graphics/Animations/Controllers");
        standingSprites = Resources.LoadAll<Sprite>("Graphics/Animations/StaticSprites/Standing");
        //a = Resources.
        m_GroundCheck = transform.Find("GroundCheck");
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        type = EntityType.PLAYER;
        m_AudioSource = GetComponent<AudioSource>();
        for (int i = 0; i < standingSprites.Length; i++) {
           // print("animator controllers: " + standingSprites[i]);
        }
        changeAnimation(AnimationIndex.RAYGUN_JETPACK);
    }

    private void changeAnimation(AnimationIndex index)
    {
        animator.runtimeAnimatorController = animatorControllers[(int)index];
        spriteRend.sprite = standingSprites[(int)index];
        print(spriteRend.sprite.name);
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
            //Debug.Log("Player Grounded: " + colliders[i].gameObject);
        }

        transform.rotation = Quaternion.Euler(0, 0, 0);

        if (m_Grounded && moving)
        {
            step++;
            if (step == m_StepInterval)
            {
                PlayFootStepAudio();
                step = 0;
            }
        }
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

    private void PlayFootStepAudio()
    {
        if (!m_Grounded)
        {
            return;
        }
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range(1, 2);
        m_AudioSource.clip = m_FootstepSounds[n];
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        m_FootstepSounds[n] = m_FootstepSounds[0];
        m_FootstepSounds[0] = m_AudioSource.clip;
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
            Vector3 warpLoc = new Vector3(-666, -666, 0);
            // Check if the destination is unoccupied
            if (!CheckIfOccupied(targetDest))
            {
                // If unoccupied move to that position
                gameObject.transform.position = warpLoc;
                warpCooldown = true;
                m_AudioSource.clip = m_FootstepSounds[2];
                m_AudioSource.PlayOneShot(m_AudioSource.clip);
                StartCoroutine("WarpCooldown");
                StartCoroutine(WarpZone(targetDest, destination));
            }
            else
            {
                Debug.Log("Collided");
                m_AudioSource.clip = m_FootstepSounds[3];
                m_AudioSource.PlayOneShot(m_AudioSource.clip);
            }
        }
       
    }

    IEnumerator WarpZone(Vector3 targetDest, int destination)
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.transform.position = targetDest;
        currZone = destination;
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
        if (m_Attack && !attacking)
        {
            int dir;

            if (weapon == WeaponType.CLUB)
            {
                if (m_FacingRight)
                    dir = 1;
                else
                    dir = -1;

                Vector2 pointA = new Vector2((transform.position.x + (dir * 0.5f)), (transform.position.y - 1));
                Vector2 pointB = new Vector2((transform.position.x), (transform.position.y + 1));
                Collider2D[] colliders = Physics2D.OverlapAreaAll(pointA, pointB);
                //Check if the player has been collided with
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject.tag == "Enemy")
                    {
                        float distance = Mathf.Abs(transform.position.x - colliders[i].transform.position.x);

                        if (distance < 2f)
                            colliders[i].gameObject.SendMessage("GetHit");
                    }
                }
            }
            else if (weapon == WeaponType.GUN)
            {
                Vector3 source = transform.Find("ShotSource").position;
                dir = m_FacingRight ? 1 : -1;
                Projectile proj = (Projectile)Instantiate(projectile, source, Quaternion.identity);
                proj.m_RigidBody2D.AddForce(new Vector3((dir * 0.4f), 0, 0));
            }
            base.Attack();
            m_AudioSource.clip = m_FootstepSounds[4];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
        }
    }
}