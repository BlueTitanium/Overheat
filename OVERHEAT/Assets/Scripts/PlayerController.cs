using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController p;

    public Animator spritesAnim;

    //FLOAT FROM 0-1
    public float heat = 0f;
    public bool overheat = false;
    public float loseOverHeat = .5f;

    public float maxHP = 100f;
    public float curHP = 100f;

    public float baseMoveSpeed = 5f;
    public float speedIncrement = 5f;
    public float jumpStrength = 5f;
    public float fastFallStrength = .3f;
    public float dashMultiplier = 2f;
    public float dashTime = .3f;
    public float dashLeft = 0;
    public float dashCD = .6f;
    public float dashCDLeft = 0f;
    public DashTrail trail;
    public float baseDamage = 10f;
    public float damageIncrement = 10f;
    public float attackSpeed = 1f;
    public bool attacking = false;

    bool isGrounded = false;
    public Transform GroundCheck1;
    public LayerMask groundLayer; // Insert the layer here.

    private Rigidbody2D rb2d;
    float horizontal = 0;
    float originalGrav;
    
    public float dischargeCD = .5f;
    public float dischargeCDLeft = 0f;
    public GameObject playerProjectile;
    public Transform shootpoint;

    // Start is called before the first frame update
    void Start()
    {
        p = this;
        curHP = maxHP;
        rb2d = GetComponent<Rigidbody2D>();
        originalGrav = rb2d.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck1.position, 0.15f, groundLayer);
        //MOVE LEFT AND RIGHT
        if (dashLeft <= 0)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            if (horizontal < 0 && !attacking)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (horizontal > 0 && !attacking)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            //DASH
            if (dashCDLeft <= 0 && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.LeftShift)))
            {
                dashCDLeft = dashCD;
                horizontal = transform.localScale.x * dashMultiplier;
                dashLeft = dashTime;
                trail.mbEnabled = true;
                rb2d.gravityScale = 0f;
            }
        }
        else
        {
            dashLeft -= Time.deltaTime;
            if(dashLeft <= 0)
            {
                trail.mbEnabled = false;
                rb2d.gravityScale = originalGrav;
            } 
        }
        if(dashCDLeft > 0)
        {
            dashCDLeft -= Time.deltaTime;
        }
        Vector2 nextVelocity = new Vector2(horizontal * baseMoveSpeed + (heat * speedIncrement), rb2d.velocity.y);
        //JUMP
        if(isGrounded && (Input.GetKeyDown(KeyCode.Space) || (Input.GetAxisRaw("Vertical") > 0)))
        {
            nextVelocity.y = jumpStrength;
        }
        if(!isGrounded && (Input.GetAxisRaw("Vertical") < 0))
        {
            nextVelocity.y = rb2d.velocity.y-fastFallStrength;
        }
        if(dashLeft > 0)
        {
            nextVelocity.y = 0;
        }
        //SWORD ATTACK
        if(!attacking && (Input.GetKeyDown(KeyCode.C) || Input.GetMouseButtonDown(0)))
        {
            StartCoroutine(Attack());
        }

        //DISCHARGE
        if (dischargeCDLeft <= 0 && (Input.GetKeyDown(KeyCode.X) || Input.GetMouseButtonDown(1)))
        {
            StartCoroutine(Discharge());
        }
        
        if(dischargeCDLeft > 0)
        {
            dischargeCDLeft -= Time.deltaTime;
        }

        rb2d.velocity = nextVelocity;
        spritesAnim.SetBool("IsMoving", Mathf.Abs(rb2d.velocity.x) > 0);
        spritesAnim.SetBool("Grounded", isGrounded);
        spritesAnim.SetFloat("Vertical", rb2d.velocity.y);
        spritesAnim.SetFloat("DashLeft", dashLeft);
    }

    IEnumerator OVERHEAT()
    {
        yield return null;
    }
    IEnumerator LoseOVERHEAT()
    {
        yield return null;
    }

    public void IncreaseHeat(float amount)
    {
        heat += amount;
        if(heat >= .25f)
        {
            //unlock dash
        }
        if (heat >= .5f)
        {
            //unlock parry
        }
        if (heat >= .75f)
        {
            //unlock discharge
        }
        if (heat >= 1)
        {
            heat = 1;
            if (!overheat)
            {
                StartCoroutine(OVERHEAT());
            }
        }
    }

    public void DecreaseHeat(float amount)
    {
        heat -= amount;
        if(overheat && heat < loseOverHeat)
        {
            StartCoroutine(LoseOVERHEAT());
        }
        if (heat < 0)
        {
            heat = 0;
        }
    }

    public void TakeKnockback(Vector2 dir, float time)
    {

    }

    public void TakeDamage(float damage, bool increaseHeat = true)
    {
        

        if(curHP > 0)
        {
            if (increaseHeat)
            {
                IncreaseHeat(.1f);
            }

            curHP -= damage;

            if (curHP <= 0)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        //show death and loss screen
    }

    IEnumerator Attack()
    {
        attacking = true;
        spritesAnim.SetTrigger("Attack");
        yield return new WaitUntil(() => spritesAnim.GetCurrentAnimatorStateInfo(0).IsName("Player_Attack"));
        yield return new WaitUntil(() => !spritesAnim.GetCurrentAnimatorStateInfo(0).IsName("Player_Attack"));
        attacking = false;
    }
    IEnumerator Discharge()
    {
        dischargeCDLeft = dischargeCD;
        spritesAnim.SetTrigger("Discharge");
        yield return new WaitUntil(() => spritesAnim.GetCurrentAnimatorStateInfo(0).IsName("Player_Discharge"));
        yield return new WaitForSeconds(.3f);
        //SHOOT PROJECTILE
        
        //yield return new WaitUntil(() => !spritesAnim.GetCurrentAnimatorStateInfo(0).IsName("Player_Discharge"));

    }
}
