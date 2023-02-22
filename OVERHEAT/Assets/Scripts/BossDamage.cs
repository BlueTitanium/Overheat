using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamage : MonoBehaviour
{
    bool showingDamage = false;
    public BossController b;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerController.p.overheat)
            {
                TakeDamage(1f);
            }
        }
    }
    public void TakeDamage(float dmg)
    {
        CameraShake.cs.cameraShake(.3f, 3f);
        DamageText.d.SpawnText(transform.position, dmg);
        b.TakeDamage(dmg);
    }
    public IEnumerator showDamage()
    {
        if (!showingDamage)
        {
            showingDamage = true;
            GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(.1f);
            GetComponent<SpriteRenderer>().color = Color.white;
            showingDamage = false;
        }
        
    }
}
