using UnityEngine;
using System.Collections;

public class Robot : Enemy {

    public Projectile projectile;

	// Use this for initialization
	void Start () {
        type = EntityType.ROBOT;
        maxDistance = 6f;
        movespeed = 1f;
        fireRange = 20f;
        attackTime = 1f;
        health = 1;
    }
	
	// Update is called once per frame
	new public void Update () {
        //Search for player
        base.Update();
        if (playerSeen)
        {
            //moves the enemy towards the player.
            moveTowards(player.transform.position);
            moveOnY(player.transform.position);

            if (PlayerInRange() && !attacking)
            {
                Attack();
            }
        }

        

    }

    new void Attack()
    {
        Vector3 source = transform.Find("ProjectileSource").position;
        int dir = m_FacingRight ? 1 : -1;
        Projectile proj = (Projectile)Instantiate(projectile, source, Quaternion.identity);
        proj.m_RigidBody2D.AddForce(new Vector3((dir * 0.4f), 0, 0));
        base.Attack();
    }

    void moveOnY(Vector3 destination)
    {
        // get the distance to the destination on the y-axis
        float yDist = (destination.y + 0.6f) - transform.position.y;
        //get the direction as either 1 or -1
        float dir = Mathf.Sign(yDist);

        if (yDist > 0.1 || yDist < -0.1)
            transform.position += new Vector3(0, dir, 0) * Time.deltaTime * movespeed;
    }
}
