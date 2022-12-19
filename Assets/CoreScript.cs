using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoreScript : MonoBehaviour
{
    public static int coreHealth;
    public TextMeshProUGUI coreHealthUI;

    void Start()
    {
        coreHealth = 100;
    }
    void Update()
    {
        coreHealthUI.text = coreHealth.ToString();
        if (coreHealth <= 0)
        {
            coreHealth = 0;
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        print(col.collider.tag);
        if (col.collider.tag == "Bullet")
        {
            coreHealth -= 5;
        }
        else if (col.collider.tag == "RPG")
        {
            coreHealth -= 20;
        }
    }
}
