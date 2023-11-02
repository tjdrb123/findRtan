using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class card : MonoBehaviour
{
    public Animator anim;
    public AudioClip flip; // 실행할 음악 파일 그 자체
    public AudioSource audioSource; // 누가 음악을 실행할건지
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
        audioSource.PlayOneShot(flip); // 한 번 실행
        transform.Find("front").gameObject.SetActive(true);
        transform.Find("back").gameObject.SetActive(false);

        if (gameManager.I.firstCard == null) // 게임매니저가 첫번째 카드가 뭔지 모르면, 그게 첫번째 카드로 해주고
        {
            gameManager.I.firstCard = gameObject;

            gameManager.I.work = true;
        }
        else // 아니면 두번째 카드로 해라
        {
            gameManager.I.work = false;

            gameManager.I.secondCard = gameObject;
            gameManager.I.isMatched(); // 두번째 카드를 까고 비교해야 되는것임
        }
    }

    public void destroyCard() // "destroyCardInvoke" 함수를 불러서 1초뒤에 실행되게 함
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
