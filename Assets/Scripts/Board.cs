using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
    enum SetCard
    {
        Ready, Start, Throw, End
    }
    SetCard setState = SetCard.Ready;
    // 프리팹 카드
    public GameObject card;

    // 보간 사용을 위한 값 저장 배열
    List<Vector2> endV2;
    List<float> startRot;
    List<GameObject> tmpG;
    List<Vector2> StartPosA;
    // 시작 위치
    [SerializeField] Vector2 startV2 = new Vector2(0f, -5f);
    // 시작 위치 각도
    float startTheta = Mathf.PI;
    // 끝 위치 각도
    List<float> EndTheta;
    //Lerp용 Time
    float lerpTime = 0.0f;
    // 반지름
    float tmpR = 0.6f;

    // 사운드용
    AudioSource sound;
    // 사운드 속도
    [SerializeField] float pitchSpeed = 3.5f;
    // 카드 뿌리는 간격
    [SerializeField] float cardTime = 0.5f;
    // 카드 날아가는 총 시간
    public float cardTotalTime = 5.5f;
    // 카드 총 개수
    [SerializeField] int cardCnt = 12;

    private void Awake()
    {
        sound = GetComponent<AudioSource>();
        if (sound != null)
        {
            sound.Stop();
            sound.pitch = pitchSpeed;
        }
        // 날아가는 시간 세팅
        // 카드 날리는 간격 * 카드 인덱스만큼 + 날아가는 시간
        cardTotalTime = cardTime * (cardCnt - 1) + 1.0f + 1.25f;
    }
    void Start()
    {
        int[] arr = { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6 };
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (arr[i], arr[j]) = (arr[j], arr[i]);
        }

        // 새로운 방식, 카드 생성하고 원하는 위치에 뿌리기
        endV2 = new List<Vector2>();   // 도착 지점 저장용 배열
        startRot = new List<float>();  // 시작 회전 값 저장용 배열
        EndTheta = new List<float>();  // 끝 위치 회전 값 저장용 배열
        tmpG = new List<GameObject>(); // 카드 오브젝트 저장용 배열
        StartPosA = new List<Vector2>(); // 시작 지점 저장용 배열
        // 또 다른 방식
        for (int i = 0; i < cardCnt; i++)
        {
            tmpG.Add(Instantiate(card, this.transform));

            // 배치 위치 지정하기
            float x = (i % 4) * 1.4f - 2.1f;
            float y = (i / 4) * 1.4f - 3.0f;
            endV2.Add(new Vector2(x, y)); // 도착지머

            // Ver3: 한점에서 펼쳐지기
            float tmpTheta = 180.0f / (cardCnt - 1) * i * Mathf.Deg2Rad;
            EndTheta.Add(tmpTheta);
            StartPosA.Add(new Vector2(0f + tmpR * Mathf.Cos(tmpTheta), -5f + tmpR * Mathf.Sin(tmpTheta)));
            tmpG[i].transform.position = new Vector2(0f + tmpR * Mathf.Cos(startTheta), -5f + tmpR * Mathf.Sin(startTheta));
            // 모두 거꾸로 회전해서 시작, 그래야 펼쳤을 때, 맨 위의 카드가 정면
            tmpG[i].transform.rotation = Quaternion.Euler(0f, 0f, 180); 

            // 시작 회전 배열에 넣어주기
            //startRot.Add((float)i * 10 - 45f);
            startRot.Add(180.0f / (cardCnt - 1) * i);
            // 시작 회전 게임 오브젝트에 설정하기
            //tmpG[i].transform.Rotate(0f, 0f, startRot[i]);
            Card tmpCard = tmpG[i].GetComponent<Card>();
            // 카드에 인덱스, 사진 등 세팅해주기
            tmpCard.Setting(arr[i]);
            // 카드 애니메이션 재생 중지하기
            tmpCard.anim.speed = 0f;
            // 첫번째 카드가 가장 위로 가게 하기
            tmpCard.backSprite.sortingOrder = 20 - i;
            tmpCard.backCanvas.sortingOrder = 20 - i;
        }
        endV2.Reverse();
    }

    // Update is called once per frame
    void Update()
    {
        switch (setState)
        {
            case SetCard.Ready:
                lerpTime += Time.deltaTime;
                if (lerpTime >= 0.5f)
                {
                    lerpTime = 0.0f;
                    setState = SetCard.Start;
                }
                break;
            case SetCard.Start:
                lerpTime += Time.deltaTime;
                for (int i = 0; i < cardCnt; i++)
                {
                    float targetTheta = Mathf.Lerp(startTheta, EndTheta[i], lerpTime * 2);
                    tmpG[i].transform.position = new Vector2(0f + tmpR * Mathf.Cos(targetTheta), -5f + tmpR * Mathf.Sin(targetTheta));
                    float nextZ = targetTheta * Mathf.Rad2Deg;
                    // 회전 적용
                    tmpG[i].transform.rotation = Quaternion.Euler(0f, 0f, nextZ);
                }
                if (lerpTime >= 0.75f)
                {
                    setState = SetCard.Throw;
                    sound.Play();
                    lerpTime = 0.0f;
                }
                break;
            case SetCard.Throw:
                // 시간에 따라 카드가 날아간다
                lerpTime += Time.deltaTime;
                // 카드 날리기
                for (int i = 0; i < cardCnt; i++) // 0번 카드 부터 카드 개수 - 1번 카드까지
                {
                    // lerpTime과 cardTime에 따라 날아가는 인덱스 설정하기
                    // ex) lerpTime = 1.2f, cardTime = 0.5f -> tmpI = (int)(1.2f / 0.5f) = 2 
                    //     (2번 인덱스 까지 카드 날리기 진행)
                    // 삼항 연산자로 cardTime이 0이라면 바로 다 날아가도록 설정
                    int tmpI = cardTime == 0.0f ? cardCnt : (int)(lerpTime / cardTime);

                    // tmpI보다 인덱스가 작으면 리턴하기
                    // 해당 인덱스부터는 업데이트가 필요 없기때문에 continue가 아닌 return
                    if (tmpI < i) return;

                    // tmpLerpTime: 실제 보간용 float 변수
                    // 각 인덱스마다 보간 값을 0에서 1.0으로 변환하기 위한 변수
                    float tmpLerpTime = lerpTime - cardTime * i;

                    // Ease Out 효과 주기
                    // 0이하거나 1을 넘어가면 오류가 나기 때문에 범위 0부터 1까지만 지정하기
                    float t = Mathf.Clamp01(tmpLerpTime);
                    // 기본은 제곱으로 설정, 제곱 수 올릴 수록 처음에 빠르고, 마지막에 느려진다
                    float easedOut = 1 - Mathf.Pow(1 - t, 2);

                    // 위치 보간하여 이동하기
                    tmpG[i].transform.position = Vector2.Lerp(StartPosA[i], endV2[i], easedOut); // 방식2
                                                                                                 // 회전 보간하여 회전하기
                    float angleOffset = Mathf.Lerp(0f, 720f - startRot[i], easedOut); // 두 바퀴 회전 용
                    float nextZ = startRot[i] + angleOffset;
                    // 회전 적용
                    tmpG[i].transform.rotation = Quaternion.Euler(0f, 0f, nextZ);
                }

                // lerpTime이 총 카드 배치 시간을 넘어가면 -1로 설정하여 Update() 안되게 하기
                // Ready + Start = 1.25초 소요
                if (1.25f + lerpTime >= cardTotalTime)
                {
                    setState = SetCard.End;
                    // 카드 배치가 끝났다면 애니메이션을 재생시키고, 카드 클릭 가능하게 하기
                    for (int i = 0; i < cardCnt; i++)
                    {
                        // 애니메이션 재생
                        tmpG[i].GetComponent<Card>().anim.speed = 1.0f;
                        // 카드 상태 배치 중에서 클릭 가능으로 변경
                        tmpG[i].GetComponent<Card>().cardState = Card.CardState.Ready;
                    }
                }
                break;
                case SetCard.End:
                // Udate() 최소화
                break;
            default:
                break;
        }
    }
}
