using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PxAudioPlayer
{
    private static List<AudioSource> SfxPlayers
    {
        get
        {
            if(_sfxPlayers != null)
            {
                return _sfxPlayers;
            }
            _sfxPlayers = PxAudioSourceManager.Instance.SfxPlayers;
            return _sfxPlayers;
        }
    }
    private static List<AudioSource> _sfxPlayers;
    public static AudioSource BgmPlayer => _bgmPlayer ? _bgmPlayer : PxAudioSourceManager.Instance.BgmPlayer;
    private static AudioSource _bgmPlayer;
    private static PxAudioSourceManager _audioSourceManager;

    public static void AssignAudioSources(List<AudioSource> sfx, AudioSource bgm)
    {
        _sfxPlayers = sfx;
        _bgmPlayer = bgm;
    }
    public static void Init()
    {
        if (_audioSourceManager != null)
        {
            Debug.LogWarning("Cannot create more than one Audio Manager!");
            return;
        }            
        _audioSourceManager = PxAudioSourceManager.CreateInstance();
    }

    #region BGM
    private static Coroutine _bgmCoroutine;
    
    public static void PlayBgmIfNotPlaying(AudioClip bgm, bool loop = true)
    {
        if (BgmPlayer.clip == bgm && BgmPlayer.isPlaying)
        {
            BgmPlayer.loop = loop;
            return;
        }
        BgmPlayer.clip = bgm;
        BgmPlayer.Play();
        BgmPlayer.loop = loop;
    }
    public static void PlayBgm(AudioClip bgm, bool loop = true)
    {
        BgmPlayer.clip = bgm;
        BgmPlayer.Play();
        BgmPlayer.loop = loop;
    }
    public static void PlayBgmWithIntro(AudioClip intro, AudioClip loop)
    {
        if (BgmPlayer.clip == intro || BgmPlayer.clip == loop)
            return;
        if (_bgmCoroutine != null)
        {
            PxAudioSourceManager.Instance.StopCoroutine(_bgmCoroutine);
        }
        _bgmCoroutine = _audioSourceManager.StartCoroutine(IEPlayBgmWithIntro(intro, loop));
    }

    private static IEnumerator IEPlayBgmWithIntro(AudioClip intro, AudioClip loop)
    {
        BgmPlayer.clip = intro;
        BgmPlayer.Play();
        yield return new WaitForSeconds(intro.length);
        BgmPlayer.clip = loop;
        BgmPlayer.Play();
        _bgmCoroutine = null;
    }
    #endregion

    #region SFX
    public static void PlaySfx(AudioClip sfx)
    {
        for (int i = 0; i < SfxPlayers.Count - 1; i++)
        {
            if (!SfxPlayers[i].isPlaying)
            {
                SfxPlayers[i].clip = sfx;
                SfxPlayers[i].Play();
                return;
            }
        }
    }
    #endregion
}
