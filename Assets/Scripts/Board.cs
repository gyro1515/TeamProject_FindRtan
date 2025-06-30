using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Board : MonoBehaviour
{
    
    // 카드 개수
    [SerializeField] int cardCnt = 16;
    [SerializeField] int cardColumn = 4;
    // 카드 시작 위치
    [SerializeField] Vector2 cardOffset = new Vector2(-2.1f, -3.0f);

    // 카드
    [SerializeField] GameObject card;


    // Start is called before the first frame update
    void Start()
    {
        int[] arr = new int[cardCnt];
        for (int i = 0; i < cardCnt; i++)
        {
            // 이미지를 위한 배열 할당
            arr[i] = i / 2;
        }
        // 배열 뒤섞기
        // arr = arr.OrderBy(x => Random.Range(0f, 7f)).ToArray(); // 강의 방법
        // Fisher–Yates Shuffle
        for (int i = cardCnt - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (arr[i], arr[j]) = (arr[j], arr[i]);
        }
        for (int i = 0; i < cardCnt; i++)
        {
            // 카드 위치 갱신
            GameObject tmp =Instantiate(card, this.transform);
            Vector2 pos = cardOffset;
            pos.x += i % cardColumn * 1.4f;
            pos.y += i / cardColumn * 1.4f;

            tmp.transform.position = pos;
            tmp.GetComponent<Card>().Setting(arr[i]);
        }
        GameManager.instance.cardCnt = cardCnt;
    }
}
