using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingGhost : MonoBehaviour
{
    [SerializeField] GameObject ghostBall;
    [SerializeField] GameObject ballSpawn;
    [SerializeField] GameObject[] ghostPins;
    [SerializeField] float ghostThrowSpeed;
    [SerializeField] float ghostThrowLength;
    [SerializeField] float ghostThrowDelayMinLength;
    [SerializeField] float ghostThrowDelayMaxLength;

    private Vector3[] ghostPinSetupLocations = new Vector3[10];
    private bool[] ghostPinKnockedOver = new bool[10];
    private GameObject selectedBall;
    private bool ghostThrowInProgress;
    private bool ghostDelayingThrow;
    private float ghostThrowTimer;
    private float ghostThrowDelayTimer;
    private float ghostThrowDelayLength;

    private int ghostCurrentThrow;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < ghostPins.Length; i++)
        {
            ghostPinSetupLocations[i] = ghostPins[i].transform.position;
        }
        selectedBall = Instantiate(ghostBall, new Vector3(ballSpawn.transform.position.x, ballSpawn.transform.position.y, ballSpawn.transform.position.z), ballSpawn.transform.rotation);
        ghostDelayingThrow = true;
        ghostThrowDelayLength = Random.Range(ghostThrowDelayMinLength, ghostThrowDelayMaxLength);
        ghostCurrentThrow = 1;
    }

    // Update is called once per frame
    void Update()
    {
        ghostHoldBall();
        checkTimers();
        ghostThrow();
    }

    void ghostHoldBall()
    {
        if (ghostDelayingThrow)
        {
            selectedBall.transform.position = new Vector3(ballSpawn.transform.position.x, ballSpawn.transform.position.y, ballSpawn.transform.position.z);
            selectedBall.transform.rotation = ballSpawn.transform.rotation;
            selectedBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    void checkTimers()
    {
        if (ghostThrowDelayTimer >= ghostThrowDelayLength)
        {
            ghostDelayingThrow = false;
            ghostThrowDelayTimer = 0;
        }

        if (ghostThrowTimer >= ghostThrowLength)
        {
            ghostThrowInProgress = false;
            ghostThrowTimer = 0;

            ghostDelayingThrow = true;
            ghostThrowDelayLength = Random.Range(ghostThrowDelayMinLength, ghostThrowDelayMaxLength);

            respawnBall();
            ghostAlleyReset();
        }
    }

    void ghostThrow() {

        if (ghostDelayingThrow)
        {
            ghostThrowDelayTimer += Time.deltaTime;
        }
        else if (ghostThrowInProgress) 
        {
            ghostThrowTimer += Time.deltaTime;
        }
        else 
        {
            ghostThrowInProgress = true;
            ghostThrowTimer = 0;

            selectedBall.GetComponent<Rigidbody>().velocity = selectedBall.transform.forward * ghostThrowSpeed;
            selectedBall.GetComponent<AudioSource>().Play();
        }
    }

    void respawnBall ()
    {
        Destroy(selectedBall);
        selectedBall = Instantiate(ghostBall, new Vector3(ballSpawn.transform.position.x, ballSpawn.transform.position.y, ballSpawn.transform.position.z), ballSpawn.transform.rotation);
    }

    void ghostAlleyReset()
    {
        if (ghostCurrentThrow == 2)
        {
            resetPins();
            ghostCurrentThrow = 1;
        }
        else
        {
            hideGhostKnockedPins();

            if (ghostHitStrike())
            {
                ghostCurrentThrow = 1;
                resetPins();
            }
            else
            {
                ghostCurrentThrow = 2;
            }
        }
    }

    private void resetPins()
    {
        for (int i = 0; i < ghostPins.Length; i++)
        {
            ghostPins[i].SetActive(true);

            ghostPins[i].GetComponent<Rigidbody>().velocity = Vector3.zero;

            ghostPins[i].transform.position = ghostPinSetupLocations[i];
            ghostPins[i].transform.rotation = new Quaternion(0, 0, 0, 0);

            ghostPinKnockedOver[i] = false;
        }
    }

    void hideGhostKnockedPins()
    {
        for (int i = 0; i < ghostPins.Length; i++)
        {
            float pinKnockbackdistance = Vector3.Distance(ghostPins[i].transform.position, ghostPinSetupLocations[i]);

            //Debug.Log("Pin " + pins[i] + " knockback: " + pinKnockbackdistance);

            if (pinKnockbackdistance > 0.1f && ghostPins[i].activeInHierarchy)
            {
                ghostPins[i].SetActive(false);
                ghostPinKnockedOver[i] = true;
            }
        }
    }

    bool ghostHitStrike ()
    {
        int ghostPinsHit = 0;

        for (int i=0; i<ghostPins.Length; i++)
        {
            if (ghostPinKnockedOver[i])
            {
                ghostPinsHit += 1;
            }
        }

        if (ghostPinsHit == 10)
        {
            return true;
        } else
        {
            return false;
        }
    }   
}