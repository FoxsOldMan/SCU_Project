using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenTrap : MonoBehaviour
{
    public float HitForce = 5f;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("Player"))
        {
            GameObject player = collision.collider.gameObject;
            player.GetComponent<GotHitable>().GotHit();
            player.GetComponent<Rigidbody>().AddForce((player.transform.position - transform.position)*HitForce, ForceMode.Impulse);
        }    
    }
}
