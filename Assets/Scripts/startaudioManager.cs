using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startaudiomanager : MonoBehaviour
{
    public AudioClip startbgm; // ������ ���� ���� �� ��ü
    public AudioSource audioSource; // ���� ������ �����Ұ���

    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = startbgm; // ���� ����
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

// ��������� ��� ����Ǿ�� �ϱ� ������, ���� �������־�� �մϴ�