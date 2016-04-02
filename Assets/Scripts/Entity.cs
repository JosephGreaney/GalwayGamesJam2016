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

    protected void Move(bool grounded, bool jump)
    {

        anim.SetBool("moving", false);
        if (moving && !jump && grounded)
        {
            // Debug.Log(type + " ID: " + id + " Moving");
            anim.SetBool("moving", moving);
        }
        else if(jump && grounded)
        {
            
            anim.SetBool("jumping", true);
            // anim.SetTrigger(standHash);
            //Debug.Log("Animation stopping");
        }else if(!jump && grounded)
        {
            anim.SetBool("jumping", false);
        }
    }
}