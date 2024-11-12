using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    AudioSource music;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        music = GetComponent<AudioSource>();
    }
    private void Awake()
    {
        GameObject obj = GameObject.Find("Music");
        if(obj != null && obj != gameObject)
        {
            Destroy(obj);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void stopMusic()
    {
        music.Pause();
    }
    public void startMusic()
    {
        music.Play();
    }
}
