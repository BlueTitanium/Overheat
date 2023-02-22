using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    // Start is called before the first frame update
    public float damage = 5f;
    public float dmgInterval = 1.0f;
    public bool canHurt = false;
    public bool isCold = false;
    public float amount = .5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(!isCold)
                PlayerController.p.TDamage(damage);
            else
            {
                PlayerController.p.TDamage(damage,false);
                PlayerController.p.DecreaseHeat(amount);
            }
        }

    }
   // IEnumerator StandingDmg(Collision2D col)
    //{
       // yield return new WaitForSeconds(dmgInterval);
        //Debug.Log("hurt");

    //}
}
