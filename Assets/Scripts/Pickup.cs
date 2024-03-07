using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    GameSession gameSession;
    void Awake()
    {
        gameSession = FindObjectOfType<GameSession>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            gameSession.AddToScore(1);
        }
    }
}
