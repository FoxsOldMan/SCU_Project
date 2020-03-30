using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10.0f;

    float startTime;
    public float lifeTime = 2;
    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        transform.position += speed * transform.forward * Time.deltaTime;
        if (startTime + lifeTime < Time.time)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        string tag = collision.collider.gameObject.tag;
        GotHitable theHitted = null;
        switch (tag)
        {
            case "Player":
                theHitted = collision.collider.gameObject.GetComponent<PlayerStateAndMovement>();
                break;
            case "Enemy":
                theHitted = collision.collider.gameObject.GetComponent<EnemyStateAndMovement>();
                Debug.Log("攻击到敌人");
                break;
            case "NPC":
                break;
            default:
                break;
        }
        if(theHitted != null)
        {
            theHitted.GotHit();
        }
        Destroy(gameObject);
    }
}
