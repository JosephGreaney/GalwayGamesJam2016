using UnityEngine;
using System.Collections;

public class Knight : Enemy {


	// Use this for initialization
	void Start () {
        type = EntityType.KNIGHT;
        maxDistance = 1.6f;
        movespeed = 4f;
        fireRange = 1.2f;
        attackTime = 1.2f;
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
        int dir;
        base.Attack();
        //Create a collider in front and behind of the player
        if (m_FacingRight)
            dir = 1;
        else
            dir = -1;

        Vector2 pointA = new Vector2((transform.position.x + (dir * fireRange)), (transform.position.y - 1));
        Vector2 pointB = new Vector2((transform.position.x), (transform.position.y + 1));
        Collider2D[] colliders = Physics2D.OverlapAreaAll(pointA, pointB);
        //Check if the player has been collided with
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag == "Player")
                Debug.Log("Hit");
        }
    }
}
