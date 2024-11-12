using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    bool slimed;
    int type;
    public Sprite[] sprites;
    public Sprite[] slimedSprites;
    public GameObject main;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(Random.Range(0, 2) == 0)
        {
            slimed = true;
        }
        type = Random.Range(0, 5);
        if (slimed)
        {
            spriteRenderer.sprite = slimedSprites[type];
        }
        else
        {
            spriteRenderer.sprite = sprites[type];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject obj = col.gameObject;
        try
        {
            if (obj.GetComponent<Character>().findChild(obj) is SlimeController == slimed)
            {
                main.GetComponent<MasterControl>().addScore((type + 1) * 100);
                Destroy(this.gameObject);
            }
            else
            {
                if (slimed)
                {
                    obj.GetComponent<BuilderController>().die();
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
        }
        catch
        {
            Destroy(this.gameObject);
            main.GetComponent<MasterControl>().spawnBonus();
        }
    }
}
