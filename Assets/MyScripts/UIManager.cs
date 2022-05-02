using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    static UIManager uI;
    public TextMeshProUGUI orbtext, deathtext, timetext, gameovertext;
    private void Awake()
    {
        if (uI!=null)
        {
            return;
        }
        uI = this;
        DontDestroyOnLoad(this);
    }
    public static void OrbUi(int orbsnum)
    {
        uI.orbtext.text = orbsnum.ToString();
    }
    public static void deathUi(int deathnum)
    {
        uI.deathtext.text = deathnum.ToString();
    }
    public static void timeUi(float time)
    {
        int minute = (int)(time / 60);
        int second = (int)(time % 60);
        uI.timetext.text = minute.ToString("00") + ":" + second.ToString("00");
    }
    public static void gameOverUi()
    {
        
    }
}
