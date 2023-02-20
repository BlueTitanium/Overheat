using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockZone : MonoBehaviour
{
    public GameObject tileBarrier;
    public List<GameObject> enemiesInside = new List<GameObject>();
    public GameObject playerIn;
    // Start is called before the first frame update
    void Start()
    {
        tileBarrier.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (tileBarrier.activeSelf == true && (enemiesInside.Count == 0 || playerIn == null))
        {
            tileBarrier.SetActive(false);
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        print("Trigger Detected " + other.gameObject.CompareTag("Player"));
        if (other.gameObject.CompareTag("Player"))
        {
            playerIn = other.gameObject;
            tileBarrier.SetActive(true);
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
