using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    public AudioClip[] clip; // 배경음악들

    private AudioSource source;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        source = GetComponent<AudioSource>();
        BgmPlay(0);
    }

    public void BgmPlay(int _playerBgmTrack)
    {
        Debug.Log("트릭 : " + _playerBgmTrack);
        source.clip = clip[_playerBgmTrack];
        source.Play();
    }
    public void BgmStop()
    {
        source.Stop();
    }
  
}
