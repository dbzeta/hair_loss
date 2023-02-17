using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_SOUND_TYPE
{
    E_NONE = -1,
    E_BGM_INGAME = 0,
    E_BGM_TOWN = 1,
    E_EFFECT_SOUND_BTN_ALERT = 10,
    E_EFFECT_SOUND_BTN_BUS,
    E_EFFECT_SOUND_BTN_CANCEL,
    E_EFFECT_SOUND_BTN_COMMON,
    E_EFFECT_SOUND_BTN_CONFIRM,
    E_EFFECT_SOUND_BTN_EXPLODE,
    E_EFFECT_SOUND_BTN_FALL,
    E_EFFECT_SOUND_BTN_LEFT,
    E_EFFECT_SOUND_BTN_RIGHT,
    E_EFFECT_SOUND_BTN_INTRO,
}

public class SoundManager : MonoBehaviour
{
    public AudioSource bgmPlayer;
    public AudioSource effectSoundPlayer;
    public AudioClip[] bgmClips;
    public AudioClip[] effectSoundClips;

    private float bgmValue;
    private float effectSoundValue;

    public float BgmValue
    {
        get { return bgmValue; }
    }
    public float EffectSoundValue
    {
        get { return effectSoundValue; }
    }


    public void Init()
    {
        if (bgmPlayer == null) bgmPlayer = this.transform.Find("BGM").GetComponent<AudioSource>();
        bgmClips = Resources.LoadAll<AudioClip>("Sounds/BGM");

        if (effectSoundPlayer == null) effectSoundPlayer = this.transform.Find("EffectSound").GetComponent<AudioSource>();
        effectSoundClips = Resources.LoadAll<AudioClip>("Sounds/EffectSound");

        if (!PlayerPrefs.HasKey("BGM")) 
            PlayerPrefs.SetFloat("BGM", 1.0f);
        bgmValue = PlayerPrefs.GetFloat("BGM");

        if (!PlayerPrefs.HasKey("EffectSound")) 
            PlayerPrefs.SetFloat("EffectSound", 1.0f);
        effectSoundValue = PlayerPrefs.GetFloat("EffectSound");
    }

    public void PlayEffectSound(ESOUND_TYPE _type)
    {
        PlayEffectSound(_type, effectSoundValue);
    }
    public void PlayEffectSound(ESOUND_TYPE _type, float _soundValue)
    {
        SetEffectSoundValue(_soundValue);
        AudioClip clip = GetAudioClip(_type);
        effectSoundPlayer.volume = effectSoundValue;
        effectSoundPlayer.loop = false;
        effectSoundPlayer.PlayOneShot(clip);
    }


    public void PlayBGM(ESOUND_TYPE _type)
    {
        PlayBGM(_type, bgmValue);
    }
    public void PlayBGM(ESOUND_TYPE _type, float _soundValue)
    {
        SetBGMValue(_soundValue);
        bgmPlayer.Stop();
        bgmPlayer.clip = GetAudioClip(_type);
        bgmPlayer.volume = bgmValue;
        bgmPlayer.loop = true;
        bgmPlayer.time = 0;
        bgmPlayer.Play();
    }

    public void SetEffectSoundValue(float _value)
    {
        effectSoundValue = _value;
        PlayerPrefs.SetFloat("EffectSound", _value);
        effectSoundPlayer.volume = effectSoundValue;
    }
    public void SetBGMValue(float _value)
    {
        bgmValue = _value;
        PlayerPrefs.SetFloat("BGM", _value);
        bgmPlayer.volume = BgmValue;
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    public AudioClip GetAudioClip(ESOUND_TYPE soundType)
    {
        AudioClip audioClip = null;

        switch (soundType)
        {
            case ESOUND_TYPE.E_NONE:
                break;
            case ESOUND_TYPE.E_BGM_INGAME:
            case ESOUND_TYPE.E_BGM_TOWN:
                {
                    int clipIdx = (int)soundType;
                    audioClip = bgmClips[clipIdx];
                    break;
                }
            case ESOUND_TYPE.E_EFFECT_SOUND_BTN_ALERT:
            case ESOUND_TYPE.E_EFFECT_SOUND_BTN_BUS:
            case ESOUND_TYPE.E_EFFECT_SOUND_BTN_CANCEL:
            case ESOUND_TYPE.E_EFFECT_SOUND_BTN_COMMON:
            case ESOUND_TYPE.E_EFFECT_SOUND_BTN_CONFIRM:
            case ESOUND_TYPE.E_EFFECT_SOUND_BTN_EXPLODE:
            case ESOUND_TYPE.E_EFFECT_SOUND_BTN_FALL:
            case ESOUND_TYPE.E_EFFECT_SOUND_BTN_LEFT:
            case ESOUND_TYPE.E_EFFECT_SOUND_BTN_RIGHT:
            case ESOUND_TYPE.E_EFFECT_SOUND_BTN_INTRO:
                {
                    int clipIdx = (int)soundType - 10;
                    audioClip = effectSoundClips[clipIdx];
                    break;
                }
            default:
                break;
        }

        return audioClip;
    }


    [ContextMenu("LoadBGMClips")]
    public void LoadBGMClips()
    {
        bgmClips = Resources.LoadAll<AudioClip>("Sounds/BGM");
    }
    [ContextMenu("LoadEffectSoundClips")]
    public void LoadEffectSoundClips()
    {
        effectSoundClips = Resources.LoadAll<AudioClip>("Sounds/EffectSound");
    }

}
