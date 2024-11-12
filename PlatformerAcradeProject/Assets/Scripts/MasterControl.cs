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
    private int realStage;

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
        
        //Converts the stage number to the actual stage number
        realStage = (((stage - 1) / 2) + 1);
        
        if(stage % 2 == 1)
        {
            if(stage <= 15)
            {
                materials = 5 - (stage / 4);
            }
            else
            {
                materials = 2;
            }
        }
    }
    
}
