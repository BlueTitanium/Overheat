using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAI : MonoBehaviour
{
    public float aggroRange = 5f;
    public float fireRange = 3f;
    public float hoverDist = 2f;
    public float wanderSpeed = 2f;
    public float aggroSpeed = 2f;
    public string playerTag = "Player";

    public float wanderRange = 1.0f;
    private float wanderX, startX;
    private Vector2 startPosition, targetPosition;

    
    
    public LayerMask ground;
    public Transform elevation;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = new Vector2(transform.position.x,transform.position.y)
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Wander(){
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, wanderSpeed * Time.deltaTime);

        if (transform.position.x == wanderX)
        {
            wanderX = Random.Range(startX - wanderRange, startX + wanderRange);
            targetPosition = new Vector2(wanderX, transform.position.y);
            
        }
    }

    private void Approach(){

    }

    private void Fire(){
        //Fire Bullet Prefab Based on Element
    }

    private Transform findPlayer(radius){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D c in colliders){       
            if (c.tag == "Player"){
                return c.Transform
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
