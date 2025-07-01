using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Burst.Intrinsics;

public class GameManager : MonoBehaviour
{
    public Button Retry;
    public static GameManager instance;


    public enum GameProgress
    {
        EndGame, StartGame, Failed
    }

    public GameProgress progress = GameProgress.StartGame;
    public Card firstCard;
    public Card secondCard;
    public Text timeTxt;
    public GameObject endTxt;
    public Text ComboTxt;
    public int Combo = 0;
    public int cardCount = 10;

    float time = 30.0f;
    bool gameOverTriggered = false;


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

                if (!gameOverTriggered)
                {
                    gameOverTriggered = true;
                    Invoke("GoToGameOver", 1.0f);
                }
                break;

            case GameProgress.StartGame:
                time -= Time.deltaTime;
                if (time <= 0.0f)
                {
                    time = 0.0f;
                    progress = GameProgress.Failed;
                    ChallengeManager.instance.OnGameFailed();
                }
                timeTxt.text = time.ToString("N2");
                break;

            case GameProgress.Failed:
                Retry.gameObject.SetActive(true);
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

            Debug.Log($"���� ī�� ��: {cardCount}");
            
            
                Combo++;
            ComboTxt.text = Combo.ToString();
            

            if (cardCount == 0)
            {
                progress = GameProgress.EndGame;
                ChallengeManager.instance.OnGameClearedEarly(time);
                endTxt.SetActive(false);
            }
        }
        else
        {
            firstCard.CloseCard();
            secondCard.CloseCard();
            Combo = 0;
            ComboTxt.text = Combo.ToString();
        }

        firstCard = null;
        secondCard = null;
    }

    void GoToGameOver()
    {
        SceneManager.LoadScene("GameOverScene");
    }
}
