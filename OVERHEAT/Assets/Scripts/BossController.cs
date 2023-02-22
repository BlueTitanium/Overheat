using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public float maxhealth = 1600f;
    public float health = 600f;
    public float speed = 5f;

    public float outerRange =3f, innerRange = 1f;

    public Animator anim;
    public Rigidbody2D rb;
    public Image bar;
    public AudioSource aud;
    public AudioClip damage;
    public AudioClip attack;
    public LockZoneBoss l;
    public GameObject explosion;

    public PlayerController p;
    private float distToPlayer;
    public Vector2 dir;
    bool spike = false;
    // Start is called before the first frame update
    void Start()
    {
        health = maxhealth;
        
    }

    // Update is called once per frame
    void Update()
    {
        distToPlayer = Vector2.Distance(transform.position, p.transform.position);
        dir = -(transform.position - p.transform.position);
        if(health/maxhealth <= .5)
        {
            speed = 12f;
        }

            if (distToPlayer > outerRange)
        {
            print("outer");

            rb.velocity = new Vector2(dir.normalized.x * speed, 0);
        }
        else if (distToPlayer <= outerRange && distToPlayer > innerRange)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            if(anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "BossSpikeHot")
                rb.velocity = new Vector2(-dir.normalized.x * speed, 0);
        }
        Attack();
        if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "BossSpikeHot")
        {
            rb.velocity = Vector2.zero;
        }
    }
    public void Attack()
    {
        if(anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "BossIdle")
        {
            anim.ResetTrigger("Hot1");
            anim.ResetTrigger("Hot2");
            anim.ResetTrigger("Spike");
            anim.ResetTrigger("Cold");
            if (dir.x <= 0)
            {
                int randomAttack = Random.Range(0, 3);
                switch (randomAttack)
                {
                    case 0:
                        anim.SetTrigger("Hot1");
                        break;
                    case 1:
                        anim.SetTrigger("Hot2"); ;
                        break;
                    case 2:
                        anim.SetTrigger("Spike");
                        break;
                    default:
                        break;
                }
            } else
            {
                int randomAttack = Random.Range(0, 2);
                switch (randomAttack)
                {
                    case 0:
                        anim.SetTrigger("Cold");
                        break;
                    case 1:
                        anim.SetTrigger("Spike");
                        break;
                    default:
                        break;
                }
            }
            
            
        }
    }

    public void AttackSound()
    {
        aud.PlayOneShot(attack);
    }

    public void TakeDamage(float dmg)
    {
        if(!aud.isPlaying)
            aud.PlayOneShot(damage);
        health -= dmg;
        if (health != maxhealth)
        {
            bar.gameObject.SetActive(true);
        }
        bar.fillAmount = health / maxhealth;
        if (health < 0)
        {
            Instantiate(explosion, transform.position, explosion.transform.rotation);
            l.TurnOff();
            Destroy(gameObject);
        }
    }
}
