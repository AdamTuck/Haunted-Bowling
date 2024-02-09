using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollisions : MonoBehaviour
{
    PlayerController playerController;
    AudioSource ballRollAudio;
    AudioSource pinHitAudio;

    private void Start()
    {
        playerController = GameManager.instance.playerController;
        ballRollAudio = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pin"))
        {
            //Debug.Log("Hit Pin: " + collision.gameObject.name);
            pinHitAudio = collision.gameObject.GetComponent<AudioSource>();
            pinHitAudio.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Pit Trigger")
        {
            playerController.throwCompleted = true;

            ballRollAudio.Stop();
        }
    }
}
