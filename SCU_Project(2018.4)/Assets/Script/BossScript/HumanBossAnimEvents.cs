using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanBossAnimEvents : MonoBehaviour, GotHitable
{
    public float bossMAXHP = 20;
    [HideInInspector]
    public float bossHP;
    public GameMode gameMode;
    public Collider breathAttackCollider;
    public Collider yowlAttackCollider;
    public ParticleSystem yowlEffect;
    public ParticleSystem breathOfFireEffect;

    void Start()
    {
        if (breathAttackCollider == null)
            Debug.Log("breathAttackCollider缺失");
        else
            breathAttackCollider.enabled = false;

        if (yowlAttackCollider == null)
            Debug.Log("breathAttackCollider缺失");
        else
            yowlAttackCollider.enabled = false;

        if (yowlEffect == null)
            Debug.Log("YowlEffect缺失");

        if (breathOfFireEffect == null)
            Debug.Log("breathOfFireEffect缺失");

        bossHP = bossMAXHP;
    }

    void Update()
    {
        if (!isAlive())
        {
            for (int i = 0; i < gameMode.puzzlesCompleteness.Length; i++)
            {
                gameMode.puzzlesCompleteness[i] = true;
            }

            Debug.Log("BOSS 挂了");
            Destroy(gameObject);
        }
            
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform rootParent = other.transform;
        if(rootParent.tag != "Player")
        {
            Debug.Log(rootParent.name + ">Tag:" + rootParent.gameObject.tag);
            return;
        }

        while (rootParent.GetComponent<Rigidbody>() == null)
        {
            if (rootParent.parent != null)
                rootParent = rootParent.parent;
            else
                return;
        }
        Vector3 forceDirection = Vector3.ProjectOnPlane(rootParent.position - transform.position, Vector3.up).normalized;
        rootParent.GetComponent<Rigidbody>().AddForce(rootParent.GetComponent<Rigidbody>().mass * forceDirection * 100, ForceMode.Impulse);

        Debug.Log("Attack: " + rootParent.name);

        breathAttackCollider.enabled = false;
        yowlAttackCollider.enabled = false;

        Debug.Log("触发器触发");
    }

    public bool StartYowlEffect()
    {
        if(yowlEffect != null)
        {
            yowlEffect.Play();
            return true;
        }

        return false;
    }

    public bool StartBreathOfFireEffect()
    {
        if (breathOfFireEffect != null)
        {
            breathOfFireEffect.Play();
            return true;
        }

        return false;
    }

    public bool StopBreathOfFireEffect()
    {
        if (breathOfFireEffect != null)
        {
            breathOfFireEffect.Stop();
            return true;
        }

        return false;
    }

    public void ActiveBreathAttackCollider()
    {
        breathAttackCollider.enabled = true;
        StartBreathOfFireEffect();
        Debug.Log("触发breathAttack");
    }

    public void DeactiveBreathAttackCollider()
    {
        breathAttackCollider.enabled = false;
        StopBreathOfFireEffect();
        Debug.Log("结束breathAttack");
    }

    public void ActiveYowlAttackCollider()
    {
        yowlAttackCollider.enabled = true;
        StartYowlEffect();
        Debug.Log("触发YowlAttack");
    }

    public void DeactiveYowlAttackCollider()
    {
        yowlAttackCollider.enabled = false;
        Debug.Log("结束YowlAttack");

    }

    public void HandAttack()
    {
        LayerMask layer = LayerMask.GetMask("Player");
        List<Transform> rootParents = new List<Transform>();
        Collider[] enemys = Physics.OverlapSphere(transform.position, 5, layer);
        Vector3 difference;
        foreach (var item in enemys)
        {
            //判定是否在前方90°角内
            difference = Vector3.ProjectOnPlane(item.transform.position - transform.position, Vector3.up);
            if (Vector3.Angle(transform.forward, difference) > 45)
                continue;

            Transform rootParent = item.transform;
            while (rootParent.GetComponent<Rigidbody>() == null)
            {
                if (rootParent.parent != null)
                    rootParent = rootParent.parent;
                else
                    break;
            }
            if (!rootParents.Contains(rootParent))
            {
                rootParents.Add(rootParent);
                Debug.Log("Attack:" + rootParent.name);

            }

        }

        foreach (var item in rootParents)
        {
            if (item.GetComponent<Rigidbody>() != null)
            {
                Vector3 forceDirection = Vector3.ProjectOnPlane(item.position - transform.position, Vector3.up).normalized;
                item.GetComponent<Rigidbody>().AddForce(item.GetComponent<Rigidbody>().mass * forceDirection * 100, ForceMode.Impulse);
                Debug.Log("Attack: " + item.name);
            }
            //Debug.Log("Attack:" + item.name);
        }
    }

    public bool GotHit()
    {
        Debug.Log("BOSS is hitted");
        bossHP--;
        return true;
    }

    public bool isAlive()
    {
        if (bossHP > 0)
            return true;

        return false;
    }
}
