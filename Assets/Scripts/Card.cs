using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Card : MonoBehaviour
{
    public enum CardState
    {
        Fly, Ready
    }
    public CardState cardState = CardState.Fly;
    // 오더 인 레이어도 수정해보기
    [SerializeField] public Canvas backCanvas;
    [SerializeField] public SpriteRenderer backSprite;

    public int idx = 0;
    // 끄고 킬 카드 앞,뒤 면
    [SerializeField] GameObject front;
    [SerializeField] GameObject back;
    // 카드 애니메이션
    [SerializeField] public Animator anim;
    // 앞면 이미지
    [SerializeField] SpriteRenderer frontImg;
    // 카드 뒤집기 사운드
    protected AudioSource audioSource;
    public AudioClip flipClip;
    public AudioClip correctClip;
    public AudioClip errorClip;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Setting(int num) // 보드에서 세팅
    {
        idx = num;
        frontImg.sprite = Resources.Load<Sprite>(idx.ToString());
        //Debug.Log("card setting");
    }

    // 게임 매니저씬에서 가져와야 함
    public virtual void OpenCard()
    {
        // 게임 중이 아니라면 동작 금지
        if (GameManager.instance.progress != GameManager.GameProgress.StartGame) return;
        // 날아가는 중이라면 동작 금지
        if (cardState == CardState.Fly) return;

        //PlayOneShot()을 사용하면 다른 효과음끼리 겹치지 않음
        audioSource.PlayOneShot(flipClip);
        anim.SetBool("IsOpen", true);
        back.SetActive(false);
        front.SetActive(true);

        // 만약 fisrstCard가 비었다면
        if (GameManager.instance.firstCard == null)
        {
            // 이 카드의 정보를 넘겨주고
            GameManager.instance.firstCard = this;
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
    public void DestroyCard()
    {
        Invoke("DestroyCardInvoke", 1.0f);
    }
    public void DestroyCardInvoke()
    {
        Destroy(gameObject);
    }

    public virtual void CloseCard()
    {
        Invoke("CloseCardInvoke", 1.0f);
    }
    public virtual void CloseCardInvoke()
    {
        front.SetActive(false);
        back.SetActive(true);
        anim.SetBool("IsOpen", false);
    }

    public void OpenCardReady()
    {
        Invoke("OpenCardInvoke", 0.2f);
    }
    public void OpenCardInvoke()
    {
        back.SetActive(false);
        front.SetActive(true);
    }
    public void PlayCorrectSound()
    {
        audioSource.PlayOneShot(correctClip);
    }
    public void PlayErrorSount()
    {
        audioSource.PlayOneShot(errorClip);
    }
}
