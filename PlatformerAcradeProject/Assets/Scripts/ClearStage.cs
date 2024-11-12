using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ClearStage : MonoBehaviour
{
    int timer;
    public GameObject spawn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        timer = 30;
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (timer != 0)
        {
            timer--;
        }
        else
        {
            Vector3 temp = spawn.transform.position;
            spawn.transform.position = this.gameObject.transform.position;
            this.gameObject.transform.position = temp;
            col.gameObject.GetComponent<Character>().findChild(col.gameObject).levelComplete();
        }
    }
}
