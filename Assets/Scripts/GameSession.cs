using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playersLives = 3;
    int score = 0;
    void Awake()
    {
        // Singleton pattern
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);  //To avoid having more than one GameSession
        }
        else
        {
            DontDestroyOnLoad(gameObject); //To avoid destroying the GameSession when loading a new scene
        }
    }

    public void ProcessPlayerDeath()
    {
        if (playersLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    void TakeLife()
    {
        playersLives--;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex); // Reload the current scene (restart the level
    }

    void ResetGameSession()
    {
        SceneManager.LoadScene(0); // Load the first scene
        Destroy(gameObject);
    }

    public void AddToScore(int points)
    {
        score += points;
    }
}
