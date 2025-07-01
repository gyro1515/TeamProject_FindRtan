using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
    public GameObject card;

    List<Vector2> endV2;
    List<Vector2> startV2;
    List<float> startRot;
    List<GameObject> tmpG;
    //Lerp용
    float lerpTime = 0.0f;
    // 사운드용
    AudioSource sound;
    // 사운드 속도
    [SerializeField] float pitchSpeed = 3.5f;
    // 카드 뿌리는 간격
    [SerializeField] float cardTime = 0.5f;
    // 카드 날아가는 총 시간
    float cardTotalTime = 5.5f;

    void Start()
    {
        sound = GetComponent<AudioSource>();
        if (sound != null ) sound.pitch = pitchSpeed;
        
        int[] arr = { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5 };
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (arr[i], arr[j]) = (arr[j], arr[i]);
        }
        // 기존 방식 = 원하는 위치에 소환
        /*for (int i = 0; i < 10; i++)
        {
           GameObject go = Instantiate(card, this.transform);

            float x = (i % 4) * 1.4f - 2.1f;
            float y = (i / 4) * 1.4f - 3.0f;

            go.transform.position = new Vector2(x, y);
            go.GetComponent<Card>().Setting(arr[i]);
        }*/
        // 새로운 방식, 카드 생성하고 원하는 위치에 뿌리기
        endV2 = new List<Vector2>();
        startV2 = new List<Vector2>();
        startRot = new List<float>();
        tmpG = new List<GameObject>();
        
        for (int i = 0; i < 10; i++)
        {
            tmpG.Add(Instantiate(card, this.transform));
            float x = (i % 4) * 1.4f - 2.1f;
            float y = (i / 4) * 1.4f - 3.0f;
            endV2.Add(new Vector2(x, y));

            tmpG[i].transform.position = new Vector2(0f, -5f);

            startV2.Add(tmpG[i].transform.position);
            startRot.Add((float)i * 10 - 45f);
            tmpG[i].transform.Rotate(0f, 0f, startRot[i]);
            //go.transform.rotation = Quaternion.Euler(0f, 0f, (float)i * 4);
            tmpG[i].GetComponent<Card>().Setting(arr[i]);
            tmpG[i].GetComponent<Card>().anim.speed = 0f;
            // 첫번째 카드가 가장 위로 가게 하기
            tmpG[i].GetComponent<Card>().backSprite.sortingOrder = 20 - i;
            tmpG[i].GetComponent<Card>().backCanvas.sortingOrder = 20 - i;
        }
        // 위치 정보 뒤집어서 먼 곳부터 날리기, 
        endV2.Reverse();
        startV2.Reverse();

        // 날아가는 시간 세팅
        cardTotalTime = cardTime * 9 + 1.0f; // 카드 날리는 간격 * 카드 인덱스 수 + 1초만큼(현재 1초동안 날아감)
    }

    // Update is called once per frame
    void Update()
    {
        if (lerpTime >= 0.0f)
        {
            lerpTime += Time.deltaTime;
            // 뿌려보기
            for (int i = 0; i < 10; i++)
            {
                int tmpI = cardTime == 0.0f ? 10 : (int)(lerpTime / cardTime); // cardTime이 0이라면 바로 다 나가도록
                if (tmpI < i) return;
                // 직선
                //tmpG[i].transform.position = Vector2.Lerp(startV2[i], endV2[i], lerpTime);
                float tmpLerpTime = lerpTime - cardTime * i;
                tmpG[i].transform.position = Vector2.Lerp(startV2[i], endV2[i], tmpLerpTime);
                float angleOffset = Mathf.Lerp(0f, 720f - startRot[i], tmpLerpTime); // 두 바퀴 회전 용
                float nextZ = startRot[i] + angleOffset;
                //tmpG[i].transform.localRotation = Quaternion.Euler(0f, 0f, nextZ);
                tmpG[i].transform.localEulerAngles = new Vector3(0f, 0f, nextZ);
                //tmpG[i].transform.eulerAngles = new Vector3(0f, 0f, nextZ);
            }
            if (lerpTime > cardTotalTime)
            {
                lerpTime = -1f; // 업데이트 방지용
                for (int i = 0; i < 10; i++)
                {
                    tmpG[i].GetComponent<Card>().anim.speed = 1.0f;
                    tmpG[i].GetComponent<Card>().cardState = Card.CardState.Ready;
                }
            }
        } 
    }
}
