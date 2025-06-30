using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonScripts : MonoBehaviour
{

    public void StartToMain()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void MainToGameOver()
    {
        SceneManager.LoadScene("GameOverScene");
    }
    public void LoadRetryScene()
    {
        SceneManager.LoadScene("StartScene");
    }
        
    




}
