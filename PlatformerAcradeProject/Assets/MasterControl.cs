using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterControl : MonoBehaviour
{
    int score;
    int lives;
    int materials;
    int stage = 1;
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
    public bool useMaterial()
    {
        if(materials > 0)
        {
            materials--;
            return true;
        }
        else
        {
            return false;
        }
    }
}
