using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerScript player = other.gameObject.GetComponent<PlayerScript>();
        if (player != null)
        {
            PlayerScript.currentCheckpointPos = transform.position;
        }
    }
}