using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MasterControl : MonoBehaviour
{
    public int score;
    public int lives;
    public int materials;
    int stage = 1;
    int extraLives = 0;
    private int realStage = 1;
    int map = 0;
    int soundLoops;
    int soundTimer;
    GameObject music;
    public GameObject bonus;
    public Grid grid;
    AudioSource sound;
    public AudioClip death;
    public AudioClip point;
    public AudioClip crossed;
    private float timer = 500f;
    public InputAction escape;


    [Header("UI Elements")]
    public TextMeshProUGUI StageText;
    public TextMeshProUGUI MaterialsText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI scoreText;
    public Canvas canvas;
    public TextMeshProUGUI timerText;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(canvas);
        
        //Handle multiple Mains on loop around
        GameObject mainObject = GameObject.Find("Main");
        music = GameObject.Find("Music");
        if (mainObject != null && mainObject != gameObject)
        {
            Destroy(mainObject);
        }
        
        GameObject obj = GameObject.Find("Canvas");
        if (obj != null && obj != canvas.gameObject)
        {
            Destroy(obj);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
        escape.Enable();
        lives = 3;
        score = 0;
        materials = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if(escape.ReadValue<float>() == 1)
        {
            SceneManager.LoadScene("MainMenu");
        }
        if (soundLoops > 0)
        {
            if (soundTimer == 0)
            {
                sound.PlayOneShot(point);
                soundTimer = 12;
                soundLoops--;
            }
            else
            {
                soundTimer--;
            }
        }
        sound = GetComponent<AudioSource>();
        if (sound == null)
        {
            Debug.LogError("AudioSource component is missing on this GameObject.");
        }
        
        StageText.text = "Stage: " + realStage;
        scoreText.text = "Score: " + score;
        
        //Timer Logic
        timer -= Time.deltaTime;
        Mathf.Clamp(timer, 0, 999999999);
        timerText.text = "Time: " + timer.ToString("F2");
        
        MaterialsText.text = materials.ToString();
        healthText.text = lives.ToString();
    }

    public void addScore(int points)
    {
        score += points;
        soundLoops = (points / 200)+1;
        soundTimer = 12;
        if(extraLives * 70000 + 30000 <= score)
        {
            lives++;
            extraLives++;
        }
    }
    public void loseLife(Character character)
    {
        sound.PlayOneShot(death);
        music.GetComponent<MusicController>().stopMusic();
        Thread.Sleep(3213);
        music.GetComponent<MusicController>().startMusic();
        lives--;
        if(lives > 0)
        {
            character.spawn();
        }

        if (lives <= 0)
        {
            SceneManager.LoadScene("Leaderboard");
        }
    }
    public void nextStage()
    {
        sound.PlayOneShot(crossed);
        music.GetComponent<MusicController>().stopMusic();
        Thread.Sleep(1306);
        music.GetComponent<MusicController>().startMusic();
        stage++;
        addScore(2000);
        
        if(stage % 2 == 1)
        {
            realStage++;
            if(stage == 17)
            {
                nextLevel();
            }
            if(stage <= 15 - ((map / 3) * 4))
            {
                materials = 5 - (map / 3) - (stage / 4);
            }
            else
            {
                materials = 2;
            }
        }
        else
        {
            if(stage == 8)
            {
                spawnBonus();
            }
        }
    }
    public void nextLevel()
    {
        map++;
        stage = 1;
        
        switch (map%3)
        {
            case 0:
                SceneManager.LoadScene("Slime game");
                break;
            case 1:
                SceneManager.LoadScene("Stage2");
                break;
            case 2:
                SceneManager.LoadScene("Stage3");
                break;
        }
    }
    public void spawnBonus()
    {
        GameObject item = Instantiate(bonus, grid.CellToWorld(
            grid.WorldToCell(new Vector3(Random.Range(-9.0f, 9.0f), Random.Range(-5.0f, 5.0f), 0))), Quaternion.identity);
        item.GetComponent<Bonus>().main = this.gameObject;
    }
}
