using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DeadbodyComponent : MonoBehaviour
{
    public float remaintime = 5f;
    public float Power = 5f;

    float maxremain;
    Vector3 maxscale;
    // Start is called before the first frame update
    void Start()
    {
        maxscale = transform.localScale;
        maxremain = remaintime;
        transform.rotation = Random.rotation;
        GetComponent<Rigidbody>().velocity =
            new Vector3(0.5f - Random.value, 0f, 0.5f - Random.value).normalized * Power + new Vector3(0f, Power, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (remaintime <= 0f) {
            Destroy(gameObject);
        }
        remaintime -= Time.deltaTime;
        transform.localScale = maxscale * Mathf.Pow(Mathf.Clamp(remaintime,0f,Mathf.Infinity) / maxremain, 0.5f);
    }
}
