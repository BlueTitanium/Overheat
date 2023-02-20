using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public string element;
    public float damage = 100f;
    public List<Sprite> sprites;
    private SpriteRenderer sr;
    public bool isByPlayer = false;
    public GameObject explosion;

    void Start(){
        sr = gameObject.GetComponent<SpriteRenderer>();
        Destroy(gameObject, 3f);
    }
    void Update(){
        VisualUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground"))
        {
            Instantiate(explosion, transform.position, explosion.transform.rotation);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Player") && !isByPlayer)
        {
            
            if (element == "Hot"){
                print(element);
                PlayerController.p.TDamage(damage);
            }
            if(element == "Cold"){
                PlayerController.p.TDamage(damage,false);
            }
            if(element == "Normal"){
                PlayerController.p.TDamage(damage,false);
            }
            Instantiate(explosion, transform.position, explosion.transform.rotation);
            Destroy(gameObject);

        }
        if (collision.gameObject.CompareTag("Drone") && isByPlayer)
        {
            
            collision.gameObject.GetComponent<DroneAI>().takeDamage(damage);
            Instantiate(explosion, transform.position, explosion.transform.rotation);
            Destroy(gameObject);

        }
        if (collision.gameObject.CompareTag("Slug") && isByPlayer)
        {
            

            collision.gameObject.GetComponent<MeleeAI>().takeDamage(damage);
            Instantiate(explosion, transform.position, explosion.transform.rotation);
            Destroy(gameObject);
        }
    }

    private void VisualUpdate(){
        if(element == "Hot"){
            sr.sprite = sprites[0];
        }
        if(element == "Cold"){
            sr.sprite = sprites[1];
        }
        if(element == "Normal"){
            sr.sprite = sprites[2];
        }
    }
}
