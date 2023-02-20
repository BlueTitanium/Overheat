using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb2d;
    public GameObject explosion;
    public float damage = 35f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3f);
    }

    public void Move(int sign)
    {
        transform.localScale = new Vector3(sign, 1, 1);
        rb2d.velocity = new Vector2(sign * speed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Instantiate(explosion, transform.position, explosion.transform.rotation);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Box"))
        {
            Instantiate(explosion, transform.position, explosion.transform.rotation);
            collision.gameObject.GetComponent<Box>().startDestroy = true;
        }
        if (collision.gameObject.CompareTag("Drone"))
        {
            Instantiate(explosion, transform.position, explosion.transform.rotation);
            collision.transform.parent.gameObject.GetComponent<DroneAI>().takeDamage(damage);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Slug"))
        {
            Instantiate(explosion, transform.position, explosion.transform.rotation);
            collision.transform.parent.gameObject.GetComponent<MeleeAI>().takeDamage(damage);
            Destroy(gameObject);
        }
    }
}
