using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    GameSession gameSession;
    // AudioSource audioSource; // Doesn't work because the object is destroyed before the sound is played
    [SerializeField] AudioClip pickupSFX;
    bool isCollected = false;

    void Awake()
    {
        gameSession = FindObjectOfType<GameSession>();
        // audioSource = GetComponent<AudioSource>(); // Doesn't work because the object is destroyed before the sound is played
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !isCollected)
        {
            isCollected = true;  // Avoid double counting
            // audioSource.Play(); // Doesn't work because the object is destroyed before the sound is played
            AudioSource.PlayClipAtPoint(pickupSFX, Camera.main.transform.position); // This works
            gameSession.AddToScore(1);
            gameObject.SetActive(false);  // Deactivate the game object to avoid double counting
            Destroy(gameObject);
        }
    }
}
