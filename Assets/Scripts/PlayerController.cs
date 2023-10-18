using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    BowlingScoring bowlingScorer;

    [SerializeField] private Transform throwingArrow;
    public float playerMoveSpeed;
    public GameObject bowlingBall;
    public GameObject ballSpawnPoint;
    public float throwSpeed;
    public float arrowMinX, arrowMaxX;

    public bool throwInProgress;
    public float throwTimer;
    public float throwDuration;

    private void Start()
    {
        bowlingScorer = GetComponent<BowlingScoring>();
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

                resetBall();

                bowlingBall.GetComponent<Rigidbody>().velocity = bowlingBall.transform.forward * throwSpeed;
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

            resetBall();
            bowlingScorer.advanceScore();
        }
    }

    private void UpdateThrowArrow()
    {
        throwingArrow.position += throwingArrow.right * Input.GetAxis("Horizontal") * playerMoveSpeed * Time.deltaTime;

        float movePosition = Input.GetAxis("Horizontal") * playerMoveSpeed * Time.deltaTime;
        throwingArrow.position = new Vector3(Mathf.Clamp(throwingArrow.position.x + movePosition, arrowMinX, arrowMaxX), throwingArrow.position.y, throwingArrow.position.z);
    }

    void resetBall ()
    {
        bowlingBall.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        bowlingBall.transform.rotation = new Quaternion(0, 0, 0, 0);
        bowlingBall.transform.position = (new Vector3(throwingArrow.transform.position.x, ballSpawnPoint.transform.position.y, ballSpawnPoint.transform.position.z));
    }
}
