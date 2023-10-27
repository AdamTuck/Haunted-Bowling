using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

private void onTriggerEnter (Collider collision)
    {
        Debug.Log(collision.gameObject.name + " fell in pits");
    }
}
