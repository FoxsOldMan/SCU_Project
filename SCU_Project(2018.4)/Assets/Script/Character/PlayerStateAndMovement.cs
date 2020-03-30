using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateAndMovement : MonoBehaviour, GotHitable, Dieable
{
    [HideInInspector]
    public bool isAlive = true;
    //Attribute
    public float HP = 5f;
    public readonly float MaxHP = 10f;

    public float Energy = 10f;
    public readonly float MaxEnergy = 20f;
    public float EnergyConsumptionRate = 10f;
    public float EnergyRegen = 2f;

    public float SAM = 0;
    public readonly float MaxSAM = 100f;
    public float SamGrowthRate = 0.5f;

    //JumpAttribute
    private new Rigidbody rigidbody;
    private bool isJumping = false;
    private float jumpTimeCounter = 0f;
    public float jumpForce = 10f;
    public float jumpTime = 0.35f;

    //public AnimationCurve jumpCurve;

    //MovementAttribute
    private Animator animator;

    private Vector3 dir = new Vector3(0f, 0f, 0f);
    public float baseWalkspeed = 20.0f;
    public float baseRunspeed = 40.0f;

    private float walkspeed;
    private float runspeed;

    private float speed = 0f;

    //RotateAttribute
    private float mouse_x = 0f;
    public float xSpeed = 50f;

    //EquipmnetRelated
    private Backpack backpack;
    private Equipment equipment;

    //AudioRelated
    private AudioSource audioSource;

    private AudioClip jumpClip;
    private AudioClip gotHitClip;
    private AudioClip attackClip;
    private AudioClip deadClip;

    //ReactionRelated
    public LayerMask ReactionLayer;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        backpack = GetComponent<Backpack>();     
        equipment = GetComponent<Equipment>();
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        jumpClip = Resources.Load<AudioClip>("SoundEffect/Jump");
        gotHitClip = Resources.Load<AudioClip>("SoundEffect/PlayerGotHit");
        attackClip = Resources.Load<AudioClip>("SoundEffect/Throw");
        deadClip = Resources.Load<AudioClip>("SoundEffect/Dead");

        walkspeed = baseWalkspeed;
        runspeed = baseRunspeed;

    }

    private void Update()
    {
        if (isAlive)
        {
            IncreaseSAM();
            if (HP <= 0 || SAM >= MaxSAM)
            {
                Dead();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            float cloestDis = float.MaxValue;
            Interactable interactor = null;
            Collider[] Interactors = Physics.OverlapSphere(transform.position, 5, ReactionLayer);
            foreach (Collider item in Interactors)
            {
                if(Vector3.Distance(transform.position, item.gameObject.transform.position) < cloestDis)
                {
                    if(item.GetComponent<Interactable>() != null)
                    {
                        cloestDis = Vector3.Distance(transform.position, item.gameObject.transform.position);
                        interactor = item.GetComponent<Interactable>();
                    }

                }
                
            }
            if (interactor != null)
                interactor.Interaction();
        }
    }

    public void Rotate(Quaternion TargetRotation)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation, Time.deltaTime * 100);
    }

    public void Rotate(float x)
    {
        mouse_x += x * Time.deltaTime * xSpeed;
        while (mouse_x > 360)
            mouse_x -= 360;
        while (mouse_x < -360)
            mouse_x += 360;
        transform.rotation = Quaternion.Euler(0, mouse_x, 0);
    }

    public void Move(float x, float z, bool run)
    {
        if (animator.GetBool("GotHit"))
        {
            //Debug.Log("被攻击中无法移动");
            return;
        }
        if (run && Energy > 0)
        {
            
            Energy -= EnergyConsumptionRate * Time.deltaTime;
            if (Energy < 0)
                Energy = 0;

            speed = runspeed;
            animator.SetBool("Run", true);
        }
        if (!run || Energy <= 0)
        {
            if (Energy < MaxEnergy)
            {
                Energy += EnergyRegen * Time.deltaTime;
            }
            else
                Energy = MaxEnergy;


            speed = walkspeed;
            animator.SetBool("Run", false);
        }

        dir.x = x;
        dir.z = z;

        if (dir != Vector3.zero)
        {
            animator.SetBool("Walk", true);

            transform.Translate(dir * Time.deltaTime * speed);
            dir.Set(0f, 0f, 0f);
        }
        else
            animator.SetBool("Walk", false);
    }

    public void Jump()
    {
        if (!animator.GetBool("Jump") && Input.GetKeyDown(KeyCode.Space))
        {
            //JumpForSeconds();
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rigidbody.velocity = Vector3.up * jumpForce;
            audioSource.clip = jumpClip;
            audioSource.Play();
            animator.SetBool("Jump", true);
        }
        if(Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rigidbody.velocity = Vector3.up * jumpForce * 0.8f;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("Floor"))
        {
            animator.SetBool("Jump", false);
            //jumpable = true;
        }
    }

    //public void JumpForSeconds()
    //{
    //    StartCoroutine(jumpForSeconds());
    //}

    //private IEnumerator jumpForSeconds()
    //{
    //    float time = 0;
    //    float x = 0;
    //    float y = 0;
    //    float z = 0;
    //    float height = 0;
    //    while(time <= 2)
    //    {

    //        x = transform.position.x;
    //        y = transform.position.y;
    //        z = transform.position.z;
            
    //        height = jumpCurve.Evaluate(time);
    //        transform.position = new Vector3(x, y+height, z);
    //        yield return new WaitForEndOfFrame();
    //        time += .1f;
    //    }
    //}

    public void UseItem()
    {

        if (backpack != null)
        {
            ItemData itemdata = backpack.handDatas[1];
            if (itemdata == null || itemdata.quantity <= 0)
            {
                Debug.Log("无物品在主动道具栏");
                return;
            }
            else
            {
                switch (itemdata.item.Type)
                {
                    case ItemType.ToEnemy:
                        if (!animator.GetBool("Attack") && !animator.GetBool("Jump"))
                        {
                            audioSource.clip = attackClip;
                            audioSource.Play();
                            animator.SetBool("Attack", true);
                        }
                        else
                            Debug.Log("攻击动画未播完");
                        break;

                    case ItemType.ToSelf:
                        itemdata.item.Use(this);
                        break;
                    case ItemType.Spetial:
                        Debug.Log("主动栏不存在主动道具");
                        break;
                }

                itemdata.quantity--;

            }
        }

    }

    public bool GotHit()
    {
        if (!animator.GetBool("GotHit"))
        {
            //Debug.Log("被攻击了");
            if (HP > 0)
            {
                HP -= 1;
            }
            SAM += 10;
            audioSource.clip = gotHitClip;
            audioSource.Play();
            animator.SetBool("GotHit", true);
            return true;
        }
        else
        {
            //Debug.Log("被攻击动画未播完");
            return false;
        }

    }

    public void IncreaseSAM()
    {
        if (SAM < MaxSAM)
        {
            SAM += SamGrowthRate * Time.deltaTime;
        }
        else
            SAM = MaxSAM;

        if(SAM >= MaxSAM * 0.5f)
        {
            walkspeed = 2 * baseWalkspeed;
            runspeed = 2 * baseRunspeed;
        }
        else
        {
            walkspeed = baseWalkspeed;
            runspeed = baseRunspeed;
        }
    }

    public void Dead()
    {
        Debug.Log("Dead");
        isAlive = false;
        audioSource.clip = deadClip;
        audioSource.Play();
    }

}
