using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitCollision : MonoBehaviour
{
private void onTriggerEnter (Collider collision)
    {
        Debug.Log(collision.gameObject.name + " fell in pits");
    }
}
