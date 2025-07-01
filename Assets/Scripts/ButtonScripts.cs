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

    // 추가: 다음 스테이지로 이동
    public void GoToNextStage()
    {
        Time.timeScale = 1.0f;
        // GameManager의 currentStageIndex 값 사용
        int nextStage = GameManager.instance.currentStageIndex + 1;
        string nextSceneName = "MainScene" + nextStage;
        SceneManager.LoadScene(nextSceneName);
    }
    //GameOverScene으로 이동
    public void GoToGameOverScene()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    public void GoToStageSelect()
    {
        Time.timeScale = 1.0f; //혹시 멈춰있다면 재개
        SceneManager.LoadScene("StageScene");
    }

}
