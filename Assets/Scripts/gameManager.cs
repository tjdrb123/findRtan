using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class gameManager : MonoBehaviour
{
    public Text warningTxt;
    public Text matchCntTxt;
    public Text timeTxt;
    public Text checkTxt;
    public GameObject selectTxt;
    public GameObject endTxt;
    public GameObject card;
    public float time = 60.0f;

    public float limit_time = 5.0f;
    public bool work = false;

    public int match_cnt = 0;
    public static gameManager I;
    public GameObject firstCard;
    public GameObject secondCard;
    public AudioClip match_success; // 실행할 음악 파일 그 자체
    public AudioClip match_fail;
    public AudioSource audioSource; // 누가 음악을 실행할건지

    int blinknum = 0;

    void Awake()
    {
        I = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        int[] rtans = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, /*8, 8, 9, 9, 10, 10, 11, 11, 12*/ };
        rtans = rtans.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();
        // (.ToArray == 리스트로 만들겠다) / (OrderBy == 이 순서를 정렬을 해주겠다)
        // ((-1.0f, 1.0f) == 리스트 값을 랜덤하게)

        for (int i = 0; i < 16; i++)
        {
            GameObject newCard = Instantiate(card);
            newCard.transform.parent = GameObject.Find("cards").transform; // newCard를 cards 공간 안으로 옮겨준다

            float x = (i / 4) * 1.4f - 2.1f; // 몫을 활용 (참고: -2.1f는 카드 위치 조정, 밑에도 마찬가지)
            float y = (i % 4) * 1.4f - 3.0f; // 나머지를 활용
            newCard.transform.position = new Vector3(x, y, 0);

            string rtanName = "rtan" + rtans[i].ToString();
            newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(rtanName);
            // newCard의 front라는 걸 찾아서 그 front에 Sprite 이미지를 rtanName으로 변경해주는 코드
        }

        /*for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GameObject newCard = Instantiate(card);
                newCard.transform.parent = GameObject.Find("cards").transform; // newCard를 cards 공간 안으로 옮겨준다

                float x = i * 1.1f - 2.2f;
                float y = j * 1.1f - 3.5f;
                newCard.transform.position = new Vector3(x, y, 0);

                string rtanName = "rtan" + rtans[].ToString();
                newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(rtanName);
            }
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (work == true) // 첫번째 카드가 열렸을때 작동기가 true가 되고
        {
            selectTxt.SetActive(true);
            timeTxt.color = Color.green;
            limit_time -= Time.deltaTime;
            timeTxt.text = limit_time.ToString("N2");

            if (limit_time < 0.0f)
            {
                firstCard.GetComponent<card>().closeFirstCard();
                firstCard = null;
                work = false;
            }
        }
        if (work == false)
        {
            selectTxt.SetActive(false);
            limit_time = 5.0f;
            timeTxt.color = Color.white;
            timeTxt.text = time.ToString("N2");
        }

        if (time < 10.0f)
        {
            InvokeRepeating("TxtColorChange", 0.01f, 3f);
            if (time < 0.0f)
            {
                GameEnd();
            }
        }
    }

    public void isMatched() // firstcard, secondcard 비교하는 함수 == 매칭
    {
        match_cnt++;

        string firstCardImage = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        // front라는 sprite에 적혀있는 sprite의 이름을 물어보는 것, 즉 비교하는 카드의 첫번째 카드가 무슨 이미지가 담긴 카드인지 확인함
        string secondCardImage = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;

        if (firstCardImage == secondCardImage) // 카드가 일치하면 삭제되고,
        {
            checkTxt.color = new Color32(255, 255, 255, 255);
            checkTxt.text = "장성규!";

            audioSource.PlayOneShot(match_success);
            firstCard.GetComponent<card>().destroyCard(); // card.cs의 destroyCard함수 불러오기
            secondCard.GetComponent<card>().destroyCard();

            int cardsLeft = GameObject.Find("cards").transform.childCount; // cards의 transform을 찾아서 childCount가 몇갠지 물어봐줘
            if (cardsLeft == 2)
            {
                //종료시키자
                //Time.timeScale = 0f;
                //endTxt.SetActive(true);
                Invoke("GameEnd", 1f);
                /* card.cs의 destroyCard()를 통해 그림이 1초뒤에 사라지게 되는데,
                그 삭제 행위가 실행되고 게임을 종료하기 위하여
                timescale이 실행되는 다른 함수를 만들고 invoke해주어 시간을 벌어줌*/
            }
        }
        else // 카드가 일치하지 않으면 다시 덮는다.
        {
            checkTxt.color = new Color32(255, 255, 255, 255);
            checkTxt.text = "실패!";

            time -= 1f;

            audioSource.PlayOneShot(match_fail);
            firstCard.GetComponent<card>().closeCard();
            secondCard.GetComponent<card>().closeCard();
        }
        // 위에 둘중 하나가 실행되어 매치가 끝나면, 재매치를 위해 변수값 비워주기
        firstCard = null;
        secondCard = null;
    }


    void GameEnd()
    {
        Time.timeScale = 0f;
        endTxt.SetActive(true); // 변수 선언을 gameobject로 해야 SetActive 제어 가능, (((((참고)))))
        warningTxt.color = new Color32(90, 90, 255, 255);
        checkTxt.color = new Color32(90, 90, 255, 255);
        matchCntTxt.text = "총 매칭 횟수: " + match_cnt.ToString();
        timeTxt.text = "0.00";
        selectTxt.SetActive(false);
    }

    void TxtColorChange()
    {
        blinknum++;
        if (blinknum % 3 == 0)
        {
            warningTxt.color = Color.black;
        }
        else
        {
            warningTxt.color = new Color32(90, 90, 255, 255);
        }
    }
}
