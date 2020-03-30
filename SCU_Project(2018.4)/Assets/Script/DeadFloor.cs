using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadFloor : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Dieable theDead = collision.collider.gameObject.GetComponent<Dieable>();
        if (theDead != null)
            theDead.Dead();
    }
}
