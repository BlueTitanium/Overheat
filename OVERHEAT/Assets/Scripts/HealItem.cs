using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : MonoBehaviour
{
    public float amount = 25f;

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
        if (collision.gameObject.CompareTag("Player"))
        {
            if(PlayerController.p.curHP != PlayerController.p.maxHP)
            {
                PlayerController.p.TakeHeal(amount);
                Destroy(gameObject);
            }
            
        }
    }
}
