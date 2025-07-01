using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour
{
    [System.Serializable]
    public struct StageButton
    {
        public Button button;
        public int stageNumber;
    }

    public StageButton[] stageButtons;

    void Start()
    {
        RefreshButtonState();
    }

    public void RefreshButtonState()
    {
        foreach (var sb in stageButtons)
        {
            bool unlocked = (sb.stageNumber == 1)
                || PlayerPrefs.GetInt("StageUnlocked_" + (sb.stageNumber - 1), 0) == 1;

            // ���̱�/�����
            sb.button.gameObject.SetActive(unlocked);
            sb.button.interactable = unlocked;

            if (unlocked)
            {
                // Ŭ�� ������ ��� (�ߺ� ����)
                sb.button.onClick.RemoveAllListeners();
                sb.button.onClick.AddListener(() => LoadStage(sb.stageNumber));
            }
        }
    }

    public void LoadStage(int stageNumber)
    {
        Time.timeScale = 1f;
        GameManager.instance.currentStageIndex = stageNumber - 1;
        SceneManager.LoadScene("MainScene" + stageNumber);
    }

}
