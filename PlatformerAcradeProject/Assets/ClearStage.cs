using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearStage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("collision");
        Debug.Log(col.gameObject);
        Debug.Log(col.gameObject.GetComponent<Character>());
        col.gameObject.GetComponent<Character>().levelComplete();
    }
}
