using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MasterControl : MonoBehaviour
{
    int score;
    public int lives;
    public int materials;
    int stage = 1;
    int extraLives = 0;
    private int realStage = 1;
    int map = 0;
    int soundLoops;
    public GameObject bonus;
    public Grid grid;
    AudioSource sound;
    public AudioClip death;
    public AudioClip point;
    public AudioClip crossed;


    [Header("UI Elements")]
    public TextMeshProUGUI StageText;
    public TextMeshProUGUI MaterialsText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI scoreText;
    public Canvas canvas;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(canvas);
    }

    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
        lives = 3;
        score = 0;
        materials = 5;
    }

    // Update is called once per frame
    void Update()
    {
        StageText.text = "Stage: " + realStage;
        scoreText.text = "Score: " + score;
        MaterialsText.text = materials.ToString();
        healthText.text = lives.ToString();
    }

    public void addScore(int points)
    {
        score += points;
        soundLoops = points / 200;
        Thread playPointSound = new Thread(playScoreSound);
        if(extraLives * 70000 + 30000 <= score)
        {
            lives++;
            extraLives++;
        }
    }
    public void loseLife(Character character)
    {
        sound.PlayOneShot(death);
        Thread.Sleep(3213);
        lives--;
        if(lives > 0)
        {
            character.spawn();
        }
    }
    public void nextStage()
    {
        sound.PlayOneShot(crossed);
        Thread.Sleep(1306);
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
                SceneManager.LoadScene("Stage3");
                break;
            case 1:
                SceneManager.LoadScene("Stage2");
                break;
            case 2:
                SceneManager.LoadScene("Slime game");
                break;
        }
    }
    public void spawnBonus()
    {
        GameObject item = Instantiate(bonus, grid.CellToWorld(
            grid.WorldToCell(new Vector3(Random.Range(-9.0f, 9.0f), Random.Range(-5.0f, 5.0f), 0))), Quaternion.identity);
        item.GetComponent<Bonus>().main = this.gameObject;
    }
    void playScoreSound()
    {
        for (int i = 0; i <= soundLoops; i++)
        {
            sound.PlayOneShot(point);
            Thread.Sleep(340);
        }
    }
}
