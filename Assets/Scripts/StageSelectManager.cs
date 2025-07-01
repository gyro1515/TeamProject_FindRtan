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
        Debug.Log("StageUnlocked_1: " + PlayerPrefs.GetInt("StageUnlocked_1", 0)); // 추가
       //스테이지 1은 항상 활성화
        stage1Button.interactable = true;
      
       //스테이지 2는 해금 여부에 따라 활성화
       if(PlayerPrefs.GetInt("StageUnlocked_1",0) == 1)
            stage2Button.interactable = true;
       else stage2Button.interactable = false;
    }

}
