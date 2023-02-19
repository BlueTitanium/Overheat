using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    // Start is called before the first frame update
    public float damage = 5f;
    public float dmgInterval = 1.0f;
    public bool canHurt = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if(collision.gameObject.name == "Player")
        //{
        //    collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        //}
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        //StartCoroutine(StandingDmg(collision));
    }

   // IEnumerator StandingDmg(Collision2D col)
    //{
       // yield return new WaitForSeconds(dmgInterval);
        //Debug.Log("hurt");

    //}
}
