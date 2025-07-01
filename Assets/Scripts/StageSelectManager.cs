using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectManager : MonoBehaviour
{
    public Button stage1Button;
    public Button stage2Button;

    void Start()
    {
        RefreshButtonState();// 씬이 시작될때 버튼 상태 갱신
        Debug.Log("StageUnlocked_1: " + PlayerPrefs.GetInt("StageUnlocked_1", 0)); // 추가
       
    }
    public void RefreshButtonState()
    {
        Debug.Log("StageUnlocked_1: "+PlayerPrefs.GetInt("StageUnlocked_1", 0));
        stage1Button.interactable = true;
        if (PlayerPrefs.GetInt("StageUnlocked_1", 0) == 1)
        {
            stage2Button.gameObject.SetActive(true);
            stage2Button.interactable = true;
        }
        else stage2Button.interactable = false;
        
    }
}
