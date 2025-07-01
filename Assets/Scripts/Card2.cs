using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card2 : Card // 카드 상속 받아서 카드에 있는 값들 그대로 쓰기
{
    // 카드가 열렸는가? 열리는 중이어도 OpenCard()작동 안되게
    bool tmpIsOpen = false;
    

    // 게임 매니저씬에서 가져와야 함
    public override void OpenCard()
    {
        // 게임 중이 아니라면 동작 금지
        if (GameManager.instance.progress != GameManager.GameProgress.StartGame) return;
        // 날아가는 중이라면 동작 금지
        if (cardState == CardState.Fly) return;
        // 뒤집는 중이라면 동작 금지 = 리턴
        if (tmpIsOpen) return;

        tmpIsOpen = true;
        //PlayOneShot()을 사용하면 다른 효과음끼리 겹치지 않음
        audioSource.PlayOneShot(flipClip);
        anim.SetBool("IsOpen", true);
        /*back.SetActive(false);
        front.SetActive(true);*/

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
    
    public override void CloseCard() // 필요한 함수만 가상함수로 만들어 오버라이드 하기
    {
        Invoke("CloseCardInvoke", 1.0f);
    }
    public override void CloseCardInvoke()
    {
        anim.SetBool("IsOpen", false);
        tmpIsOpen = false;
    }
}
