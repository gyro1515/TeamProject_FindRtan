using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
                timeTxt.text = "시간 초과!";
                break;
            default:
                break;
        }
        if (time <= 5f)
        {
            timeTxt.color = Color.red;
        }
        else if (time <= 13f)
        {
            timeTxt.color = new Color(1f, 0.5f, 0f);
        }
        else
        {
            timeTxt.color = Color.black;
        }

    }

    public void Matched()
    {
        if (firstCard.idx == secondCard.idx)
        {
            firstCard.DestroyCard();
            secondCard.DestroyCard();
            cardCount -= 2;
            if (cardCount == 0)
            {
                progress = GameProgress.EndGame;
                //Time.timeScale = 0.0f;
                endTxt.SetActive(false);
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

    
    
}