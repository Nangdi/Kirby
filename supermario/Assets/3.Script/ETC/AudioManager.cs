using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;
    private AudioSource source;

    public float Volumn;
    public bool loop;

    public Sound(float Volumn) 
    {
        source.volume = Volumn;
    }

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
    }

    public void Play()
    {
        source.Play();
    }
    public void Stop()
    {
        source.Stop();
    }
    public bool IsPlaying()
    {
        return source.isPlaying;
    }
}
public class AudioManager : MonoBehaviour
{
    [SerializeField]
    public Sound[] sounds;
    private static AudioManager instance;
    //private AudioManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;


        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
      
        Debug.Log("!");

        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject soundObject = new GameObject("사운드파일 이름 : " + i + " = " + sounds[i].name);
            sounds[i].SetSource(soundObject.AddComponent<AudioSource>());
            soundObject.transform.SetParent(transform);
        }
    }
    public void Play(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Play();
                return;
            }
        }
    }

    public void Stop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Stop();
                return;
            }
        }
    }

    public bool IsPlay(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                
                return sounds[i].IsPlaying();
            }

        }

        return false;
    }
}

