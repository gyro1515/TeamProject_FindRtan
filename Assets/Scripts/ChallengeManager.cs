using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
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
            Debug.Log("잘봐 이게 클릭이야");
        }
    }
    public void OnGameClearedEarly(float timeLeft)
    {
        float elapsedTime = 30f - timeLeft;

        if (Win) return;
        Win = true;

        int index = 0;


        if (elapsedTime <= 10f && !Win)
        {
           
            Debug.Log("사이버사이코");
            index = 0;
        }
        else if (elapsedTime <= 15f && !Win)
         {
           
            Debug.Log("뚱이정도?");
            index = 1;
         }
       else if (elapsedTime <= 20f && !Win)
        {
           
            Debug.Log("바보");
            index = 2;
        }
        else if (elapsedTime <= 25f && !Win)
        {
            
            Debug.Log("이거나 받아라!");
            index = 3;
        }
    }

}