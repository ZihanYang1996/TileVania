using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(LoadNextScene());
        
        }
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(levelLoadDelay);
        
        // Get the current scene index (int)
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Check if the current scene is the last scene in the build settings
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        // Load the next scene (which is a copy of the current scene)
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(nextSceneIndex);        
    }
}
