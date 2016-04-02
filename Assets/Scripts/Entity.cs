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

    int runHash = Animator.StringToHash("RunnerAnimation");
    int standHash = Animator.StringToHash("Standing");

    protected EntityType type;
    public enum EntityType
    {
        PLAYER, WIZARD, DINOSAUR, CAVEMAN, ROBOT, ALIEN
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

        print(type + " ID: " + id + " attacked\n");
    }

    protected void Move(bool grounded, bool jump, Rigidbody2D rigid)
    {
        
        if (rigid.velocity.y == 0)
        {
            print(rigid.velocity.y);
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