    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollisions : MonoBehaviour
{
    GameManager gameManager;
    PlayerController playerController;
    AudioSource ballRollAudio;
    AudioSource pinHitAudio;

    bool hasHitPins;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerController = FindObjectOfType<PlayerController>();
        ballRollAudio = GetComponent<AudioSource>();

        hasHitPins = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pin") && !hasHitPins)
        {
            gameManager.soundManager.PlaySound("collide");
            hasHitPins = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Pit Trigger")
        {
            playerController.throwCompleted = true;

            gameManager.soundManager.PlaySound("ballInPit");
        }
    }
}
