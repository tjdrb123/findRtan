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
    public AudioClip match_success; // ������ ���� ���� �� ��ü
    public AudioClip match_fail;
    public AudioSource audioSource; // ���� ������ �����Ұ���

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
        // (.ToArray == ����Ʈ�� ����ڴ�) / (OrderBy == �� ������ ������ ���ְڴ�)
        // ((-1.0f, 1.0f) == ����Ʈ ���� �����ϰ�)

        for (int i = 0; i < 16; i++)
        {
            GameObject newCard = Instantiate(card);
            newCard.transform.parent = GameObject.Find("cards").transform; // newCard�� cards ���� ������ �Ű��ش�

            float x = (i / 4) * 1.4f - 2.1f; // ���� Ȱ�� (����: -2.1f�� ī�� ��ġ ����, �ؿ��� ��������)
            float y = (i % 4) * 1.4f - 3.0f; // �������� Ȱ��
            newCard.transform.position = new Vector3(x, y, 0);

            string rtanName = "rtan" + rtans[i].ToString();
            newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(rtanName);
            // newCard�� front��� �� ã�Ƽ� �� front�� Sprite �̹����� rtanName���� �������ִ� �ڵ�
        }

        /*for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GameObject newCard = Instantiate(card);
                newCard.transform.parent = GameObject.Find("cards").transform; // newCard�� cards ���� ������ �Ű��ش�

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
        if (work == true) // ù��° ī�尡 �������� �۵��Ⱑ true�� �ǰ�
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

    public void isMatched() // firstcard, secondcard ���ϴ� �Լ� == ��Ī
    {
        match_cnt++;

        string firstCardImage = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        // front��� sprite�� �����ִ� sprite�� �̸��� ����� ��, �� ���ϴ� ī���� ù��° ī�尡 ���� �̹����� ��� ī������ Ȯ����
        string secondCardImage = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;

        if (firstCardImage == secondCardImage) // ī�尡 ��ġ�ϸ� �����ǰ�,
        {
            checkTxt.color = new Color32(255, 255, 255, 255);
            checkTxt.text = "�强��!";

            audioSource.PlayOneShot(match_success);
            firstCard.GetComponent<card>().destroyCard(); // card.cs�� destroyCard�Լ� �ҷ�����
            secondCard.GetComponent<card>().destroyCard();

            int cardsLeft = GameObject.Find("cards").transform.childCount; // cards�� transform�� ã�Ƽ� childCount�� ��� �������
            if (cardsLeft == 2)
            {
                //�����Ű��
                //Time.timeScale = 0f;
                //endTxt.SetActive(true);
                Invoke("GameEnd", 1f);
                /* card.cs�� destroyCard()�� ���� �׸��� 1�ʵڿ� ������� �Ǵµ�,
                �� ���� ������ ����ǰ� ������ �����ϱ� ���Ͽ�
                timescale�� ����Ǵ� �ٸ� �Լ��� ����� invoke���־� �ð��� ������*/
            }
        }
        else // ī�尡 ��ġ���� ������ �ٽ� ���´�.
        {
            checkTxt.color = new Color32(255, 255, 255, 255);
            checkTxt.text = "����!";

            time -= 1f;

            audioSource.PlayOneShot(match_fail);
            firstCard.GetComponent<card>().closeCard();
            secondCard.GetComponent<card>().closeCard();
        }
        // ���� ���� �ϳ��� ����Ǿ� ��ġ�� ������, ���ġ�� ���� ������ ����ֱ�
        firstCard = null;
        secondCard = null;
    }


    void GameEnd()
    {
        Time.timeScale = 0f;
        endTxt.SetActive(true); // ���� ������ gameobject�� �ؾ� SetActive ���� ����, (((((����)))))
        warningTxt.color = new Color32(90, 90, 255, 255);
        checkTxt.color = new Color32(90, 90, 255, 255);
        matchCntTxt.text = "�� ��Ī Ƚ��: " + match_cnt.ToString();
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
