using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController p;

    public Animator spritesAnim;
    public SpriteRenderer sprite;
    public Color initialColor, endColor;
    //FLOAT FROM 0-1
    public float heat = 0f;
    public bool overheat = false;
    public float loseOverHeat = .5f;
    public float canCoolDown, CoolDown;
    public ParticleSystem[] fireParticles;
    public ParticleSystem healParticles;
    public Animation overheatTextAnimation;

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
    public float nextDamage = 2.0f;
    float dmgTime;

    bool isGrounded = false;
    public Transform GroundCheck1;
    public LayerMask groundLayer; // Insert the layer here.

    private Rigidbody2D rb2d;
    float horizontal = 0;
    float originalGrav;
    
    public float dischargeCD = .8f;
    public float dischargeCDLeft = 0f;
    public GameObject playerProjectile;
    public Transform shootpoint;

    public bool dashAllowed, deflectAllowed, dischargeAllowed;

    [Header("HUD ELEMENTS")]
    public Image hpBar;
    public Image dischargeBar;
    public Image dashBar;
    public Image heatMeter, heatMeter2;
    public GameObject dashText, deflectText, dischargeText;
    public GameObject OVERHEATED;
    public bool takingDamage;

    // Start is called before the first frame update
    void Start()
    {
        p = this;
        curHP = maxHP;
        rb2d = GetComponent<Rigidbody2D>();
        originalGrav = rb2d.gravityScale;
        hpBar.fillAmount = curHP / maxHP;
        OVERHEATED.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!(Time.timeScale == 0))
        {
            dmgTime -= Time.deltaTime; // For damage immunity
            isGrounded = Physics2D.OverlapCircle(GroundCheck1.position, 0.15f, groundLayer);
            //MOVE LEFT AND RIGHT
            if (dashLeft <= 0)
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
                    if (dashAllowed && dashCDLeft <= 0 && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.LeftShift)))
                    {
                        dashCDLeft = dashCD;
                        horizontal = transform.localScale.x * dashMultiplier;
                        dashLeft = dashTime;
                        trail.mbEnabled = true;
                        rb2d.gravityScale = 0f;
                        //IncreaseHeat(0f);
                    }
                }
                else
                {
                    dashLeft -= Time.deltaTime;
                    if (dashLeft <= 0)
                    {
                        trail.mbEnabled = false;
                        rb2d.gravityScale = originalGrav;
                    }
                }
                if (dashCDLeft > 0)
                {
                    dashCDLeft -= Time.deltaTime;
                    dashBar.fillAmount = (dashCD - dashCDLeft) / dashCD;
                }
                Vector2 nextVelocity;
                if (!overheat)
                {
                    nextVelocity = new Vector2((horizontal * baseMoveSpeed) + (horizontal * heat * speedIncrement), rb2d.velocity.y);
                }
                else
                {
                    nextVelocity = new Vector2((horizontal * baseMoveSpeed) + (horizontal * 1.5f * speedIncrement), rb2d.velocity.y);
                    StartCoroutine(TakeDamage(Time.deltaTime, false, false, false));
                }
                //JUMP
                if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || (Input.GetAxisRaw("Vertical") > 0)))
                {
                    nextVelocity.y = jumpStrength;
                }
                if (!isGrounded && (Input.GetAxisRaw("Vertical") < 0))
                {
                    nextVelocity.y = rb2d.velocity.y - fastFallStrength;
                }
                if (dashLeft > 0)
                {
                    nextVelocity.y = 0;
                }
                //SWORD ATTACK
                if (!attacking && (Input.GetKeyDown(KeyCode.C) || Input.GetMouseButtonDown(0)))
                {
                    StartCoroutine(Attack());
                }

                //DISCHARGE
                if (dischargeAllowed && dischargeCDLeft <= 0 && (Input.GetKeyDown(KeyCode.X) || Input.GetMouseButtonDown(1)))
                {
                    StartCoroutine(Discharge());
                }

                if (dischargeCDLeft > 0)
                {
                    dischargeCDLeft -= Time.deltaTime;
                    dischargeBar.fillAmount = (dischargeCD - dischargeCDLeft) / dischargeCD;
                }

                if (canCoolDown > 0)
                {
                    canCoolDown -= Time.deltaTime;
                }
                else
                {
                    DecreaseHeat(Time.deltaTime * .2f);
                }


                rb2d.velocity = nextVelocity;
                spritesAnim.SetBool("IsMoving", Mathf.Abs(rb2d.velocity.x) > 0);
                spritesAnim.SetBool("Grounded", isGrounded);
                spritesAnim.SetFloat("Vertical", rb2d.velocity.y);
                spritesAnim.SetFloat("DashLeft", dashLeft);
            }
        }
    }

    IEnumerator OVERHEAT()
    {
        CameraShake.cs.cameraShake(1f, 3.2f);
        OVERHEATED.SetActive(true);
        overheat = true;
        sprite.color = Color.red;
        overheatTextAnimation.Play();
        foreach(var p in fireParticles)
        {
            p.Play();
        }
        yield return null;
    }
    IEnumerator LoseOVERHEAT()
    {
        OVERHEATED.SetActive(false);
        overheat = false;
        sprite.color = initialColor;
        foreach (var p in fireParticles)
        {
            p.Stop();
        }
        yield return null;
    }

    public void IncreaseHeat(float amount)
    {
        canCoolDown = CoolDown;
        heat += amount;
        heatMeter.fillAmount = heat;
        heatMeter2.fillAmount = heat;
        if (heat >= .25f)
        {
            //unlock dash
            GameManager.gm.ShowTutorial(0);
            dashAllowed = true;
            dashBar.transform.parent.gameObject.SetActive(true);
            dashText.SetActive(true);
        }
        if (heat >= .5f)
        {
            //unlock deflect
            GameManager.gm.ShowTutorial(1);
            deflectAllowed = true;
            deflectText.SetActive(true);
        }
        if (heat >= .75f)
        {
            //unlock discharge
            GameManager.gm.ShowTutorial(2);
            dischargeAllowed = true;
            dischargeBar.transform.parent.gameObject.SetActive(true);
            dischargeText.SetActive(true);
        }
        if (heat >= 1)
        {
            heat = 1;
            if (!overheat)
            {
                GameManager.gm.ShowTutorial(3);
                StartCoroutine(OVERHEAT());
            }
        }
        if(!overheat)
            sprite.color = Color.Lerp(initialColor,endColor,heat);
    }

    public void DecreaseHeat(float amount)
    {
        //canCoolDown = CoolDown;
        heat -= amount;
        if(overheat && heat < loseOverHeat)
        {
            StartCoroutine(LoseOVERHEAT());
        }
        heatMeter.fillAmount = heat;
        heatMeter2.fillAmount = heat;
        if (heat < .25f)
        {
            //lock dash
            dashAllowed = false;
            dashBar.transform.parent.gameObject.SetActive(false);
            dashText.SetActive(false);
        }
        if (heat < .5f)
        {
            //lock deflect
            deflectAllowed = false;
            deflectText.SetActive(false);
        }
        if (heat < .75f)
        {
            //lock discharge
            dischargeAllowed = false;
            dischargeBar.transform.parent.gameObject.SetActive(false);
            dischargeText.SetActive(false);
        }
        if (heat < 0)
        {
            heat = 0;
        }
        if (!overheat)
            sprite.color = Color.Lerp(initialColor, endColor, heat);
    }

    public void TakeKnockback(Vector2 dir, float time)
    {

    }

    public void TakeHeal(float amount){
            if (curHP < maxHP)
            {
                print("heal");
                healParticles.Play();
                curHP += amount;
                hpBar.fillAmount = curHP / maxHP;
                if (curHP >= maxHP)
                {
                    curHP = maxHP;
                }
            }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (attacking)
        {
            if(collision.gameObject.tag == "Box")
            {
                Destroy(collision.gameObject, 1);
                IncreaseHeat(.1f);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Spike")
        {
            TakeDamage(collision.gameObject.GetComponent<Spike>().damage);
        }
        if (attacking)
        {
            if (collision.gameObject.tag == "Box")
            {
                Destroy(collision.gameObject);
                IncreaseHeat(.1f);
            }
        }
    }

    /*
    public void TakeDamage(float damage, bool increaseHeat = true)
    {
        
            if (dmgTime <= 0)
            {
                dmgTime = nextDamage;
                if (increaseHeat)
                {
                    IncreaseHeat(.1f);
                }

                curHP -= damage;
                hpBar.fillAmount = curHP / maxHP;

                if (curHP <= 0)
                {
                    Die();
                }

            }
        
    }
    */
    public IEnumerator TakeDamage(float damage, bool increaseHeat = true, bool showDamage = true, bool shakeScreen = true)
    {
        if (!takingDamage)
        {
            if(shakeScreen)
                CameraShake.cs.cameraShake(.3f, 3f);
            takingDamage = true;
            if (curHP > 0)
            {

                if (increaseHeat)
                {
                    IncreaseHeat(.1f);
                }

                curHP -= damage;
                hpBar.fillAmount = curHP / maxHP;
                if (showDamage)
                {
                    yield return null;
                    sprite.color = Color.red;
                    yield return new WaitForSeconds(0.1f);
                    sprite.color = Color.Lerp(initialColor, endColor, heat);
                }
                if (curHP <= 0)
                {
                    Die();
                }

            }
            if(showDamage)
                yield return new WaitForSeconds(nextDamage - .1f);
            takingDamage = false;
        }
        
    }

    public void Die()
    {
        //show death and loss screen
        GameManager.gm.LoseGame();
    }

    IEnumerator Attack()
    {
        attacking = true;
        spritesAnim.SetTrigger("Attack");
        yield return new WaitUntil(() => spritesAnim.GetCurrentAnimatorStateInfo(0).IsName("Player_Attack"));
        while (spritesAnim.GetCurrentAnimatorStateInfo(0).IsName("Player_Attack")){
            IncreaseHeat(.0025f);
            yield return new WaitForSeconds(.01f);
        }
        attacking = false;
    }
    IEnumerator Discharge()
    {
        dischargeCDLeft = dischargeCD;
        spritesAnim.SetTrigger("Discharge");
        yield return new WaitUntil(() => spritesAnim.GetCurrentAnimatorStateInfo(0).IsName("Player_Discharge"));
        yield return new WaitForSeconds(.3f);
        StartCoroutine(TakeDamage(10f, false));
        IncreaseHeat(0f);
        DecreaseHeat(.1f);
        //SHOOT PROJECTILE
        GameObject o = Instantiate(playerProjectile, shootpoint.position, playerProjectile.transform.rotation);
        o.GetComponent<PlayerProjectile>().Move((int)transform.localScale.x);
        
        //yield return new WaitUntil(() => !spritesAnim.GetCurrentAnimatorStateInfo(0).IsName("Player_Discharge"));

    }
}
