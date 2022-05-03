using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    SceneFader fader;
    Door door;
    List<Orbs> orb;
    int deathNum;
    float time;
    bool gameover;
    private void Awake()
    {
        if (instance!=null)//只要是管理器，类似gamemanager，audiomanager，就要写这个，不然场景生成一次就会多一个管理器。UImanager里还要加上destroy（gameobject）;来删除上次场景的UI。
        {         
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
        orb = new List<Orbs>();
    }
  
    public static void Orbseat(Orbs obj)
    {
        if (!instance.orb.Contains(obj))
            return;
        instance.orb.Remove(obj);
        
        if (instance.orb.Count==0)
        {
            instance.door.DoorOpen();          
        }
        UIManager.OrbUi(instance.orb.Count);
    }
    public static void RegisterSceneFader(SceneFader obj)
    {
        instance.fader = obj;
    }
    public static void RegisterDoorOpen(Door obj)
    {
        instance.door = obj;
    }
    public static void RegisterOrbs(Orbs obj)
    {
        if (!instance.orb.Contains(obj))
        {
          instance.orb.Add(obj);
        }
        UIManager.OrbUi(instance.orb.Count);
    }
    private void Update()
    {
        if (gameover)
        {
            return;
        }
        if (!gameover)
        {
        time += Time.deltaTime;     
        }
        UIManager.timeUi(time); 
    }
    public static void gameoverUi()
    {
        instance.gameover = true;
        UIManager.gameOverUi();
    }
    public static bool isgameover()
    {
        return instance.gameover;
    }
    public static void Deathload()
    {
        instance.deathNum++;
        UIManager.deathUi(instance.deathNum);
        instance.fader.Playfader();
        instance.Invoke("Sceneload", 2.2f);
    }
     void Sceneload()
    {
        instance.orb.Clear(); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
