using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    public TextMeshProUGUI pointsText;
    
    public void Setup(int score)
    {
        gameObject.SetActive(true); 
        pointsText.text = "Score:" + score.ToString();
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
