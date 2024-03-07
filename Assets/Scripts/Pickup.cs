using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    GameSession gameSession;
    // AudioSource audioSource; // Doesn't work because the object is destroyed before the sound is played
    [SerializeField] AudioClip pickupSFX;
    void Awake()
    {
        gameSession = FindObjectOfType<GameSession>();
        // audioSource = GetComponent<AudioSource>(); // Doesn't work because the object is destroyed before the sound is played
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // audioSource.Play(); // Doesn't work because the object is destroyed before the sound is played
            AudioSource.PlayClipAtPoint(pickupSFX, Camera.main.transform.position); // This works
            Destroy(gameObject);
            gameSession.AddToScore(1);
        }
    }
}
