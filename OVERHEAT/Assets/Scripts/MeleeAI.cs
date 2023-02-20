using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeleeAI : MonoBehaviour
{
    public float aggroRange = 5f;
    public float wanderSpeed = 3f;
    public float aggroSpeed = 2f;
    public float wanderRange = 5f;
    public string playerTag = "Player";
    private string state;

    public float damage = 10f;
    
    private float wanderX, startX, BobY;
    
    public float maxhealth = 10;
    private float health;
    public Slider bar;
    private Vector2 wanderPosition,targetPosition;
    public LayerMask ground;

    void Start()
    {
        wanderPosition = new Vector2(transform.position.x, transform.position.y);
        wanderX = transform.position.x;
        startX = transform.position.x;
        state = "Wander";
        health = maxhealth;
    }

    // Update is called once per frame
    void Update()
    {
        Transform player = findPlayer(aggroRange);
        if(player){
            state = "Aggro";
            Approach(player);
        } else{
            state = "Wander";
            Wander();
        }
    }

    //moving drone + tilt and face direction
    private Vector2 MeleeMove(Vector3 target, float speed, Vector2 aim){
        Vector2 str = transform.position;
        Vector2 dir = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        
        if(state != "Aggro"){
            transform.localScale = new Vector3(Mathf.Sign(dir.x-str.x), 1, 1);
        } else{
            transform.localScale = new Vector3(Mathf.Sign(aim.x-str.x), 1, 1);
        }
        bar.transform.localScale = transform.localScale;
        return dir;
    }

    private void Wander(){
        transform.position = MeleeMove(wanderPosition,wanderSpeed,Vector2.zero);

        if (transform.position.x == wanderX)
        {
            wanderX = Random.Range(startX - wanderRange, startX + wanderRange);
            wanderPosition = new Vector2(wanderX, transform.position.y);
        }
    }

    private void Approach(Transform target){
        Vector2 direction = target.position - transform.position;
        targetPosition = (Vector2)target.position - direction.normalized;
        transform.position = MeleeMove(targetPosition,aggroSpeed, (Vector2)target.position);
    }

    public void takeDamage(float dmg){
        health -= dmg;
        if(health != maxhealth){
            bar.gameObject.SetActive(true);
        }
        bar.value = health/maxhealth;
        if(health < 0){
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(PlayerController.p.TakeDamage(damage));
        }
    }

    private Transform findPlayer(float radius){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D c in colliders){       
            if (c.tag == playerTag){
                return c.transform;
            }
        }
        return null;
    }
}
