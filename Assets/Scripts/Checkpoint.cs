using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    bool isTriggered = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !isTriggered)
        {
            GameSession.instance.UpdateCheckpoint(transform.position);
            isTriggered = true;
        }
    }
}
