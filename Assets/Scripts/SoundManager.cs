using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource ballSound, uiSound, pinSound;
    [SerializeField] private AudioClip ballThrowClip, ballRollingClip, pinCollisionClip, spareClip, strikeClip;

    public void PlaySound (string soundName)
    {
        switch (soundName) {
            case "ballThrow":
                ballSound.Play();
            break;
            case "pinSound":
                pinSound.Play();
            break;
            case "ballRolling":
                ballSound.loop = true;
                ballSound.clip = ballRollingClip;
                ballSound.Play();
            break;
        }
    }
}
