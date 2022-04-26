using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    static AudioManager manager;

    [Header("环境音效")]
    public AudioClip AmbientAudio;//环境音效
    public AudioClip BkAudio;//背景音乐

    [Header("人物音效")]
    public AudioClip[] WalkstepAudio;//走路的声音，有4个用数组存储
    public AudioClip[] CrouchstepAudio;//蹲下走路的声音，同样有4个
    public AudioClip JumpAudio;//跳跃的声音
    public AudioClip JumpyellAudio;//跳跃时人物的声音
    public AudioClip deathAudio;//死亡声音
    public AudioClip deathAudiovoice;//死亡叫声
    public AudioClip orbAudiovoice;//收集到宝珠的叫声

    [Header("FX音效")]
    public AudioClip deathFx;//归还宝珠声音
    public AudioClip orbFx;//收集到宝珠的声音

    AudioSource Ambient;//环境声
    AudioSource Bk;//背景声
    AudioSource Fx;//特效声
    AudioSource Player;//人物脚本声
    AudioSource Playervoice;//人物叫声
     void Awake()
    {
        if (manager!=null)
        {
            Destroy(gameObject);
            return;//return不能掉
        }
        manager = this;
        DontDestroyOnLoad(gameObject);
        Ambient = gameObject.AddComponent<AudioSource>();
        Bk = gameObject.AddComponent<AudioSource>();
        Fx = gameObject.AddComponent<AudioSource>();
        Player = gameObject.AddComponent<AudioSource>();
        Playervoice = gameObject.AddComponent<AudioSource>();
        PlayAmbientAudio();
    }

    void PlayAmbientAudio()
    {
        manager.Ambient.clip = manager.AmbientAudio;
        manager.Ambient.loop = true;
        manager.Ambient.Play();

        manager.Bk.clip = manager.BkAudio;
        manager.Bk.loop = true;
        manager.Bk.Play();
    }
    public static void PlaystepAudio()
    {
        int index1 = Random.Range(0, manager.WalkstepAudio.Length);
        manager.Player.clip = manager.WalkstepAudio[index1];
        manager.Player.Play();   
    }
    public static void PlayCrouchstepAudio()
    {
        int index2 = Random.Range(0, manager.CrouchstepAudio.Length);
        manager.Player.clip = manager.CrouchstepAudio[index2];
        manager.Player.Play();
    }

    public static void PlayjumpAudio()
    {
        manager.Player.clip = manager.JumpAudio;
        manager.Player.Play();

        manager.Playervoice.clip = manager.JumpyellAudio;
        manager.Playervoice.Play();
    }
  public static void PlaydeathAudio()
    {
        manager.Player.clip = manager.deathAudio;
        manager.Player.Play();

        manager.Player.clip  = manager.deathAudiovoice;
        manager.Player.Play();

        manager.Fx.clip = manager.deathFx;
        manager.Fx.Play();
    }
    public static void PlayorbAudio()
    {
        manager.Fx.clip = manager.orbFx;
        manager.Fx.Play();

        manager.Playervoice.clip = manager.orbAudiovoice;
        manager.Playervoice.Play();
    }
}
