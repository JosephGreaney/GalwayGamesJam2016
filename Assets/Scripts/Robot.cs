using UnityEngine;
using System.Collections;

public class Robot : Enemy {

	// Use this for initialization
	void Start () {
        type = EntityType.ROBOT;
        maxDistance = 6f;
        movespeed = 1f;
        fireRange = 20f;
        attacking = false;
    }
	
	// Update is called once per frame
	new void Update () {
        //Search for player
        if (playerSeen)
        {
            //moves the enemy towards the player.
            moveTowards(player.transform.position);
            moveOnY(player.transform.position);
        }

        if (PlayerInRange() && !attacking)
        {
            //Attack();
        }

    }

    new void Attack()
    {

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
