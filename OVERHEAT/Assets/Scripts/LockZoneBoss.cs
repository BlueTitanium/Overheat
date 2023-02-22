using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockZoneBoss : MonoBehaviour
{
    public GameObject tileBarrier;
    public GameObject boss;
    public List<GameObject> enemiesInside = new List<GameObject>();
    bool hasBossSpawned = false;
    public GameObject playerIn;
    public AudioClip bossMusic;
    // Start is called before the first frame update
    void Start()
    {
        tileBarrier.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (tileBarrier.activeSelf == true && (enemiesInside.Count == 0) && hasBossSpawned)
        {
            TurnOff();
        }

    }

    public void TurnOff()
    {

        tileBarrier.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        print("Trigger Detected " + other.gameObject.CompareTag("Player"));
        if (other.gameObject.CompareTag("Player") && !hasBossSpawned)
        {
            playerIn = other.gameObject;
            tileBarrier.SetActive(true);
            boss.SetActive(true);
            enemiesInside.Add(boss);
            hasBossSpawned = true;
            GameManager.gm.BGM.clip = bossMusic;
            GameManager.gm.BGM.volume = .3f;
            GameManager.gm.BGM.loop = true;
            GameManager.gm.BGM.Play();

        }
        else if ((other.gameObject.CompareTag("Drone") || other.gameObject.CompareTag("Slug")) && (!enemiesInside.Contains(other.gameObject)))
        {
            print("Count " + enemiesInside.Count);
            enemiesInside.Add(other.gameObject);
            if (playerIn != null)
            {
                tileBarrier.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (enemiesInside.Contains(other.gameObject))
        {
            enemiesInside.Remove(other.gameObject);
        }
        if (other.gameObject.CompareTag("Player"))
        {
            playerIn = null;
        }
    }
}
