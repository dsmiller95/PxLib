using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PxAudioPlayer
{
    private static List<AudioSource> SfxPlayers
    {
        get
        {
            if(_sfxPlayers != null && _sfxPlayers.Count > 0 && _sfxPlayers[0] != null)
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
    public static AudioSource BgmBlendPlayer => _bgmBlendPlayer ? _bgmBlendPlayer : PxAudioSourceManager.Instance.BgmBlendPlayer;
    private static AudioSource _bgmBlendPlayer;
    private static PxAudioSourceManager _audioSourceManager => PxAudioSourceManager.Instance;

    public static void AssignAudioSources(List<AudioSource> sfx, AudioSource bgm, AudioSource bgmBlend)
    {
        _sfxPlayers = sfx;
        _bgmPlayer = bgm;
        _bgmBlendPlayer = bgmBlend;
    }
    public static void Init()
    {
        if (_audioSourceManager != null)
        {
            Debug.LogWarning("Cannot create more than one Audio Manager!");
            return;
        }
        // will self-assign to its own Instance property when complete
        PxAudioSourceManager.CreateInstance();
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
    
    /// <summary>
    /// Start playing a new bgm at 0 volume, ramping up to full volume over blendTime seconds,
    /// at the same time decreasing the volume of any already playing background music,
    /// eventually playing only the new background music.
    /// </summary>
    public static void BlendBgm(AudioClip bgm, float blendTime, bool blendVolume = true, bool loop = true)
    {
        if (BgmPlayer.clip == bgm)
            return;
        if (_bgmCoroutine != null)
        {
            PxAudioSourceManager.Instance.StopCoroutine(_bgmCoroutine);
        }
        _bgmCoroutine = _audioSourceManager.StartCoroutine(IEBlendBgm(bgm, blendTime, blendVolume, loop));
    }
    private static IEnumerator IEBlendBgm(AudioClip bgm, float blendTime, bool blendVolume, bool loop)
    {
        var baseVolume = BgmPlayer.volume;
        var baseBlendVolume = BgmBlendPlayer.volume;
        
        BgmBlendPlayer.clip = bgm;
        BgmBlendPlayer.loop = loop;
        if (bgm != null)
        {
            BgmBlendPlayer.Play();
        }
        else
        {
            BgmBlendPlayer.Stop();
        }

        if (blendVolume)
        {
            BgmBlendPlayer.volume = 0;
            var startTime = Time.time;
            float timeSinceStart = 0;
            while ((timeSinceStart = Time.time - startTime) < blendTime)
            {
                var normalizedTime = timeSinceStart / blendTime;
                BgmBlendPlayer.volume = baseBlendVolume * normalizedTime;
                BgmPlayer.volume = baseVolume * (1 - normalizedTime);
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(blendTime);
        }
        
        BgmPlayer.Stop();
        BgmPlayer.clip = bgm;
        BgmPlayer.loop = loop;
        BgmPlayer.volume = baseVolume;
        BgmPlayer.time = BgmBlendPlayer.time;
        if(bgm != null) BgmPlayer.Play();
        
        BgmBlendPlayer.volume = baseBlendVolume;
        BgmBlendPlayer.Stop();
        _bgmCoroutine = null;
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
    public static void PlaySfx(AudioClip sfx, bool allowOverlap = true)
    {
        if (!allowOverlap)
        {
            // search for a player playing this effect already
            foreach (var player in SfxPlayers)
            {
                if (player.isPlaying && player.clip == sfx)
                {
                    return;
                }
            }
        }

        AudioSource sfxPlayer = null;
        for (int i = 0; i < SfxPlayers.Count - 1; i++)
        {
            if(SfxPlayers[i].isPlaying) continue;
            sfxPlayer = SfxPlayers[i];
            break;
        }
        if (sfxPlayer == null) return;

        sfxPlayer.clip = sfx;
        sfxPlayer.pitch = _audioSourceManager.GetSemitoneMultiplier();
        sfxPlayer.Play();
    }
    
    public static void PlaySfxDelayed(AudioClip sfx, float delay, bool allowOverlap = true)
    {
        _audioSourceManager.StartCoroutine(IEPlaySfxDelayed(sfx, delay, allowOverlap));
    }
    
    private static IEnumerator IEPlaySfxDelayed(AudioClip sfx, float delay, bool allowOverlap)
    {
        yield return new WaitForSeconds(delay);
        PlaySfx(sfx, allowOverlap);
    }

    #endregion
}
