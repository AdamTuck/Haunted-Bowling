using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    BowlingScoring bowlingScorer;
    PlayerInput playerInput;

    [Header("References")]
    [SerializeField] private Transform throwingArrow;
    [SerializeField] private Animator arrowAnimator;
    [SerializeField] private GameObject[] bowlingBallColours;
    [SerializeField] private GameObject selectedBall;
    public GameObject bowlingBall;
    public GameObject ballSpawnPoint;

    [Header("Arrow Properties")]
    public float playerArrowMoveSpeed;
    public float arrowMinX, arrowMaxX;

    [Header("Throw Properties")]
    public float throwSpeed;
    public float throwProgressDuration;
    public float throwCompletedDuration;
    private bool throwInProgress;
    [HideInInspector] public bool throwCompleted;
    private float throwProgressTimer;
    private float throwCompletedTimer;

    [Header("Walk Around")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float turnSpeed;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private GameObject bowlingCam;
    [SerializeField] private GameObject fpsCam;
    private Vector3 playerVelocity;
    private bool isBowling;

    private void Start()
    {
        bowlingScorer = GameManager.instance.bowlingScorer;
        playerInput = PlayerInput.GetInstance();
        isBowling = true;
    }

    void Update()
    {
        CheckThrowDuration();
        CheckMoveType();
        UpdateThrowArrow();

        if (isBowling)
        {
            ThrowBall();
        }
        else
        {
            MovePlayer();
            RotatePlayer();
        }
    }

    void CheckMoveType ()
    {
        if (playerInput.switchMovement)
        {
            isBowling = !isBowling;
            AdjustCamera();
        }
    }

    void AdjustCamera ()
    {
        if (isBowling)
        {
            fpsCam.SetActive(false);
            bowlingCam.SetActive(true);
            UnlockCursor();
        }
        else
        {
            fpsCam.SetActive(true);
            bowlingCam.SetActive(false);
            LockCursor();
        }
    }

    private void ThrowBall()
    {
        if (playerInput.throwBall)
        {
            if (!throwInProgress && !throwCompleted && !bowlingScorer.scoreSheetShowing)
            {
                throwInProgress = true;
                arrowAnimator.SetBool("Aiming", false);

                selectedBall = Instantiate(bowlingBallColours[Random.Range(0, 8)], 
                    new Vector3(throwingArrow.position.x, ballSpawnPoint.transform.position.y, 
                    ballSpawnPoint.transform.position.z), throwingArrow.transform.rotation);

                selectedBall.GetComponent<Rigidbody>().velocity = selectedBall.transform.forward * throwSpeed;
                selectedBall.GetComponent<AudioSource>().Play();
            }
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
        if (bowlingScorer.scoreSheetShowing || throwInProgress || throwCompleted || !isBowling)
            arrowAnimator.SetBool("Aiming", false);
        else
        {
            arrowAnimator.SetBool("Aiming", true);

            float movePosition = playerInput.horizontal * playerArrowMoveSpeed * Time.deltaTime;
            throwingArrow.position = new Vector3(Mathf.Clamp(throwingArrow.position.x + movePosition, arrowMinX, arrowMaxX),
                throwingArrow.position.y, throwingArrow.position.z);
        } 
    }

    public void resetThrow ()
    {
        //arrowAnimator.SetBool("Aiming", true);
    }

    // PLAYER MOVEMENT //
    void MovePlayer()
    {
        characterController.Move((transform.forward * playerInput.vertical + transform.right * playerInput.horizontal) * moveSpeed * Time.deltaTime);

        if (playerVelocity.y < 0)
        {
            playerVelocity.y = -2.0f;
        }

        playerVelocity.y += gravity * Time.deltaTime;

        characterController.Move(playerVelocity * Time.deltaTime);
    }

    public void SetYVelocity(float value)
    {
        playerVelocity.y = value;
    }

    public float GetForwardSpeed()
    {
        return playerInput.vertical * moveSpeed;
    }

    void RotatePlayer()
    {
        transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime * playerInput.mouseX);
    }

    void LockCursor ()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}