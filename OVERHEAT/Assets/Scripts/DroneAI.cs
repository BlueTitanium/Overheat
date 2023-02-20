using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public float bobSpeed = 2f;
    public float bobHeight = 0.5f;

    public float hoverDist = 2f;
    private bool recalc = false;

    private float wanderX, startX, BobY;
    
    public float maxhealth = 10;
    private float health;
    public Slider bar;
    private Vector2 wanderPosition,targetPosition;
    public Animator anim;
    public Transform firePoint,visuals;
    public GameObject bulletPrefab;
    public LayerMask wall;

    void Start()
    {
        wanderPosition = new Vector2(transform.position.x, transform.position.y);
        wanderX = transform.position.x;
        startX = transform.position.x;
        BobY = transform.position.y;
        state = "Wander";
        health = maxhealth;
    }

    private void VisualUpdate(){
        if(element == "Hot"){
            anim.SetInteger("State",0);
        }
        if(element == "Cold"){
            anim.SetInteger("State",1);
        }
        if(element == "Normal"){
            anim.SetInteger("State",2);
        }
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
        Bob();
        VisualUpdate();
        print(gameObject.name + state);
    }

    private void Bob(){
        float y = BobY + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    //moving drone + tilt and face direction
    private Vector2 DroneMove(Vector3 target, float speed, Vector2 aim){
        Vector2 str = transform.position;
        Vector2 dir = transform.position;
        Collider2D[] c = Physics2D.OverlapCircleAll(transform.position, 1f,wall);
        print(c.Length);
        if(c.Length != 0){
            recalc = true;
        } else{
            dir = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        
        if(state != "Aggro"){
            transform.localScale = new Vector3(Mathf.Sign(dir.x-str.x), 1, 1);
        } else{
            transform.localScale = new Vector3(Mathf.Sign(aim.x-str.x), 1, 1);
        }
        bar.transform.localScale = transform.localScale;
        return dir;
    }

    private void Wander(){
        transform.position = DroneMove(wanderPosition,wanderSpeed,Vector2.zero);

        if (transform.position.x == wanderX || recalc)
        {
            wanderX = Random.Range(startX - wanderRange, startX + wanderRange);
            wanderPosition = new Vector2(wanderX, transform.position.y);
            recalc = false;
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.p.TDamage(1f);
        }
    }

    private void Fire(Transform target){
        Vector2 direction = target.position - firePoint.position;
        GameObject projectile = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);;
        projectile.GetComponent<EnemyProjectile>().element = element;
        projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction)* Quaternion.Euler(0, 0, 90);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        projectileRb.velocity = direction.normalized * fireSpeed;
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
