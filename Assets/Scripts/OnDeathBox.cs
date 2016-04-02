using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * This class is used to handle the collision detection for boxes that kill the player
 * when the player collides with this script's trigger box then the scene is restarted
*/
public class OnDeathBox : MonoBehaviour {

    // If the player enters with the trigger box then the player is killed
	void OnTriggerEnter2D(Collider2D coll) {
        
		if (coll.gameObject.tag == "Player") {
            //Kill the player
            GameManager.instance.PlayerDeath();
        }
	}
}
