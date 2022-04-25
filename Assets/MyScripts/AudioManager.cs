using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    static AudioManager manager;

    [Header("������Ч")]
    public AudioClip AmbientAudio;//������Ч
    public AudioClip BkAudio;//��������

    [Header("������Ч")]
    public AudioClip[] WalkstepAudio;//��·����������4��������洢
    public AudioClip[] CrouchstepAudio;//������·��������ͬ����4��
    public AudioClip JumpAudio;//��Ծ������
    public AudioClip JumpyellAudio;//��Ծʱ���������

    AudioSource Ambient;
    AudioSource Bk;
    AudioSource Fx;
    AudioSource Player;
    AudioSource Playervoice;
     void Awake()
    {
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
  
}
