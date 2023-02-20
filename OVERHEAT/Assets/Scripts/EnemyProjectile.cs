using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public string element;
    public float damage = 100f;
    public List<Sprite> sprites;
    private SpriteRenderer sr;

    void Start(){
        sr = gameObject.GetComponent<SpriteRenderer>();
    }
    void Update(){
        VisualUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            if(element == "Hot"){
                print(element);
                PlayerController.p.TDamage(damage);
            }
            if(element == "Cold"){
                PlayerController.p.TDamage(damage,false);
            }
            if(element == "Normal"){
                PlayerController.p.TDamage(damage,false);
            }
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
