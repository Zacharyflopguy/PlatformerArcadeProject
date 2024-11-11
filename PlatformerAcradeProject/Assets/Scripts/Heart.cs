using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{

    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Image heartImage;
    [Tooltip("Will be full if health is equal to or greater than this value")]
    public int heartNumber;
    public MasterControl mainControl;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        heartImage.sprite = mainControl.lives >= heartNumber ? fullHeart : emptyHeart;
    }
}
