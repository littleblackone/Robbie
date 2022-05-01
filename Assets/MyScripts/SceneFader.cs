using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    Animator fader;
    int faderID;
    private void Start()
    {
        fader = GetComponent<Animator>();
        faderID = Animator.StringToHash("Fade");
        GameManager.RegisterSceneFader(this);
    }
    public void Playfader()
    {
        fader.SetTrigger(faderID);
    }
}
