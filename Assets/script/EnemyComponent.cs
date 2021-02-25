using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyComponent : MonoBehaviour
{
    public int Health = 1;
    public GameObject[] DeathEffect;
    
    float invisibletime = 0.5f, currentinvistime = 0f ;
    Rigidbody rigidbodys;

    private void OnTriggerEnter(Collider other)
    {
        if (currentinvistime > invisibletime && other.gameObject.tag == "Player")
        {
            Health += -1;
            currentinvistime = Time.deltaTime;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        rigidbodys = GetComponent<Rigidbody>();
        Vector3 randVec = new Vector3(Random.Range(-1f,1f),0f, Random.Range(-1f, 1f)).normalized * 10f;
        rigidbodys.AddForce(randVec,ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentinvistime += Time.fixedDeltaTime;
        if (Health <= 0) {
            foreach (GameObject dEffect in DeathEffect) {
                if (dEffect != null) {
                    Instantiate(dEffect,transform.position,Quaternion.identity);
                }
            }
            Destroy(gameObject);
        }
    }
}
