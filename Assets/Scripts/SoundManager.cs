using UnityEngine;
using UnityEngine.SceneManagement;

/**
 *  The SoundManager class manages the sounds of the game
 */
public class SoundManager : MonoBehaviour {

    public static SoundManager instance = null;  // static instance of SoundManager which allows it to be a singleton
    public AudioSource musicSource;
    public AudioSource effectSource;
    
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

    /**
     *  
     */
    public void PlayEffect(AudioClip clip)
    {
        effectSource.clip = clip;

        //Play the clip.
        effectSource.Play();
    }
}
