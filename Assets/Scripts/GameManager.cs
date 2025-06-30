using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    public Card firstCard;
    public card SecondCard;
    public Text timeTxt;
    public gameobject endTxt;
    public int cardCount = 0;
    float time = 0.0f;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    void Update()
    {
        time += time.deltaTime;
        timeTxt.text = time.ToString("N2");
    }

    public void Matched()
    {
        if (firstCard.idx == SecondCard.idx)
        {
            firstCard.DestroyCard();
            SecondCard.DestroyCard();
            cardCount -= 2;
            if (cardCount == 0)
            {
                Time.timeScale = 0.0f;
                endTxt.SetActive(false);
            }

        }
        else
        {
            firstCard.CloseCard();
            SecondCard.CloseCard();


        }

        firstCard = null;
        SecondCard = null;

    }

    
    
}