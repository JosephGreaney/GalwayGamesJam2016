using UnityEngine;
using System.Collections;

public class Caveman : Enemy {

	// Use this for initialization
	void Start () {
        type = EntityType.CAVEMAN;
        maxDistance = 2.5f;
        movespeed = 4f;
        fireRange = 3f;
        attackTime = 1f;
	}
	
	// Update is called once per frame
	new void Update () {
        base.Update();
	    if (PlayerInRange() && !attacking)
        {
            Attack();
        }
	}

    new void Attack()
    {
        base.Attack();
        //Create a collider in front and behind of the player
        Vector2 pointA = new Vector2((transform.position.x - fireRange), (transform.position.y - 1));
        Vector2 pointB = new Vector2((transform.position.x + fireRange), (transform.position.y + 1));
        Collider2D[] colliders = Physics2D.OverlapAreaAll(pointA, pointB);
        //Check if the player has been collided with
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag == "Player")
            {
                player.SendMessage("GetHit");
            }
                
        }
    }
}
