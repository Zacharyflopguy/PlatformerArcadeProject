using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MasterControl : MonoBehaviour
{
    int score;
    public int lives;
    public int materials;
    int stage = 1;
    int extraLives = 0;
    private int realStage = 1;
    int map = 0;
    public GameObject goal;
    public GameObject spawn;

    [Header("UI Elements")]
    public TextMeshProUGUI StageText;
    public TextMeshProUGUI MaterialsText;
    public TextMeshProUGUI healthText;
    // Start is called before the first frame update
    void Start()
    {
        lives = 3;
        score = 0;
        materials = 5;
    }

    // Update is called once per frame
    void Update()
    {
        StageText.text = "Stage: " + realStage;
        MaterialsText.text = materials.ToString();
        healthText.text = lives.ToString();
    }

    public void addScore(int points)
    {
        score += points;
        if(extraLives * 70000 + 30000 <= score)
        {
            lives++;
            extraLives++;
        }
    }
    public void loseLife(Character character)
    {
        lives--;
        if(lives > 0)
        {
            character.spawn();
        }
    }
    public void nextStage()
    {
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
            Vector3 temp = spawn.transform.position;
            spawn.transform.position = goal.transform.position;
            goal.transform.position = temp;
        }
    }
    public void nextLevel()
    {
        map++;
        stage = 1;
    }
}
