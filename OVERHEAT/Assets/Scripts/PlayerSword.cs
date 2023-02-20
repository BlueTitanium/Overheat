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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided with " + collision.gameObject.name);
        if(collision.gameObject.tag == "Box")
        {
            Destroy(collision.gameObject);
            PlayerController.p.IncreaseHeat(0.05f);
        }
    }
}
