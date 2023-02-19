using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAI : MonoBehaviour
{
    public float aggroRange = 5f;
    public float fireRange = 3f;
    public float wanderSpeed = 3f;
    public float aggroSpeed = 2f;
    public float wanderRange = 5f;
    public float fireLeeway = 0.5f;
    public float fireCooldown = 4f;
    private float timeToFire = 0f;
    public float fireSpeed = 5f;

    public string playerTag = "Player";
    public string element = "Hot";
    private string state;

    public float hoverDist = 2f;

    private float wanderX, startX;
    
    private Vector2 startPosition, wanderPosition,targetPosition;
    
    public Transform suspend,firePoint;
    public GameObject hot,cold;
    public LayerMask ground;
    
    
    // Start is called before the first frame update
    void Start()
    {
        startPosition = new Vector2(transform.position.x, transform.position.y);
        state = "Wander";
    }

    // Update is called once per frame
    void Update()
    {
        Transform player = findPlayer(aggroRange);
        if(player){
            state = "Aggro";
            Approach(player,fireRange);
        } else{
            state = "Wander";
            Wander();
        }
        timeToFire -= Time.deltaTime; 
            
    }

    //moving drone + tilt and face direction
    private Vector2 DroneMove(Vector3 target, float speed, Vector2 aim){
        Vector2 str = transform.position;
        Vector2 dir = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if(state != "Aggro"){
            transform.localScale = new Vector3(Mathf.Sign(dir.x-str.x), 1, 1);
        } else{
            transform.localScale = new Vector3(Mathf.Sign(aim.x-str.x), 1, 1);
        }
        return dir;
    }

    private void Wander(){
        transform.position = DroneMove(wanderPosition,wanderSpeed,Vector2.zero);

        if (transform.position.x == wanderX)
        {
            wanderX = Random.Range(startX - wanderRange, startX + wanderRange);
            wanderPosition = new Vector2(wanderX, transform.position.y);
        }
    }

    private void Approach(Transform target, float distance){
        Vector2 direction = target.position - transform.position;
        targetPosition = (Vector2)target.position - direction.normalized * distance;
        transform.position = DroneMove(targetPosition,aggroSpeed, (Vector2)target.position);

        if(((Vector2)transform.position - (Vector2)targetPosition).magnitude < fireLeeway && timeToFire < 0f){
            timeToFire = fireCooldown;
            Fire(target);
        }
    }

    private void Fire(Transform target){
        Vector2 direction = target.position - firePoint.position;
        GameObject projectile;
        if(element == "Hot"){
            projectile = Instantiate(hot, firePoint.position, Quaternion.identity);
        } else{
            projectile = Instantiate(cold, firePoint.position, Quaternion.identity);
        }
        projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction)* Quaternion.Euler(0, 0, 90);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        projectileRb.velocity = direction.normalized * fireSpeed;
    }

    private Transform findPlayer(float radius){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D c in colliders){       
            print(c.tag);
            if (c.tag == playerTag){
                return c.transform;
            }
        }
        return null;
    }

    // private bool CheckGrounded()
    // {
    //     RaycastHit2D hit = Physics2D.Raycast(elevation.position, Vector2.down, hoverDist, ground);
    //     return hit.collider != null;
    // }

    
}
