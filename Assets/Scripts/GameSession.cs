using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Diagnostics.CodeAnalysis;

public class GameSession : MonoBehaviour
{
    public static GameSession instance;
    [SerializeField] int playersLives = 3;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    int score = 0;

    Vector3 lastCheckpointPosition;

    void Awake()
    {
        // Singleton pattern (works but not the most standard way to do it)
        // int numGameSessions = FindObjectsOfType<GameSession>().Length;
        // if (numGameSessions > 1)
        // {
        //     Destroy(gameObject);  //To avoid having more than one GameSession
        // }
        // else
        // {
        //     DontDestroyOnLoad(gameObject); //To avoid destroying the GameSession when loading a new scene
        // }

        // Singleton pattern (the most standard way to do it)
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);  //To avoid having more than one GameSession
        }
        lastCheckpointPosition = FindObjectOfType<PlayerMovement>().transform.position;  // Set the initial checkpoint position
    }

    void Start()
    {
        livesText.text = playersLives.ToString();
        scoreText.text = score.ToString();
    }
    public void ProcessPlayerDeath()
    {
        if (playersLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();  // Reset the game session when the player runs out of lives
        }
    }

    void TakeLife()
    {
        playersLives--;
        livesText.text = playersLives.ToString();  // Update the lives text
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Restart the current scene and respawn the player at the last checkpoint
        // SceneManager.LoadScene(currentSceneIndex);
        FindObjectOfType<PlayerMovement>().Respawn(lastCheckpointPosition);  
    }

    void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();  // Reset the ScenePersist
        SceneManager.LoadScene(0); // Load the first scene
        Destroy(gameObject);
    }

    public void AddToScore(int points)
    {
        score += points;
        scoreText.text = score.ToString();  // Update the score text
    }

    public void UpdateCheckpoint(Vector3 NewCheckPointPosition)
    {
        // Debug.Log("Last checkpoint: " + lastCheckpointPosition);
        lastCheckpointPosition = NewCheckPointPosition;
        // Debug.Log("New checkpoint: " + lastCheckpointPosition);
    }
}
