using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateAndMovement : MonoBehaviour, GotHitable, Dieable
{

    public GameObject Player;
    private Vector3 distance;

    //Attribute
    public float HP = 5f;
    private EnemyState state = EnemyState.Idle;

    //MovementAttribute
    private Animator animator;
    private Vector3 oriPosition;

    private Vector3 dir = new Vector3(0f, 0f, 0f);
    public float walkSpeed = 20.0f;
    public float runSpeed = 30.0f;
    private float speed = 0f;

    //AttackRelated
    public float hitForce = 4f;
    public float redLine = 10f;
    public float safeLine = 15f;
    private float collisionStayTime = 0;
    private float attackCD = 3f;

    //AudioRelated
    private AudioSource audioSource;
    private AudioClip gotHitClip;

    

    // Start is called before the first frame update
    void Start()
    {
        oriPosition = transform.position;
        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        gotHitClip = Resources.Load<AudioClip>("SoundEffect/EnemyGotHit");


    }

    private void Update()
    {
        if (HP <= 0)
        {
            Dead();
        }

        UpdateState();
        Action();

    }

    private void UpdateState()
    {
        distance = Player.transform.position - transform.position;
        switch (state)
        {
            case EnemyState.Idle:
                if (distance.magnitude < redLine)
                {
                    state = EnemyState.Attack;
                }
                break;
            case EnemyState.Attack:
                if (!Player.GetComponent<PlayerStateAndMovement>().isAlive || distance.magnitude >= safeLine)
                {
                    state = EnemyState.GoBack;
                }
                break;
            case EnemyState.GoBack:
                if (Vector3.Distance(transform.position, oriPosition) <= 2f)
                {
                    state = EnemyState.Idle;
                }
                if (distance.magnitude <= safeLine/2)
                {
                    state = EnemyState.Attack;
                }
                break;
        }
    }

    private void Action()
    {
        switch (state)
        {
            case EnemyState.Idle:
                StopMoving();
                break;
            case EnemyState.Attack:
                RotateTowardTarget(Player.transform.position);
                MoveTowardTarget(Player.transform.position, true);
                break;
            case EnemyState.GoBack:
                RotateTowardTarget(oriPosition);
                MoveTowardTarget(oriPosition, false);
                break;
        }
    }

    public void RotateTowardTarget(Vector3 target)
    {
        Vector3 difference = target - transform.position;

        transform.forward = difference;
    }

    public void MoveTowardTarget(Vector3 target, bool run)
    {
        Vector3 difference = target - transform.position;
        dir.Set(difference.x, 0, difference.z);
        dir.Normalize();

        if (animator.GetBool("GotHit"))
        {
            Debug.Log("被攻击中无法移动");
            return;
        }
        if (run)
        {
            speed = runSpeed;
            animator.SetBool("Run", true);
        }
        else
        {
            speed = walkSpeed;
            animator.SetBool("Run", false);
        }


        if (dir != Vector3.zero)
        {
            animator.SetBool("Walk", true);

            //transform.position += dir * Time.deltaTime * speed;
            transform.Translate(dir * Time.deltaTime * speed, Space.World);
        }
        else
            animator.SetBool("Walk", false);
    }

    public void MoveForward(bool run)
    {
        if (animator.GetBool("GotHit"))
        {
            Debug.Log("被攻击中无法移动");
            return;
        }
        if (run)
        {
            speed = runSpeed;
            animator.SetBool("Run", true);
        }
        else
        {
            speed = walkSpeed;
            animator.SetBool("Run", false);
        }

        animator.SetBool("Walk", true);

        transform.Translate(transform.forward.normalized * Time.deltaTime * speed);

    }

    private void StopMoving()
    {
        animator.SetBool("Walk", false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (this.enabled && collision.collider.tag.Equals("Player"))
        {
            GameObject player = collision.collider.gameObject;
            player.GetComponent<PlayerStateAndMovement>().GotHit();
            player.GetComponent<Rigidbody>().AddForce((player.transform.position - transform.position) * hitForce, ForceMode.Impulse);

            collisionStayTime = 0f;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (this.enabled && collision.collider.tag.Equals("Player"))
        {
            GameObject player = collision.collider.gameObject;
            collisionStayTime += Time.deltaTime;
            if(collisionStayTime/attackCD > 0 && player.GetComponent<PlayerStateAndMovement>().GotHit())
            {
                player.GetComponent<Rigidbody>().AddForce((player.transform.position - transform.position) * hitForce, ForceMode.Impulse);
                collisionStayTime = 0;
            }
        }
    }

    public bool GotHit()
    {

        if (!animator.GetBool("GotHit"))
        {

            animator.SetBool("GotHit", true);
        }
        else
        {
            Debug.Log("被攻击动画未播完");
        }

        Debug.LogFormat("{0}被攻击了", gameObject.name);
        if (HP > 0)
        {
            HP -= 1;
        }
        audioSource.clip = gotHitClip;
        audioSource.Play();

        return true;
    }

    public void GotHitReset()
    {
        Debug.Log("GotHit Reset");
        animator.SetBool("GotHit", false);
    }

    public void Dead()
    {
        Debug.LogFormat("{0}挂了", gameObject.name);
        Destroy(gameObject);
    }
}

enum EnemyState
{
    Idle,
    Attack,
    GoBack,
}
