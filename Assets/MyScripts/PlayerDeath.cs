using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public GameObject deathFx;
    int layerID;
    void Start()
    {
        layerID = LayerMask.NameToLayer("Traps");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer==layerID)
        {
            Instantiate(deathFx, transform.position, transform.rotation);
            gameObject.SetActive(false);
            AudioManager.PlaydeathAudio();
            GameManager.Deathload();
        }
    }
    
}
