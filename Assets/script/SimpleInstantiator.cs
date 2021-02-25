using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInstantiator : MonoBehaviour
{
    //キャラ生成装置.
    public GameObject InstObj;
    public float spawntime = 3f;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("InstEnemy", 0f, spawntime);
    }

    void InstEnemy()
    {
        if (InstObj)
        {
            Instantiate(InstObj, transform.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
