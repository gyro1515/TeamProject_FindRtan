using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonScripts : MonoBehaviour
{
    public void SelectToStage()
    {
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

    // �߰�: ���� ���������� �̵�
    public void GoToNextStage()
    {
        Time.timeScale = 1.0f;
        // GameManager�� currentStageIndex �� ���
        int nextStage = GameManager.instance.currentStageIndex + 1;
        string nextSceneName = "MainScene" + nextStage;
        SceneManager.LoadScene(nextSceneName);
    }
    //GameOverScene���� �̵�
    public void GoToGameOverScene()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    public void GoToStageSelect()
    {
        // ���� �ر� ���� ����
        PlayerPrefs.DeleteKey("StageUnlocked_1");
        PlayerPrefs.Save();

        Time.timeScale = 1.0f; //Ȥ�� �����ִٸ� �簳
        SceneManager.LoadScene("StageScene");
    }

}
