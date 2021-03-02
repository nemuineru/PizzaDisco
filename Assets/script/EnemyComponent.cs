using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyComponent : MonoBehaviour
{
    public float remaintime = 45f;
    public float speed = 10f;
    public int Health = 1;
    public int BaseScore = 100;
    public float heatlev = 0.1f;
    public float Bouncy = 0.4f;
    public bool isOndeadBounce = true;
    public bool isReallyEnemy = false;
    public GameObject[] DamageEffect;
    public GameObject[] DeathEffect;

    public float invisibletime = 0.5f, currentinvistime = 1000f;
    Rigidbody rigidbodys;
    GameObject Player;
    Systems sys;

    //敵の爆発またはプレイヤーに対し当たったとき...
    private void OnTriggerEnter(Collider other)
    {
        if (sys.RestTime > 0f)
        {
            if (currentinvistime > invisibletime && other.gameObject.tag == "Player")
            {
                PlayerMovement plmove = other.transform.root.GetComponent<PlayerMovement>();
                if (plmove.isRotating)
                {
                    foreach (GameObject effs in DamageEffect)
                    {
                        if (effs != null)
                        {
                            Instantiate(effs, other.ClosestPoint(plmove.transform.position), Quaternion.identity);
                        }
                    }
                    Health -= plmove.Power;
                    currentinvistime = Time.deltaTime;
                    if ((Health > 0 || isOndeadBounce && Health >= 0) && plmove.PowerUpTime <= 0f)
                    {
                        Rigidbody rigplayer = other.transform.root.GetComponent<Rigidbody>();
                        rigplayer.velocity += -rigplayer.velocity * Bouncy;
                    }
                }
            }
            if (other.gameObject.tag == "explod")
            {
                Health += -1000;
                currentinvistime = Time.deltaTime;
            }
        }
    }

    private void Awake()
    {
        sys = GameObject.FindGameObjectWithTag("system").GetComponent<Systems>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform.root.gameObject;   
        rigidbodys = GetComponent<Rigidbody>();
        Vector3 randVec = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized * speed;
        rigidbodys.AddForce(randVec, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (sys.RestTime > 0f)
        {
            if (isReallyEnemy)
            {
                rigidbodys.AddForce((Player.transform.position - transform.position).normalized * speed * Time.fixedDeltaTime * 10f, ForceMode.VelocityChange);
            }
            remaintime -= Time.fixedDeltaTime;
            currentinvistime += Time.fixedDeltaTime;
            if (Health <= 0)
            {
                GameObject SystemObj = GameObject.FindGameObjectWithTag("system");
                Systems system = SystemObj.GetComponent<Systems>();
                system.Currentheat += heatlev / Mathf.Pow(system.Multiplier, 1.2f);
                system.Score += BaseScore * system.Multiplier;
                foreach (GameObject dEffect in DeathEffect)
                {
                    if (dEffect != null)
                    {
                        Instantiate(dEffect, transform.position, Quaternion.identity);
                    }
                }
                Destroy(gameObject);
            }
            if (remaintime < 0f)
            {
                if (isReallyEnemy)
                {
                    foreach (GameObject dEffect in DeathEffect)
                    {
                        if (dEffect != null)
                        {
                            Instantiate(dEffect, transform.position, Quaternion.identity);
                        }
                    }
                }
                Destroy(gameObject);
            }
        }
        else {
            rigidbodys.Sleep();
        }
    }
}
