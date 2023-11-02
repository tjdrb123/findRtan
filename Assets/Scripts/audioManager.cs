using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{

    public AudioClip playbgm; // ������ ���� ���� �� ��ü
    public AudioSource audioSource; // ���� ������ �����Ұ���

    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = playbgm; // ���� ����
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
        {
            audioSource.Stop();
        }

        if (gameManager.I.time < 10.0f)
        {
            GetComponent<AudioSource>().pitch = 1.2f;
        }
    }
}


// ��������� ��� ����Ǿ�� �ϱ� ������, ���� �������־�� �մϴ�