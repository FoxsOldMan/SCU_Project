using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEnemy : MonoBehaviour, GotHitable, Dieable
{
    public GameObject player;
    public int HP = 3;
    public float scopeoOfFear = 10f;
    public float increaseRateOfSAM = 10f;
    public float scopeOfVisial = 30f;

    private Vector3 difference;
    private Color color;

    private void Start()
    {
        color = GetComponent<Renderer>().material.color;

        color.a = 0f;
        GetComponent<Renderer>().material.color = color;
    }

    private void Update()
    {
        if (HP <= 0)
            Dead();

        difference = player.transform.position - transform.position;
        if(difference.magnitude <= scopeOfVisial)
        {
            float transparency = (scopeOfVisial - difference.magnitude) / 10f;

            color.a = Mathf.Clamp(transparency, 0, 1);
            //Debug.Log("透明度" + color.a);
            GetComponent<Renderer>().material.color = color;

            if (difference.magnitude <= scopeoOfFear)
            {
                transform.forward = new Vector3(difference.x, 0, difference.z).normalized;
                player.GetComponent<PlayerStateAndMovement>().SAM += increaseRateOfSAM * Time.deltaTime;
            }

        }


    }

    public bool GotHit()
    {
        if (HP > 0)
        {
            HP--;
            return true;
        }

        return false;
    }

    public void Dead()
    {
        Destroy(gameObject);
    }
}
