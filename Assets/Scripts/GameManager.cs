using UnityEngine;
using UnityEngine.SceneManagement;

/**
 *  The GameManager class manages GameObjects
 */
public class GameManager : MonoBehaviour {

    public static GameManager instance = null;  // static instance of GameManager which allows it to be a singleton

    /**
     *  Awake is called before Start functions
     */
    void Awake ()
    {
        //Check if instance already exists
        if (instance == null)
            instance = this;        //If not set instance to this
        
        else if (instance != this)
            Destroy(gameObject);    //Otherwise destroy this

        //Sets this object to not be destroyed when scene is reloaded
        DontDestroyOnLoad(gameObject);
	}
}
