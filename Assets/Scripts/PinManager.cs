using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinManager : MonoBehaviour
{
    public GameObject[] pins;
    private Vector3[] pinSetupLocations = new Vector3[10];
    public bool[] pinKnockedOver = new bool[10];

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            pinSetupLocations[i] = pins[i].transform.position;
        }
    }

    public void hideKnockedPins ()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            float pinKnockbackdistance = Vector3.Distance(pins[i].transform.position, pinSetupLocations[i]);

            //Debug.Log("Pin " + pins[i] + " knockback: " + pinKnockbackdistance);

            if (pinKnockbackdistance > 0.1f && pins[i].activeInHierarchy)
            {
                pins[i].SetActive(false);
                pinKnockedOver[i] = true;
            }
        }
    }

    public void resetPins ()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            pins[i].SetActive(true);

            Rigidbody rb = pins[i].GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;

            pins[i].transform.position = pinSetupLocations[i];
            pins[i].transform.rotation = new Quaternion(0, 0, 0, 0);
            pinKnockedOver[i] = false;
        }
    }
}
