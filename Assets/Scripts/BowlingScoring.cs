using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BowlingScoring : MonoBehaviour
{
    PinManager pinManager;

    [SerializeField] private TextMeshProUGUI[] roll1Texts;
    [SerializeField] private TextMeshProUGUI[] roll2Texts;
    [SerializeField] private TextMeshProUGUI roll3Text;
    [SerializeField] private TextMeshProUGUI[] frameTotalsText;
    [SerializeField] private TextMeshProUGUI overallScoreText;

    [SerializeField] private TextMeshProUGUI[] roll1TextsScreen;
    [SerializeField] private TextMeshProUGUI[] roll2TextsScreen;
    [SerializeField] private TextMeshProUGUI roll3TextScreen;
    [SerializeField] private TextMeshProUGUI[] frameTotalsTextScreen;
    [SerializeField] private TextMeshProUGUI overallScoreTextScreen;

    public int[,] gameScore = new int[10,3];
    public int[] frameScore = new int[10];
    public int overallScore;
    public int currentFrame;
    public int currentThrow;
    public GameObject scoreSheetContainer;
    public bool scoreSheetShowing;
    private bool justThrewStrikeOrSpare;
    private bool frame10roll1Strike;
    private bool frame10roll2StrikeOrSpare;
    private bool gameOver;
    private float scoreSheetTimer;
    public float scoreSheetTimeout;

    [SerializeField] private GameObject celebrationScreen;

    // Start is called before the first frame update
    void Start()
    {
        pinManager = GetComponent<PinManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreSheetShowing && !gameOver)
        {
            scoreSheetTimer += Time.deltaTime;

            if (scoreSheetTimer >= scoreSheetTimeout)
            {
                hideScoreSheet();
            }
        }
    }

    public void advanceScore ()
    {
        pinManager.hideKnockedPins();

        countPoints();
        updateTotals();
        updateFrameAndThrow();
        showScoreSheet();
        updateScreenScoring();
    }

    private void updateFrameAndThrow()
    {
        if (currentThrow == 0)
        {
            // Strike Check
            if (gameScore[currentFrame, 0] == 10)
            {
                roll1Texts[currentFrame].SetText("X");
                justThrewStrikeOrSpare = true;
                if (currentFrame == 9)
                {
                    frame10roll1Strike = true;
                    currentThrow += 1;
                }
                else
                {
                    currentFrame++;
                    currentThrow = 0;
                }
                pinManager.resetPins();
            }
            else
            {
                if (gameScore[currentFrame, currentThrow] > 0)
                {
                    roll1Texts[currentFrame].SetText("" + gameScore[currentFrame, currentThrow]);
                }
                else
                {
                    roll1Texts[currentFrame].SetText("-");
                }

                currentThrow += 1;
            }
        }
        else if (currentFrame == 9 && currentThrow == 1)
        {
            // Strike and spare check
            if (gameScore[currentFrame, currentThrow] == 10)
            {
                roll2Texts[currentFrame].SetText("X");
                justThrewStrikeOrSpare = true;
                frame10roll2StrikeOrSpare = true;
                currentThrow += 1;
                pinManager.resetPins();
            }
            else if (gameScore[currentFrame, 0] + gameScore[currentFrame, 1] == 10)
            {
                roll2Texts[currentFrame].SetText("/");
                justThrewStrikeOrSpare = true;
                frame10roll2StrikeOrSpare = true;
                currentThrow += 1;
                pinManager.resetPins();
            }
            else if (gameScore[currentFrame,0] == 10)
            {
                // if there was a strike in frame 10 roll 1, continue the game until 3 rolls
                if (gameScore[currentFrame, currentThrow] > 0)
                {
                    roll2Texts[currentFrame].SetText("" + gameScore[currentFrame, currentThrow]);
                }
                else
                {
                    roll2Texts[currentFrame].SetText("-");
                }

                currentThrow += 1;
            }
            else
            {
                if (gameScore[currentFrame, currentThrow] > 0)
                {
                    roll2Texts[currentFrame].SetText("" + gameScore[currentFrame, currentThrow]);
                }
                else
                {
                    roll2Texts[currentFrame].SetText("-");
                }

                roll3Text.SetText("-");
                gameOver = true;
            }

            
        }
        else if (currentThrow == 1)
        {
            // Spare check
            if (gameScore[currentFrame, 0] + gameScore[currentFrame, 1] == 10)
            {
                roll2Texts[currentFrame].SetText("/");
                justThrewStrikeOrSpare = true;
            }
            else
            {
                if (gameScore[currentFrame, currentThrow] > 0)
                {
                    roll2Texts[currentFrame].SetText("" + gameScore[currentFrame, currentThrow]);
                }
                else
                {
                    roll2Texts[currentFrame].SetText("-");
                }
            }

            currentThrow = 0;
            currentFrame++;

            pinManager.resetPins();
        }
        else if (currentFrame == 9 && currentThrow == 2)
        {
            // Strike and spare check
            if (gameScore[currentFrame, currentThrow] == 10)
            {
                roll3Text.SetText("X");
                justThrewStrikeOrSpare = true;
            }
            else if (gameScore[currentFrame, 0] + gameScore[currentFrame, 1] == 10)
            {
                roll3Text.SetText("/");
                justThrewStrikeOrSpare = true;
            }
            else
            {
                roll3Text.SetText("" + gameScore[currentFrame, currentThrow]);
            }

            gameOver = true;
        }
    }

    void updateTotals()
    {
        // Totals are updated after the current frame and throw are incremented
        overallScore = 0;

        for (int frame = 0; frame < 10; frame++)
        {
            frameScore[frame] = gameScore[frame, 0] + gameScore[frame, 1];

            if (frame == 9)
            {
                frameScore[frame] += gameScore[frame, 2];
                overallScore += frameScore[frame];
                frameTotalsText[frame].SetText("" + overallScore);
            }
            else if (gameScore[frame, 0] == 10)
            {
                int bonusScore = addFutureScores(2,frame);

                Debug.Log("Frame " + frame + " Strike Bonus Added: " + bonusScore);

                if (currentFrame >= frame+2 || (currentFrame == frame + 1 && currentThrow >= 1))
                {
                    overallScore += frameScore[frame] + bonusScore;
                    frameTotalsText[frame].SetText("" + overallScore);
                }
                else
                {
                    frameTotalsText[frame].SetText("");
                    return;
                }
            }
            else if (frameScore[frame] == 10)
            {
                int bonusScore = addFutureScores(1,frame);

                Debug.Log("Frame " + frame + " Spare Bonus Added: " + bonusScore);

                if ((currentFrame == frame+1 && currentThrow >= 1) || currentFrame >= frame+2 || bonusScore == 10)
                {
                    overallScore += frameScore[frame] + bonusScore;
                    frameTotalsText[frame].SetText("" + overallScore);
                }
                else
                {
                    frameTotalsText[frame].SetText("");
                    return;
                }
            } 
            else if (gameScore[frame,0] + gameScore[frame,1] > 0)
            {
                overallScore += frameScore[frame];
                frameTotalsText[frame].SetText("" + overallScore);
            }

            // blank out zero values
            if (frameTotalsText[frame].Equals("0"))
            {
                frameTotalsText[frame].SetText("");
            }
        }

        // update total current score
        overallScoreText.SetText("" + overallScore);
    }

    private int addFutureScores (int scoresToAdd, int frameWithBonus)
    {
        int bonusScore = 0;
        int bonusesAdded = 0;

        for (int frame = frameWithBonus+1; frame < 10; frame++)
        {
            for (int roll = 0; roll < 3; roll++)
            {
                if (roll < 2 || frame == 9)
                {
                    bonusScore += gameScore[frame, roll];
                    bonusesAdded++;
                }

                // Reduces bonuses added by 1 which will bypass the next roll on all frames except frame 10
                if (gameScore[frame, roll] == 10 && roll == 0 && frame <= 8)
                {
                    bonusesAdded--;
                }

                if (bonusesAdded == scoresToAdd)
                {
                    return bonusScore;
                }
            }
        }

        if (bonusesAdded == scoresToAdd)
        {
            return bonusesAdded;
        }
        else
        {
            return 0;
        }
        
    }

    void countPoints()
    {
        int currentScore = 0;

        for (int i = 0; i < pinManager.pins.Length; i++)
        {
            if (pinManager.pinKnockedOver[i])
            {
                currentScore++;
            }
        }

        if (currentThrow == 0)
        {
            gameScore[currentFrame, currentThrow] = currentScore;
        } 
        else if (currentThrow == 1)
        {
            if (frame10roll1Strike)
            {
                gameScore[currentFrame, currentThrow] = currentScore;
            }
            else
            {
                gameScore[currentFrame, currentThrow] = currentScore - gameScore[currentFrame, 0];
            }
        } 
        else if (currentThrow == 2)
        {

            if (frame10roll2StrikeOrSpare)
            {
                gameScore[currentFrame, currentThrow] = currentScore;
            }
            else
            {
                gameScore[currentFrame, currentThrow] = currentScore - gameScore[currentFrame, 1];
            }
        }
    }

    public void showScoreSheet ()
    {
        if (justThrewStrikeOrSpare)
        {
            celebrationScreen.SetActive(true);
        }

        LeanTween.moveLocalY(scoreSheetContainer, -445.8f, 0.5f);
        scoreSheetShowing = true;
    }

    public void hideScoreSheet ()
    {
        LeanTween.moveLocalY(scoreSheetContainer, -645, 0.5f);
        scoreSheetShowing = false;
        justThrewStrikeOrSpare = false;
        celebrationScreen.SetActive(false);
        scoreSheetTimer = 0;
    }

    private void updateScreenScoring()
    {
        for (int frame=0; frame < 10; frame++)
        {
            roll1TextsScreen[frame].SetText(roll1Texts[frame].text);
            roll2TextsScreen[frame].SetText(roll2Texts[frame].text);
            frameTotalsTextScreen[frame].SetText(frameTotalsText[frame].text);
        }
        overallScoreTextScreen.SetText("" + overallScoreText.text);
    }
}