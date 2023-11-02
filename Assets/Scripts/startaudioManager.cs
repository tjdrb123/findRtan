using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startaudiomanager : MonoBehaviour
{
    public AudioClip startbgm; // 실행할 음악 파일 그 자체
    public AudioSource audioSource; // 누가 음악을 실행할건지

    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = startbgm; // 음악 파일
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

// 배경음악은 계속 재생되어야 하기 때문에, 따로 관리해주어야 합니다