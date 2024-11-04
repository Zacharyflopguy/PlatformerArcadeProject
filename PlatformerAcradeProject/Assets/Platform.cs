using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void slimed()
    {
        spriteRenderer.sprite = sprites[1];
    }
}
