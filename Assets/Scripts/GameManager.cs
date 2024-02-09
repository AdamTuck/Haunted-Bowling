using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerController playerController;
    public PinManager pinManager;
    public BowlingScoring bowlingScorer;

    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(instance);
            return;
        }

        instance = this;
    }

    void Start()
    {
        StartGame();
    }

    public void StartGame ()
    {
        playerController.resetThrow();
    }

    public void ResetGame ()
    {
        bowlingScorer.resetScoring();
        pinManager.resetPins();
    }
}
