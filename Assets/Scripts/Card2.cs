using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card2 : MonoBehaviour
{
    // �׽�Ʈ��
    float tmpTime = 0;
    bool tmpIsOpen = false;


    public int idx = 0;
    // ���� ų ī�� ��,�� ��
    [SerializeField] GameObject front;
    [SerializeField] GameObject back;
    // ī�� �ִϸ��̼�
    [SerializeField] Animator anim;
    // �ո� �̹���
    [SerializeField] SpriteRenderer frontImg;
    // ī�� ������ ����
    AudioSource audioSource;
    public AudioClip clip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Setting(int num) // ���忡�� ����
    {
        idx = num;
        //frontImg.sprite = Resources.Load<Sprite>("rtan" + idx.ToString());
        frontImg.sprite = Resources.Load<Sprite>($"rtan{idx}");
    }
    private void Update()
    {
        // �׽�Ʈ��
        tmpTime += Time.deltaTime;
        if (tmpTime >= 1.0f && !tmpIsOpen) // 1�ʰ� ������ ���� ������ ��,
        {
            tmpTime = -0.2f;
            tmpIsOpen = true;
            audioSource.PlayOneShot(clip);
            anim.SetBool("IsOpen", true);
            //OpenCardReady();
        }
        else if (tmpTime >= 1.0f && tmpIsOpen) // 1�ʰ� ������ ���� ������ ��,
        {
            tmpTime = 0.0f;
            tmpIsOpen = false;
            front.SetActive(false);
            back.SetActive(true);
            anim.SetBool("IsOpen", false);
        }
    }
    // ���� �Ŵ��������� �����;� ��
    /*public void OpenCard()
    {
        // ���� ���� �ƴ϶�� ���� ����
        //if (GameManager.instance.gameStep != GameManager.GAMESTEP.STARTGAME) return;

        //PlayOneShot()�� ����ϸ� �ٸ� ȿ�������� ��ġ�� ����
        audioSource.PlayOneShot(clip);
        anim.SetBool("IsOpen", true);
        back.SetActive(false);
        front.SetActive(true);

        // ���� fisrstCard�� ����ٸ�
        if (GameManager.instance.firstCard == null)
        {
            // �� ī���� ������ �Ѱ��ְ�
            GameManager.instance.firstCard = this;
        }
        // ���� fisrstCard�� á�ٸ�
        else
        {
            // secondCard�� �� ī�� ������ �Ѱ��ش�.
            GameManager.instance.secondCard = this;
            // �� �� Mached() ȣ��
            GameManager.instance.Matched();
        }
    }*/
    public void DestroyCard()
    {
        Invoke("DestroyCardInvoke", 1.0f);
    }
    public void DestroyCardInvoke()
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

    public void OpenCardReady()
    {
        Invoke("OpenCardInvoke", 0.2f);
    }
    public void OpenCardInvoke()
    {
        back.SetActive(false);
        front.SetActive(true);
    }
}
