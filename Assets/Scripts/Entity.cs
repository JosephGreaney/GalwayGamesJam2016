using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{

    private static int ID;

    private int id;
    public int health;
    public int damage;
    protected Animator anim;
    protected bool moving;
    protected bool falling;
    protected bool rising;
    protected bool attacking;

    int runHash = Animator.StringToHash("RunnerAnimation");
    int standHash = Animator.StringToHash("Standing");
    int attackingHash = Animator.StringToHash("Attacking");

    protected EntityType type;
    public enum EntityType
    {
        PLAYER, KNIGHT, DINOSAUR, CAVEMAN, ROBOT, ALIEN
    };
    // Use this for initialization
    void Start()
    {
        id = ID++;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

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
        yield return new WaitForSeconds(0.4f);
        attacking = false;
        anim.SetBool("attacking", false);
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
}