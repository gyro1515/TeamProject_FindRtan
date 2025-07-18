using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonScripts : MonoBehaviour
{
    public void SelectToStage()
    {
        // 선택 패널 비활성화
        GameManager.instance.selectPanel.SetActive(false);
        SceneManager.LoadScene("StageScene");
    }
    public void RetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void StartToMain1()
    {
        SceneManager.LoadScene("MainScene1");
    }
    public void StartToMain2()
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
        SceneManager.LoadScene("MainScene2");
    }
    //GameOverScene으로 이동
    public void GoToGameOverScene()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    public void GoToStageSelect()
    {
        // 기존 해금 정보 삭제
        PlayerPrefs.DeleteKey("StageUnlocked_1");
        PlayerPrefs.Save();

        Time.timeScale = 1.0f; //혹시 멈춰있다면 재개
        SceneManager.LoadScene("StageScene");
    }

}
