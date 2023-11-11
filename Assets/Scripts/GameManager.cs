using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    PlayerController playerController;
    PinManager pinManager;
    BowlingScoring bowlingScorer;
    public SoundManager soundManager;

    [SerializeField] private GameObject mobileInputCanvas;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        pinManager = FindObjectOfType<PinManager>();
        bowlingScorer = FindObjectOfType<BowlingScoring>();
        soundManager = FindObjectOfType<SoundManager>();

#if UNITY_ANDROID || UNITY_IOS
        mobileInputCanvas.SetActive(true);
#else
        mobileInputCanvas.SetActive(false);
#endif

        StartGame();
    }

    private void Update()
    {
        
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
