using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    public GameObject parryParticles;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            collision.gameObject.GetComponent<Box>().startDestroy = true;
            PlayerController.p.IncreaseHeat(0.05f);
        }
        if (collision.gameObject.CompareTag("Drone"))
        {
            float totalDamage = (!PlayerController.p.overheat) ? PlayerController.p.baseDamage + PlayerController.p.heat * PlayerController.p.damageIncrement : PlayerController.p.baseDamage + 2 * PlayerController.p.damageIncrement;
            collision.gameObject.GetComponent<DroneAI>().takeDamage(totalDamage);
            PlayerController.p.IncreaseHeat(0.05f);
        }
        if (collision.gameObject.CompareTag("Slug"))
        {
            float totalDamage = (!PlayerController.p.overheat) ? PlayerController.p.baseDamage + PlayerController.p.heat * PlayerController.p.damageIncrement : PlayerController.p.baseDamage + 2 * PlayerController.p.damageIncrement;
            collision.gameObject.GetComponent<MeleeAI>().takeDamage(totalDamage);
            PlayerController.p.IncreaseHeat(0.05f);
        }
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            if (PlayerController.p.deflectAllowed)
            {
                collision.GetComponent<Rigidbody2D>().velocity = -collision.GetComponent<Rigidbody2D>().velocity;
                collision.GetComponent<EnemyProjectile>().isByPlayer = true;
                Instantiate(parryParticles, collision.transform.position, parryParticles.transform.rotation);
            }
        }
    }
}
