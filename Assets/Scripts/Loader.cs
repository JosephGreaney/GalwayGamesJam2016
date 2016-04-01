using UnityEngine;
using System.Collections;

/**
 *  The loader class is used to instantiate GameObjects
 */
public class Loader : MonoBehaviour
{

    public GameObject gameManager;      //GameManager prefab

    /**
     *  Awake is used to instantiate game objects
     */
    void Awake()
    {
        // Check if GameManager has already been instantiated
        if (GameManager.instance == null)
            Instantiate(gameManager);
    }
}
