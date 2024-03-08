using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    public static ScenePersist instance;
    void Awake()
    {
        // Singleton pattern (works but not the most standard way to do it)
        // int numScenePersists = FindObjectsOfType<ScenePersist>().Length;
        // if (numScenePersists > 1)
        // {
        //     Destroy(gameObject);
        // }
        // else
        // {
        //     DontDestroyOnLoad(gameObject);
        // }

        // Singleton pattern (the most standard way to do it)
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ResetScenePersist()
    {
        Destroy(gameObject);
    }
}
