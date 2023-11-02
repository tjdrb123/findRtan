using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class card : MonoBehaviour
{
    public Animator anim;
    public AudioClip flip; // ������ ���� ���� �� ��ü
    public AudioSource audioSource; // ���� ������ �����Ұ���
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openCard()
    {
        anim.SetBool("isOpen", true);
        audioSource.PlayOneShot(flip); // �� �� ����
        transform.Find("front").gameObject.SetActive(true);
        transform.Find("back").gameObject.SetActive(false);

        if (gameManager.I.firstCard == null) // ���ӸŴ����� ù��° ī�尡 ���� �𸣸�, �װ� ù��° ī��� ���ְ�
        {
            gameManager.I.firstCard = gameObject;

            gameManager.I.work = true;
        }
        else // �ƴϸ� �ι�° ī��� �ض�
        {
            gameManager.I.work = false;

            gameManager.I.secondCard = gameObject;
            gameManager.I.isMatched(); // �ι�° ī�带 ��� ���ؾ� �Ǵ°���
        }
    }

    public void destroyCard() // "destroyCardInvoke" �Լ��� �ҷ��� 1�ʵڿ� ����ǰ� ��
    {
        Invoke("destroyCardInvoke", 1.0f);
    }

    void destroyCardInvoke()
    {
        Destroy(gameObject);
    }

    public void closeCard()
    {
        Invoke("closeCardInvoke", 1f);
    }

    void closeCardInvoke()
    {
        anim.SetBool("isOpen", false);
        transform.Find("back").gameObject.SetActive(true);
        transform.Find("back").GetComponent<SpriteRenderer>().color = Color.gray;

        transform.Find("front").gameObject.SetActive(false);
    }

    public void closeFirstCard()
    {
        anim.SetBool("isOpen", false);

        Invoke("firstCardChange", 0.5f);
        
    }

    public void firstCardChange()
    {
        transform.Find("back").gameObject.SetActive(true);
        transform.Find("back").GetComponent<SpriteRenderer>().color = Color.gray;

        transform.Find("front").gameObject.SetActive(false);
    }
}
