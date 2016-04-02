using UnityEngine;
using System.Collections;

public class Caveman : Enemy {

    private bool attacking;

	// Use this for initialization
	void Start () {
        type = EntityType.CAVEMAN;
        maxDistance = 2f;
        movespeed = 4f;
        fireRange = 2.2f;
        attacking = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if (PlayerInRange() && !attacking)
        {
            Attack();
        }
	}

    new void Attack()
    {
        base.Attack();
    }
}
