using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    PlayerController playerController;
    PinManager pinManager;
    BowlingScoring bowlingScorer;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        pinManager = FindObjectOfType<PinManager>();
        bowlingScorer = FindObjectOfType<BowlingScoring>();

        StartGame();
        playerController.resetThrow();
    }

    private void Update()
    {
        
    }

    public void StartGame ()
    {

    }

    public void SetNextThrow ()
    {

    }
}
