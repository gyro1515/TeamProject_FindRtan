using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeManager : MonoBehaviour
{
    public static ChallengeManager instance;

    private bool Lose = false;
    private bool Win = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void OnGameFailed()
    {
        if (!Lose)
        {
            Lose = true;
            Debug.Log("패배도 경험이다");
        }
    }
    public void OnGameClearedEarly(float timeLeft)
    {
        float elapsedTime = 30f - timeLeft;
        if (elapsedTime <= 10f && !Win)
        {
            Win = true;
            Debug.Log("인공지능 플레이어");
        }
    }
   
}