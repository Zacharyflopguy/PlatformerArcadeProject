using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    Character other;
    public GameObject otherCharacter;
    public GameObject spawnPoint;
    public GameObject despawnPoint;
    public GameObject master;
    MasterControl mainControl;
    // Start is called before the first frame update
    void Start()
    {
        mainControl = master.GetComponent<MasterControl>();
        other = otherCharacter.GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void die()
    {
        mainControl.loseLife(this);
    }
    public void levelComplete()
    {
        other.spawn();
        despawn();
    }
    public abstract void spawn();
    public abstract void despawn();
}
