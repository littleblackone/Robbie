using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    SceneFader fader;
    List<Orbs> orb;
    public int orbNum;
    public int deathNum;
    private void Awake()
    {
        if (instance!=null)
        {         
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
        orb = new List<Orbs>();
    }
    private void Update()
    {
        orbNum = instance.orb.Count;
    }
    public static void Orbseat(Orbs obj)
    {
        if (instance.orb!=null)
        {
            instance.orb.Remove(obj);
        }
        
    }
    public static void RegisterSceneFader(SceneFader obj)
    {
        instance.fader = obj;
    }
    public static void RegisterOrbs(Orbs obj)
    {
        if (!instance.orb.Contains(obj))
        {
          instance.orb.Add(obj);
        }
        
    }
     public static void Deathload()
    {
        instance.deathNum++;
        instance.fader.Playfader();
        instance.Invoke("Sceneload", 2.2f);
    }
     void Sceneload()
    {
        instance.orb.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
