using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinManager : MonoBehaviour
{
    public GameObject[] pins;
    private Vector3[] pinSetupLocations = new Vector3[10];

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            pinSetupLocations[i] = pins[i].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hideKnockedPins ()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            Debug.Log("Pin " + i + " current position: " + pins[i].transform.position);
            Debug.Log("Pin " + i + " setup position: " + pinSetupLocations[i]);

            if (pins[i].transform.position != pinSetupLocations[i])
            {
                pins[i].SetActive(false);
            }
        }
    }

    public void resetPins ()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            pins[i].SetActive(true);
            pins[i].transform.position = pinSetupLocations[i];
            pins[i].transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }
}
