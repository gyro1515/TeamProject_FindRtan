using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScripts : MonoBehaviour
{
    public RectTransform creditText;
    public float speed = 130f;
    public float endY = 1500f;
    void Update()
    {
        if (creditText.anchoredPosition.y < endY)
        {
            creditText.anchoredPosition += new Vector2(0, speed * Time.deltaTime);

        }
        else
        {

            SceneManager.LoadScene("StartScene");
        }
        
    }
}
