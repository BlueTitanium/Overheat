using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AudioSource eruptSound;
    // Start is called before the first frame update
    void Start()
    {
        eruptSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
