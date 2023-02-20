using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    // Start is called before the first frame update
    SpriteRenderer boxSprite;
    public bool startDestroy = false;
    void Start()
    {
        boxSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (startDestroy)
        {
            boxSprite.color = new Color(boxSprite.color.r, boxSprite.color.g, boxSprite.color.b, boxSprite.color.a - 0.04f);
        }

        if (boxSprite.color.a < 0.3f)
        {
            Destroy(gameObject);
        }
    }
}
