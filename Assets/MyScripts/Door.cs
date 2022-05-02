using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator doorOpen;
    int doorId;
    private void Start()
    {
        doorOpen = GetComponent<Animator>();
        doorId = Animator.StringToHash("Open");
        GameManager.RegisterDoorOpen(this);       
    }
    public void DoorOpen()
    {
        doorOpen.SetTrigger(doorId);
        Invoke("DooropenSound", 1.2f);
    }
    void DooropenSound()
    {
      AudioManager.PlayDooropenAudio();
    }
}
