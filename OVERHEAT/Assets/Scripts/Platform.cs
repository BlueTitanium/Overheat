using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // Start is called before the first frame update
    public float amount;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        /*
        if (collision.gameObject.name == "Player")
        {
            if(amount > 0)
                collision.gameObject.GetComponent<PlayerController>().IncreaseHeat(amount);
            else
                collision.gameObject.GetComponent<PlayerController>().DecreaseHeat(-1* amount);
        }*/

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerLegs"))
        {
            if (amount > 0)
                PlayerController.p.IncreaseHeat(amount);
            else
                PlayerController.p.DecreaseHeat(-1 * amount);
        }

    }
}
