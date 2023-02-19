using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAI : MonoBehaviour
{
    public float aggroRange = 5f;
    public float fireRange = 3f;
    public float hoverDist = 2f;
    public float wanderSpeed = 3f;
    public float aggroSpeed = 2f;
    public string playerTag = "Player";

    public float wanderRange = 5f;
    private float wanderX, startX;
    private Vector2 startPosition, wanderPosition,targetPosition;

    public LayerMask ground;
    public Transform elevation;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        Transform player = findPlayer(aggroRange);
        if(player){
            Approach(player,fireRange);
        } else{
            Wander();
        }
            
    }

    //moving drone + tilt and face direction
    private Vector2 DroneMove(Vector3 target, float speed){
        Vector2 str = transform.position;
        Vector2 dir = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        transform.localScale = new Vector3(Mathf.Sign(dir.x-str.x), 1, 1);
        return dir;
    }

    private void Wander(){
        transform.position = DroneMove(wanderPosition,wanderSpeed);

        if (transform.position.x == wanderX)
        {
            wanderX = Random.Range(startX - wanderRange, startX + wanderRange);
            wanderPosition = new Vector2(wanderX, transform.position.y);
        }
    }

    private void Approach(Transform target, float distance){
        Vector2 direction = target.position - transform.position;
        targetPosition = (Vector2)target.position - direction.normalized * distance;
        transform.position = DroneMove(targetPosition,aggroSpeed);

        if((transform.position - targetPosition).magnitude < 0.01f){
            Fire();
        }
    }

    private void Fire(){
        //Fire Bullet Prefab Based on Element
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

    private bool CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(elevation.position, Vector2.down, hoverDist, ground);
        return hit.collider != null;
    }

    
}
