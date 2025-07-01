using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Button Retry;
    public static GameManager instance;

    public enum GameProgress
    {
        EndGame, StartGame, Failed, NextStage
    }
    //�������� ���� �߰�
    public int currentStageIndex = 0; // 0������ ����, ���� ���� ������ �°�
    public int totalStageCount = 2; // ��ü �������� ����
    //panel ���� �߰�
    public GameObject selectPanel; // �ν����Ϳ��� �������� �ǳ� ������Ʈ �Ҵ�

    public GameProgress progress = GameProgress.StartGame;
    public Card firstCard;
    public Card secondCard;
    public Text timeTxt;
    public GameObject endTxt;
    public int cardCount = 10;
    float time = 30.0f;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    void Update()
    {
        switch (progress)
        {
            case GameProgress.EndGame:
                SceneManager.LoadScene("GameOverScene");
                Debug.Log("End");
                break;
            case GameProgress.StartGame:
                time -= Time.deltaTime;
               
                if(time <= 0.0f)
                {
                    time = 0.0f;
                    progress = GameProgress.Failed;
                }
                timeTxt.text = time.ToString("N2");
                break;
            case GameProgress.Failed:   
                Retry.gameObject.SetActive(true);
                break;
            case GameProgress.NextStage:
                //���� �������� �̸��� "MainScene1", "MainScene2"
                int nextStage = currentStageIndex + 1;
                string nextSceneName = "MainScene"+ nextStage;
                SceneManager.LoadScene(nextSceneName);
                break;
            default:
                break;
        }

    }

    public void Matched()
    {
        if (firstCard.idx == secondCard.idx)
        {
            firstCard.DestroyCard();
            secondCard.DestroyCard();
            cardCount -= 2;
            Debug.Log($"End{cardCount}");
            if (cardCount == 0)
            {
                // ������ ������������ üũ
                progress = GameProgress.NextStage;
                // ���� �������� �ر�
                PlayerPrefs.SetInt("StageUnlocked_" + (currentStageIndex + 1), 1);
                PlayerPrefs.Save();
                // Ŭ���� �ǳ� Ȱ��ȭ
                if (selectPanel != null) selectPanel.SetActive(true);
                //���� ������ ���߰� ������
                Time.timeScale = 0.0f;
            }
            
        }
        else
        {
            firstCard.CloseCard();
            secondCard.CloseCard();


        }

        firstCard = null;
        secondCard = null;

    }
    
    
    public bool IsStageUnlocked(int stageIndex)
    {
        if (stageIndex == 0) return true;
        return PlayerPrefs.GetInt("StageUnlocked_"+ stageIndex, 0) == 1;
    }
    
}