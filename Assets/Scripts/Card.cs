using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int idx = 0;
    // 끄고 킬 카드 앞,뒤 면
    [SerializeField] GameObject front;
    [SerializeField] GameObject back;
    // 카드 애니메이션
    [SerializeField] Animator anim;
    // 앞면 이미지
    [SerializeField] SpriteRenderer frontImg;
    // 카드 뒤집기 사운드
    AudioSource audioSource;
    public AudioClip clip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Setting(int num) // 보드에서 세팅
    {
        idx = num;
        //frontImg.sprite = Resources.Load<Sprite>("rtan" + idx.ToString());
        frontImg.sprite = Resources.Load<Sprite>($"rtan{idx}");
    }

    public void OpenCard()
    {
        // 게임 중이 아니라면 동작 금지
        if (GameManager.instance.gameStep != GameManager.GAMESTEP.STARTGAME) return;

        //PlayOneShot()을 사용하면 다른 효과음끼리 겹치지 않음
        audioSource.PlayOneShot(clip); 
        anim.SetBool("IsOpen", true);
        back.SetActive(false);
        front.SetActive(true);

        // 만약 fisrstCard가 비었다면
        if (GameManager.instance.fisrstCard == null)
        {
            // 이 카드의 정보를 넘겨주고
            GameManager.instance.fisrstCard = this;
        }
        // 만약 fisrstCard가 찼다면
        else
        {
            // secondCard에 이 카드 정보를 넘겨준다.
            GameManager.instance.secondCard = this;
            // 그 후 Mached() 호출
            GameManager.instance.Matched();
        }
    }
    public void DestoryCard()
    {
        Invoke("DestoryCardInvoke", 1.0f);
    }
    public void DestoryCardInvoke()
    {
        Destroy(gameObject);
    }

    public void CloseCard()
    {
        Invoke("CloseCardInvoke", 1.0f);
    }
    public void CloseCardInvoke()
    {
        front.SetActive(false);
        back.SetActive(true);
        anim.SetBool("IsOpen", false);
    }
}
