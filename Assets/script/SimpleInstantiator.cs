using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInstantiator : MonoBehaviour
{
    //キャラ生成装置.
    public Systems sys;
    public GameObject[] InstObj;
    public GameObject[] InstItem;
    public GameObject[] InstEnemys;
    public float range;
    public float spawntime = 3f;
    List<GameObject> SpawnedList = new List<GameObject>(); 
    // Start is called before the first frame update
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        int Circleresol = 32;
        Vector3 StartVec, EndVec;
        for (int i = 0; i < Circleresol; i++)
        {
            StartVec = transform.position + Quaternion.AngleAxis(i/360f,Vector3.up) * Vector3.forward * range;
            EndVec = transform.position + Quaternion.AngleAxis((i + 1) / 360f, Vector3.up) * Vector3.forward * range;
            Gizmos.DrawLine(StartVec,EndVec);
        }
        Gizmos.DrawWireSphere(transform.position, range);
    }
    
    void Start()
    {
        InvokeRepeating("InstEnemy", 0f, spawntime);
    }

    void InstEnemy()
    {
        float randinstrange = Random.Range(0f,range);
        float randRotate = Random.Range(0f, 360f);
        Vector3 instPos = transform.position + Quaternion.AngleAxis(randRotate, Vector3.up) * Vector3.forward * randinstrange ;
        if (InstObj.Length != 0 && SpawnedList.Count < Mathf.Ceil(sys.CurrentMaxCharacterNum))
        {
            SpawnedList.Add(Instantiate(InstObj[Random.Range(0,InstObj.Length)], instPos, Quaternion.Euler(new Vector3(0.5f - Random.value, 0.5f - Random.value, 0.5f - Random.value).normalized * 360f)));
        }
    }

    float spawntimes = 0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (sys.RestTime > 0f && sys.Countdowns < 0f)
        {

            if (spawntimes > spawntime)
            {
                spawntimes = 0f;
                InstEnemy();
            }
            else {
                spawntimes += Time.fixedDeltaTime;
            }

            int i = 0;
            foreach (GameObject obj in SpawnedList)
            {
                if (obj == null)
                {
                    SpawnedList.RemoveAt(i);
                    sys.KillNum += 1;
                    sys.ItemKillNum += 1;
                }
                i++;
            }
            if (sys.ItemKillNum >= sys.ItemAppearNum)
            {
                sys.ItemKillNum = 0;
                float randinstrange = Random.Range(0f, range);
                float randRotate = Random.Range(0f, 360f);
                Vector3 instPos = transform.position + Quaternion.AngleAxis(randRotate, Vector3.up) * Vector3.forward * randinstrange;
                Instantiate(InstItem[Random.Range(0, InstItem.Length)],
                         instPos, Quaternion.Euler
                         (new Vector3(0.5f - Random.value, 0.5f - Random.value, 0.5f - Random.value).normalized * 360f));
            }
            if (Mathf.Pow(sys.CurrentTime / 1000f, 2f) > Random.Range(0f, 2f))
            {
                float randinstrange = Random.Range(0f, range);
                float randRotate = Random.Range(0f, 360f);
                Vector3 instPos = transform.position + Quaternion.AngleAxis(randRotate, Vector3.up) * Vector3.forward * randinstrange;
                if (InstEnemys.Length != 0)
                    Instantiate(InstEnemys[Random.Range(0, InstEnemys.Length)],
                             instPos, Quaternion.Euler
                             (new Vector3(0.5f - Random.value, 0.5f - Random.value, 0.5f - Random.value).normalized * 360f));
            }
        }
    }
    
}
