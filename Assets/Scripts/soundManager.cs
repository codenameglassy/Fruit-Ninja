using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SoundType
{
    mainMenuSound,
    backgroundSound,
    slashSound,
}


public class soundManager : MonoBehaviour
{
    public static soundManager instance;
    public List<AudioClip> mainMenuSound;
    public List<AudioClip> backGroundSound;

    public List<AudioClip> slashEffectSounds;

    private AudioSource backGroundAudioSource;
    private AudioSource slashAudioSource;
    // Start is called before the first frame update
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        PlaySound(SoundType.mainMenuSound);
        SceneManager.LoadScene(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (backGroundAudioSource != null)
        {
            if(backGroundAudioSource.isPlaying == false)
            {
                switch (SceneManager.GetActiveScene().buildIndex)
                {
                    
                    case 1:
                        PlaySound(SoundType.mainMenuSound);
                        break;
                    case 2:
                        PlaySound(SoundType.backgroundSound);
                        break;
                    
                }
            }
        }
    }

    public void PlaySound(SoundType soundType)
    {
        AudioClip clip;
        int soundIndex;
        switch (soundType)
        {
            case SoundType.mainMenuSound:
                soundIndex = Random.Range(0, mainMenuSound.Count);
                clip = mainMenuSound[soundIndex];
                if (backGroundAudioSource == null)
                {
                    backGroundAudioSource = gameObject.AddComponent<AudioSource>();
                }
                backGroundAudioSource.clip = clip;
                backGroundAudioSource.loop = false;
                backGroundAudioSource.Play();
                break;
            case SoundType.backgroundSound:
                soundIndex = Random.Range(0, backGroundSound.Count);
                clip = backGroundSound[soundIndex];
                if (backGroundAudioSource == null)
                {
                    backGroundAudioSource = gameObject.AddComponent<AudioSource>();
                }
                backGroundAudioSource.clip = clip;
                backGroundAudioSource.loop = false;
                backGroundAudioSource.Play();
                break;
            case SoundType.slashSound:
                soundIndex = Random.Range(0, slashEffectSounds.Count);
                clip = slashEffectSounds[soundIndex];
                if (slashAudioSource == null)
                {
                    slashAudioSource = gameObject.AddComponent<AudioSource>();
                }
                slashAudioSource.clip = clip;
                slashAudioSource.loop = false;
                slashAudioSource.Play();
                break;
            default:
                break;
        }
    }

    public void MusicVolumeChanged(float volume)
    {
        if(backGroundAudioSource != null)
        {
            backGroundAudioSource.volume = volume;
        }
    }

    public void SoundVolumeChanged(float volume)
    {
        if (slashAudioSource != null)
        {
            slashAudioSource.volume = volume;
        }
    }

    public float MusicVolume()
    {
        if (backGroundAudioSource != null)
        {
            return backGroundAudioSource.volume;
        }
        return 1;
    }

    public float SoundVolume()
    {
        if (slashAudioSource != null)
        {
            return slashAudioSource.volume;
        }
        return 1;
    }
}
