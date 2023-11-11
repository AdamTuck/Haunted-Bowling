using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource ballSound, uiSound, pinSound;
    [SerializeField] private AudioClip ballThrowClip, ballRollingClip, pinCollisionClip, spareClip, strikeClip, ballInPit;

    public void PlaySound (string soundName)
    {
        switch (soundName) {
            case "ballThrow":
                ballSound.PlayOneShot(ballThrowClip);
                break;
            case "ballRolling":
                ballSound.loop = true;
                ballSound.clip = ballRollingClip;
                ballSound.Play();
                break;
            case "collide":
                ballSound.loop = false;
                ballSound.Stop();
                pinSound.PlayOneShot(pinCollisionClip);
                break;
            case "ballInPit":
                ballSound.loop = false;
                ballSound.Stop();
                ballSound.PlayOneShot(ballInPit);
                break;
            case "strike":
                uiSound.PlayOneShot(strikeClip);
                break;
            case "spare":
                uiSound.PlayOneShot(spareClip);
                break;
        }
    }
}
