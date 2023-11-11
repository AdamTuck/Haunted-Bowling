using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    BowlingScoring bowlingScorer;
    [SerializeField] private SoundManager soundManager;

    [SerializeField] private Transform throwingArrow;
    [SerializeField] private Animator arrowAnimator;
    public float playerMoveSpeed;
    public GameObject bowlingBall;
    public GameObject ballSpawnPoint;
    [SerializeField] private GameObject[] bowlingBallColours;
    [SerializeField] private GameObject selectedBall;
    public float throwSpeed;
    public float arrowMinX, arrowMaxX;

    public bool throwInProgress;
    public bool throwCompleted;
    public float throwProgressTimer;
    public float throwCompletedTimer;
    public float throwProgressDuration;
    public float throwCompletedDuration;

    private float mobileHorizontalAxis;

    private void Start()
    {
        bowlingScorer = GetComponent<BowlingScoring>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckThrowDuration();

        UpdateThrowArrow();

        TryThrowBall();
    }

    public void ThrowBall()
    {
        if (!throwInProgress && !throwCompleted && !bowlingScorer.scoreSheetShowing)
        {
            throwInProgress = true;
            arrowAnimator.SetBool("Aiming", false);

            selectedBall = Instantiate(bowlingBallColours[Random.Range(0, 8)], 
                new Vector3(throwingArrow.position.x, ballSpawnPoint.transform.position.y, 
                ballSpawnPoint.transform.position.z), throwingArrow.transform.rotation);

            selectedBall.GetComponent<Rigidbody>().velocity = selectedBall.transform.forward * throwSpeed;
            soundManager.PlaySound("ballThrow");
            soundManager.PlaySound("ballRolling");
        }
    }

    private void TryThrowBall()
    {
        if (Input.GetKey(KeyCode.W))
        {
            ThrowBall();
        }
    }

    private void CheckThrowDuration()
    {
        if (throwInProgress)
        {
            throwProgressTimer += Time.deltaTime;
        }

        if (throwCompleted)
        {
            throwCompletedTimer += Time.deltaTime;
            throwInProgress = false;
        }

        if (throwCompletedTimer >= throwCompletedDuration || throwProgressTimer >= throwProgressDuration)
        {
            throwProgressTimer = 0;
            throwCompletedTimer = 0;
            throwCompleted = false;
            throwInProgress = false;

            Destroy(selectedBall);

            bowlingScorer.advanceScore();
            resetThrow();
        }
    }

    private void UpdateThrowArrow()
    {
        if (bowlingScorer.scoreSheetShowing || throwInProgress || throwCompleted)
        {
            arrowAnimator.SetBool("Aiming", false);
        } else
        {
            arrowAnimator.SetBool("Aiming", true);
        }

        float movePosition;

#if UNITY_STANDALONE || UNITY_EDITOR
        movePosition = Input.GetAxis("Horizontal") * playerMoveSpeed * Time.deltaTime;
#elif UNITY_ANDROID || UNITY_IOS
        movePosition = mobileHorizontalAxis * playerMoveSpeed * Time.deltaTime;
#endif

        throwingArrow.position = new Vector3(Mathf.Clamp(throwingArrow.position.x + movePosition, arrowMinX, arrowMaxX), 
            throwingArrow.position.y, throwingArrow.position.z);

    }

    public void setMobileHorizontal(bool isLeft)
    {
        if (isLeft)
        {
            mobileHorizontalAxis = -1;
        }
        else
        {
            mobileHorizontalAxis = 1;
        }
    }

    public void ResetMobileHorizontal()
    {
        mobileHorizontalAxis = 0;
    }

    public void resetThrow ()
    {
        //arrowAnimator.SetBool("Aiming", true);
    }
}
