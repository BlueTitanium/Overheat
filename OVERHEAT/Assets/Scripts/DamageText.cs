using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public GameObject playerText;
    public static DamageText d;
    public GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        d = this;
    }


    public void SpawnText(Vector2 pos, float num, bool isPlayers = false, bool hasOffset = true)
    {
        Vector2 offset = new Vector2(0, 0);
        if (hasOffset)
        {
            offset = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f) + 1);
        }
        GameObject t;
        if (isPlayers) {
            t = Instantiate(playerText, pos + offset, text.transform.rotation);
        }
        else {
            t = Instantiate(text, pos + offset, text.transform.rotation);
        }
        
        var texts = t.transform.GetComponentsInChildren<TextMeshProUGUI>();
        foreach(var a in texts)
        {
            a.text = "" + (int)num;
        }
    }
}
