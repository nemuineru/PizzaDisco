using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public Systems sys;
    public PlayerMovement plmove;
    public ItemType type;
    public float remainingtime = 60f;
    public GameObject[] DeathEffect;

    Rigidbody rigid;

    public enum ItemType {
        TimeAdd,
        EnemyFrenzy,
        MorePowerful,
        SpeedUp
    };

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && plmove.isRotating && sys.RestTime > 0f)
        {
            switch (type)
            {
                case ItemType.TimeAdd:
                    {
                        sys.plustime += 10;
                        break;
                    }
                case ItemType.EnemyFrenzy:
                    {
                        sys.CurrentMaxCharacterNum += 50;
                        break;
                    }
                case ItemType.MorePowerful:
                    {
                        plmove.PowerUpTime += 5f;
                        break;
                    }
                case ItemType.SpeedUp:
                    {
                        plmove.SpeedUpTime += 10f;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            foreach (GameObject dEffect in DeathEffect)
            {
                if (dEffect != null)
                {
                    Instantiate(dEffect, transform.position, Quaternion.identity);
                }
            }
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        sys = GameObject.FindGameObjectWithTag("system").GetComponent<Systems>();
        plmove = GameObject.FindGameObjectWithTag("Player").transform.root.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sys.RestTime > 0f)
        {
            remainingtime -= Time.deltaTime;
            if (remainingtime < 0f)
            {
                Destroy(gameObject);
            }
        }
        else {
            rigid.Sleep();
        }
    }
}
