using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StonesThrown : MonoBehaviour
{
    public float force = 20f;
    public float lifeTime = 2;
    void Start()
    {
        Invoke("OutOfLifeTime", lifeTime);
        GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.Impulse);
    }


    private void OnCollisionEnter(Collision collision)
    {
        string tag = collision.collider.gameObject.tag;
        GotHitable theHitted = collision.collider.gameObject.GetComponent<GotHitable>();

        if (theHitted != null)
        {
            theHitted.GotHit();
        }
        Destroy(gameObject);
    }

    private void OutOfLifeTime()
    {
        Destroy(gameObject);
    }
}
