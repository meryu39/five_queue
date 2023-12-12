using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx { Adrenaline, BasicAttack, BasicAttackWind, BloodPack, BossAttack, 
                      BossDie, BossShaking, BossSummon, BrokenPipe, Cancel, CurvetDamaged, 
                      DashZombieDetected, Door, ElevatorMove, FireExtinguisher, HeavyZombieDetected, 
                      HumanZombieDetected, ItemDump, ItemUse, MenuSelect, MiniButton, Scalpel, SlotFill, 
                      TrapZombieDetected, TrapZombieSplit, UseActive1, UseActive3, UseActive4, 
                      UseDash, UseUlt1, UseUlt2
    }


    private void Awake()
    {
        instance = this;
        Init();
        DontDestroyOnLoad(instance);
    }

    void Init()
    {
        //배경을 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmaPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;

        //효과음 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for(int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake=false;
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    public void PlaySfx(Sfx sfx)
    {
        for(int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)    continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }
}
