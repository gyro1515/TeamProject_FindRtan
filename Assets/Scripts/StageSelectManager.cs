using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour
{
    public Button stage1Button;
    public Button stage2Button;

    void Start()
    {
        RefreshButtonState();// ���� ���۵ɶ� ��ư ���� ����
              
    }
    public void RefreshButtonState()
    {
        //Debug.Log("StageUnlocked_1: "+PlayerPrefs.GetInt("StageUnlocked_1", 0));
        //1. stage1��ư�� �׻��̰�
        stage1Button.gameObject.SetActive(true);
        stage1Button.interactable = true;
        stage1Button.onClick.RemoveAllListeners();
        stage1Button.onClick.AddListener(() => LoadStage(1));
        //2.PlayerPrefs�κ��� stage2 �ر� ���� Ȯ��
        bool isStage2Unlocked = PlayerPrefs.GetInt("StageUnlocked_1", 0) ==  1;
        Debug.Log("Stage2 Unlocked?" + isStage2Unlocked);

        
        if (PlayerPrefs.GetInt("StageUnlocked_1", 0) == 1)
        {
            // �رݵ� ���: ���̰� Ŭ�������ϰ�
            stage2Button.gameObject.SetActive(true);
            stage2Button.interactable = true;
            stage2Button.onClick.AddListener(() => LoadStage(2)); 
            //(����) Ŭ�� ������ ���
            // stage2Button.onClick.RemoveAllListener();
            // stage2Button.onClick.AddListener(() => LoadStage(2));
        }
        else
        {
            //��ݵ� ���: �ƿ�����
            stage2Button.gameObject.SetActive(false);
        }
        
    }
    //(����) �μ��� �� �ε��� �޼���
    public void LoadStage(int stageNumber)
    {
        Time.timeScale = 1f;
        GameManager.instance.currentStageIndex = stageNumber -1;
        SceneManager.LoadScene("MainScene" + stageNumber);
    }
}
