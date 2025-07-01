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

    void Start()
    {
        sound = GetComponent<AudioSource>();
        sound.pitch = 3.5f;
        int[] arr = { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5 };
        //arr.OrderBy(x => Random.Range(1f, 5f)).ToArray();
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
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (lerpTime <= 1.0f && lerpTime >= 0.0f)
        {
            lerpTime += Time.deltaTime;
            // 뿌려보기
            for (int i = 0; i < 10; i++)
            {
                tmpG[i].transform.position = Vector2.Lerp(startV2[i], endV2[i], lerpTime);
                float angleOffset = Mathf.Lerp(0f, 720f - startRot[i], lerpTime); // 두 바퀴 회전 용
                float nextZ = startRot[i] + angleOffset;
                tmpG[i].transform.localRotation = Quaternion.Euler(0f, 0f, nextZ);
            }
        }
        else if (lerpTime > 1.0f)
        {
            lerpTime = -1f;
            for (int i = 0; i < 10; i++)
            {
                tmpG[i].GetComponent<Card>().anim.speed = 1.0f;
            }
        }
    }
}
