using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterControl : MonoBehaviour
{
    int score;
    int lives;
    // Start is called before the first frame update
    void Start()
    {
        lives = 3;
        score = 0;
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
}
