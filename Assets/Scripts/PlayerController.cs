using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    BowlingScoring bowlingScorer;

    [SerializeField] private Transform throwingArrow;
    [SerializeField] private Animator arrowAnimator;
    public float playerMoveSpeed;
    public GameObject bowlingBall;
    public GameObject ballSpawnPoint;
    [SerializeField] private Rigidbody[] bowlingBallColours;
    [SerializeField] private Rigidbody selectedBall;
    public float throwSpeed;
    public float arrowMinX, arrowMaxX;

    public bool throwInProgress;
    public float throwTimer;
    public float throwDuration;

    private void Start()
    {
        bowlingScorer = GetComponent<BowlingScoring>();
        resetThrow();
    }

    // Update is called once per frame
    void Update()
    {
        CheckThrowDuration();

        UpdateThrowArrow();

        ThrowBall();
    }

    private void ThrowBall()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (!throwInProgress && !bowlingScorer.scoreSheetShowing)
            {
                throwInProgress = true;
                arrowAnimator.SetBool("Aiming", false);

                Destroy(selectedBall);
                selectedBall = Instantiate(bowlingBallColours[Random.Range(0, 8)], 
                    new Vector3(throwingArrow.position.x, ballSpawnPoint.transform.position.y, 
                    ballSpawnPoint.transform.position.z), throwingArrow.transform.rotation);

                selectedBall.GetComponent<Rigidbody>().velocity = selectedBall.transform.forward * throwSpeed;
            }
        }
    }

    private void CheckThrowDuration()
    {
        if (throwInProgress)
        {
            throwTimer += Time.deltaTime;
        }

        if (throwTimer >= throwDuration)
        {
            throwTimer = 0;
            throwInProgress = false;

            bowlingScorer.advanceScore();
            resetThrow();
        }
    }

    private void UpdateThrowArrow()
    {
        float movePosition = Input.GetAxis("Horizontal") * playerMoveSpeed * Time.deltaTime;
        throwingArrow.position = new Vector3(Mathf.Clamp(throwingArrow.position.x + movePosition, arrowMinX, arrowMaxX), 
            throwingArrow.position.y, throwingArrow.position.z);
    }

    void resetThrow ()
    {
        arrowAnimator.SetBool("Aiming", true);
    }
}
