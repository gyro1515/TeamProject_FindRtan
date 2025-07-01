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
        RefreshButtonState();// 씬이 시작될때 버튼 상태 갱신
              
    }
    public void RefreshButtonState()
    {
        //Debug.Log("StageUnlocked_1: "+PlayerPrefs.GetInt("StageUnlocked_1", 0));
        //1. stage1버튼은 항상보이게
        stage1Button.gameObject.SetActive(true);
        stage1Button.interactable = true;
        stage1Button.onClick.RemoveAllListeners();
        stage1Button.onClick.AddListener(() => LoadStage(1));
        //2.PlayerPrefs로부터 stage2 해금 여부 확인
        bool isStage2Unlocked = PlayerPrefs.GetInt("StageUnlocked_1", 0) ==  1;
        Debug.Log("Stage2 Unlocked?" + isStage2Unlocked);

        
        if (PlayerPrefs.GetInt("StageUnlocked_1", 0) == 1)
        {
            // 해금된 경우: 보이고 클릭가능하게
            stage2Button.gameObject.SetActive(true);
            stage2Button.interactable = true;
            stage2Button.onClick.AddListener(() => LoadStage(2)); 
            //(선택) 클릭 리스너 등록
            // stage2Button.onClick.RemoveAllListener();
            // stage2Button.onClick.AddListener(() => LoadStage(2));
        }
        else
        {
            //잠금된 경우: 아예숨김
            stage2Button.gameObject.SetActive(false);
        }
        
    }
    //(선택) 인수로 씬 로드할 메서드
    public void LoadStage(int stageNumber)
    {
        Time.timeScale = 1f;
        GameManager.instance.currentStageIndex = stageNumber -1;
        SceneManager.LoadScene("MainScene" + stageNumber);
    }
}
