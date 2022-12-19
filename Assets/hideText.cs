using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class hideText : MonoBehaviour
{
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;

    private float timer = 0f;

    private void FixedUpdate()
    {
        if (timer >= 5)
        {
            Destroy(text1);
        }
        else
        {
            timer += Time.fixedDeltaTime;
        }

        if (Input.GetKey(KeyCode.H))
        {
            Destroy(text2);
        }
        
        
    }
}
