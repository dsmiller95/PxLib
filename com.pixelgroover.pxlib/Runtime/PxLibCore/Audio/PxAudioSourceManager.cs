using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PxAudioSourceManager : MonoBehaviour
{
    public const float MAX_BGM_VOLUME = 1.0f;
    public const float MAX_SFX_VOLUME = 1.0f;
    private const int NUMBER_OF_SFX_CHANNELS = 8;
    private static PxAudioSourceManager _instance;
    public static PxAudioSourceManager Instance => _instance;
    public List<AudioSource> SfxPlayers => _sfxPlayers;
    public AudioSource BgmPlayer => _bgmPlayer;
    public AudioSource BgmBlendPlayer => _bgmBlendPlayer;

    [SerializeField] private AudioMixerGroup sfxMixerGroup;
    [SerializeField] private AudioMixerGroup bgmMixerGroup;
    
    [SerializeField] private List<AudioSource> _sfxPlayers;
    [SerializeField] private AudioSource _bgmPlayer;
    [SerializeField] private AudioSource _bgmBlendPlayer;
    
    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(this);
            return;
        }
        _instance = this;
        StartUpHacks();
        Debug.Log("Started Up");
    }
    public static PxAudioSourceManager CreateInstance()
    {
        if (_instance != null)
        {
            Debug.LogWarning("Cannot create more than one instance of PxAudioSourceManager!");
            return null;
        }
        var audioManager = new GameObject("AudioMangaer", typeof(PxAudioSourceManager)).GetComponent<PxAudioSourceManager>();
        audioManager.StartCoroutine(audioManager.IECreateInstance());
        return audioManager;
    }
    private List<AudioSource> CreateSfxAudioSources()
    {
        var sfxChannels = new List<AudioSource>();
        for(int i = 0; i < NUMBER_OF_SFX_CHANNELS; i++)
        {
            var audioSource = _instance.gameObject.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = sfxMixerGroup;
            sfxChannels.Add(audioSource);
        }
        return sfxChannels;
    }
    private AudioSource CreateBgmAudioSource()
    {
        var audioSource = _instance.gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = bgmMixerGroup;
        audioSource.volume = MAX_BGM_VOLUME;
        return audioSource;
    }
    private IEnumerator IECreateInstance()
    {
        _instance._sfxPlayers = CreateSfxAudioSources();
        yield return null;
        _instance._bgmPlayer = CreateBgmAudioSource();
        _instance._bgmBlendPlayer = CreateBgmAudioSource();
        yield return null;
        PxAudioPlayer.AssignAudioSources(_instance._sfxPlayers, _instance._bgmPlayer, _instance._bgmBlendPlayer);
    }
    private void StartUpHacks()
    {
        _instance._sfxPlayers = CreateSfxAudioSources();
        _instance._bgmPlayer = CreateBgmAudioSource();
        _instance._bgmBlendPlayer = CreateBgmAudioSource();
    }
}
