using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingScoring : MonoBehaviour
{
    PinManager pinManager;

    public int[,] gameScore = new int[11,2];
    public int currentFrame;
    public int currentThrow;
    public GameObject scoreSheetContainer;
    public bool scoreSheetShowing;
    private float scoreSheetTimer;
    public float scoreSheetTimeout;

    // Start is called before the first frame update
    void Start()
    {
        pinManager = GetComponent<PinManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreSheetShowing)
        {
            scoreSheetTimer += Time.deltaTime;

            if (scoreSheetTimer >= scoreSheetTimeout)
            {
                hideScoreSheet();
            }
        }
    }

    void checkSpecialScoring (int frame)
    {

        if (frame >= 3 && gameScore[frame, 0] == 10 && gameScore[frame-1, 0] == 10 && gameScore[frame-2, 0] == 10)
        {
            // TURKEY
            Debug.Log("TURKEY");
        }
        else if (frame >= 2 && gameScore[frame, 0] == 10 && gameScore[frame - 1, 0] == 10)
        {
            // DOUBLE
            Debug.Log("DOUBLE");
        }
        else if (gameScore[frame, 0] == 10)
        {
            // STEERIKE
            Debug.Log("STEERIKE");
        }
        else if (gameScore[frame, 0] + gameScore[frame, 1] == 10)
        {
            // SPAAAARE
            Debug.Log("Spare!!");
        }
    }

    public void advanceScore ()
    {
        if (currentThrow == 1)
        {
            currentThrow += 1;
            pinManager.hideKnockedPins();
        }
        else if (currentFrame < 10 && currentThrow == 2)
        {
            currentThrow = 1;
            currentFrame += 1;

            pinManager.resetPins();
        }

        showScoreSheet();
    }

    public void showScoreSheet ()
    {
        LeanTween.moveLocalY(scoreSheetContainer, -445.8f, 0.5f);
        scoreSheetShowing = true;
    }

    public void hideScoreSheet ()
    {
        LeanTween.moveLocalY(scoreSheetContainer, -645, 0.5f);
        scoreSheetShowing = false;
        scoreSheetTimer = 0;
    }
}