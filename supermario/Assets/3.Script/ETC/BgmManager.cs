using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    public AudioClip[] clip; // ������ǵ�

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
        Debug.Log("Ʈ�� : " + _playerBgmTrack);
        source.clip = clip[_playerBgmTrack];
        source.Play();
    }
    public void BgmStop()
    {
        source.Stop();
    }
  
}
