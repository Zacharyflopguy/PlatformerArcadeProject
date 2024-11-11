using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public Character other;
    public GameObject otherCharacter;
    public GameObject spawnPoint;
    public GameObject despawnPoint;
    public GameObject master;
    public MasterControl mainControl;
    // Start is called before the first frame update
    void Start()
    {

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

    public Character findChild(GameObject obj)
    {
        foreach(Component i in obj.GetComponents<Component>())
        {
            if (i is BuilderController)
            {
                return obj.GetComponent<BuilderController>();
            }else if(i is SlimeController)
            {
                return obj.GetComponent<SlimeController>();
            }
        }
        return null;
    }
}
