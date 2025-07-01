using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonScripts : MonoBehaviour
{
    
    public void StartToMain1()
    {
        SceneManager.LoadScene("MainScene1");
    }
    public void StatrToMain2()
    {
        SceneManager.LoadScene("MainScene2");
    }
   
    public void MainToGameOver()
    {
        SceneManager.LoadScene("GameOverScene");
    }
    public void LoadRetryScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    // �߰�: ���� ���������� �̵�
    public void GoToNextStage()
    {
        // GameManager�� currentStageIndex �� ���
        int nextStage = GameManager.instance.currentStageIndex + 1;
        string nextSceneName = "MainScene" + nextStage;
        SceneManager.LoadScene(nextSceneName);
    }




}
