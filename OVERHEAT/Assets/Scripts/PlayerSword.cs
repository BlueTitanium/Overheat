using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
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
            collision.transform.parent.gameObject.GetComponent<DroneAI>().takeDamage(1f);
            PlayerController.p.IncreaseHeat(0.05f);
        }
        if (collision.gameObject.CompareTag("Slug"))
        {
            collision.transform.parent.gameObject.GetComponent<MeleeAI>().takeDamage(1f);
            PlayerController.p.IncreaseHeat(0.05f);
        }
    }
}
