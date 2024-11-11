using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    bool slime = false;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (slime)
        {
            Destroy(this.gameObject, 5);
        }
    }
    public void slimed()
    {
        spriteRenderer.sprite = sprites[1];
        slime = true;
    }
}
