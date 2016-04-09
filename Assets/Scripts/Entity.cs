using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{

    private static int ID;

    private int id;
    public int health = 1;
    public int damage = 1;
    public float attackTime = 0.4f;

    protected Animator anim;
    protected bool moving;
    protected bool falling;
    protected bool rising;
    protected bool attacking;
    protected bool gettingHit;

    int runHash = Animator.StringToHash("RunnerAnimation");
    int standHash = Animator.StringToHash("Standing");
    int attackingHash = Animator.StringToHash("Attacking");

    private GameObject player;

    protected EntityType type;
    public enum EntityType
    {
        PLAYER, KNIGHT, DINOSAUR, CAVEMAN, ROBOT, ALIEN
    };
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        id = ID++;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected void Update()
    {
        //Check if health is less than 0 or 0
        if (health <= 0)
        {
            if (type == EntityType.PLAYER)
            {
                //Kill player
                Debug.Log("Loaded");
                GameManager.instance.PlayerDeath();
            }
            else
            { 
                /*if (type == EntityType.CAVEMAN)
                {
                    player.SendMessage("ChangeWeapon", 1);
                }
                else if (type == EntityType.KNIGHT)
                {
                    player.SendMessage("ChangeWeapon", 2);
                    player.SendMessage("ChangeGear", "HELMET");
                }
                else if(type == EntityType.ROBOT)
                {
                    player.SendMessage("ChangeWeapon", 3);
                    player.SendMessage("ChangeGear", "JETPACK");
                }*/
                //Kill enemy

                //Show death animation
                //Destroy this gameObject
                Destroy(gameObject);
            }
        }
    }

    protected void Attack()
    {
        if (!attacking)
        {
            print("attacking true");
            attacking = true;
            anim.SetBool("attacking", true);
            print(type + " ID: " + id + " attacked\n");
            StartCoroutine("AttackCooldown");
        }
        
    }

    IEnumerator AttackCooldown()
    {
        print("attack false");
        yield return new WaitForSeconds(attackTime);
        attacking = false;
        anim.SetBool("attacking", false);
    }

    IEnumerator HitCooldown()
    {
        //wait 0.5 seconds before allowing to be hit again
        yield return new WaitForSeconds(0.5f);
        gettingHit = false;
    }

    protected void Move(bool grounded, bool jump, Rigidbody2D rigid)
    {
        
        if (rigid.velocity.y == 0)
        {
            rising = false;
            falling = false;
            
        }else if (rigid.velocity.y < 0)
        {
            rising = false;
            falling = true;
        }else
        {
            falling = false;
            rising = true;
        }
        
        anim.SetBool("moving", moving);
        if (!grounded)
        {
            //Debug.Log("not grounded: \n" + rising);

            if (falling)
            {
                anim.SetBool("falling", true);
                anim.SetBool("rising", false);
            }else if(rising)
            {
                anim.SetBool("falling", false);
                anim.SetBool("rising", true);
            }


        }else
        {
            anim.SetBool("falling", false);
            anim.SetBool("rising", false);
        }
    }

    public void GetHit()
    {
        if (health != 0 && !gettingHit)
        {
            //anim.SetBool("hit", true);
            gettingHit = true;
            StartCoroutine("HitCooldown");
            health--;
        }
    }
}