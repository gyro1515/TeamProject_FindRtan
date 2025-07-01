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
        Debug.Log("StageUnlocked_1: " + PlayerPrefs.GetInt("StageUnlocked_1", 0)); // �߰�
       //�������� 1�� �׻� Ȱ��ȭ
        stage1Button.interactable = true;
      
       //�������� 2�� �ر� ���ο� ���� Ȱ��ȭ
       if(PlayerPrefs.GetInt("StageUnlocked_1",0) == 1)
            stage2Button.interactable = true;
       else stage2Button.interactable = false;
    }

}
