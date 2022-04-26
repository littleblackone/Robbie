using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Orbs : MonoBehaviour
{
    int player;
    public GameObject orbFx;
    void Start()
    {
        player = LayerMask.NameToLayer("Player");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer==player)
        {
            gameObject.SetActive(false);
            Instantiate(orbFx, transform.position, transform.rotation);
            AudioManager.PlayorbAudio();
        }
    }

}
