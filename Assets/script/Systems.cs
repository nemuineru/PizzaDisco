using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Systems : MonoBehaviour
{
    public float Countdowns = 3f;
    public CinemachineVirtualCamera maincams, resultcam;
    public fortweet resulttxts;

    public float Basefontsize;
    public float CurrentTime = 0f;
    public float RestTime;
    public float Currentheat;
    public float FirstMaxCharacterNum = 30;
    public float CurrentMaxCharacterNum = 30;
    public int plustime = 0;
    public int Score = 0;
    public int ItemAppearNum = 15;
    public int ItemKillNum = 0;
    public int KillNum = 0;
    public int PrevMultiplier = 1;
    public int Multiplier = 1;



    public TextMeshProUGUI TxtScore;
    public TextMeshProUGUI TxtTime;
    public TextMeshProUGUI Txtplustime;
    public TextMeshProUGUI TxtMultiplier;
    public TextMeshProUGUI TxtCountDown;
    public TextMeshProUGUI ResultUI;

    public GameObject mainUI,resultUI;
    public GameObject tweetbutton;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountdownStart());
    }

    // Update is called once per frame
    void Update()
    {
        if (Countdowns < 0f)
        {
            if (RestTime > 0f)
            {
                CurrentTime += Time.deltaTime;
                CurrentMaxCharacterNum -= Time.deltaTime;
                CurrentMaxCharacterNum = Mathf.Clamp(CurrentMaxCharacterNum, FirstMaxCharacterNum, Mathf.Infinity);
                TxtTime.text = (Mathf.Ceil(RestTime * 100) / 100).ToString("f1");
                TxtScore.text = Score.ToString();
                string heatGauge = "";
                int heatgaugeint = Mathf.CeilToInt(Mathf.Repeat(Currentheat, 1f) * 10f);
                for (int i = 0; i < 10; i++)
                {
                    if (i < heatgaugeint)
                    {
                        heatGauge += "=";
                    }
                    else
                    {
                        heatGauge += "-";
                    }
                }
                TxtMultiplier.text = "x" + Multiplier.ToString() + heatGauge;
                RestTime -= Time.deltaTime;

                Currentheat -= Time.deltaTime * 0.1f * Mathf.Ceil(Currentheat);
                Currentheat = Mathf.Clamp(Currentheat, 0f, Mathf.Infinity);
                Multiplier = Mathf.CeilToInt(Mathf.Pow(2f, Mathf.Ceil(Currentheat - 1)));
                TxtMultiplier.fontSize = Basefontsize + Multiplier * 4f;
                if (PrevMultiplier != Multiplier)
                {
                    if (PrevMultiplier < Multiplier)
                    {
                        Currentheat += 0.5f;
                        StartCoroutine(FontMoreBig());
                    }
                    PrevMultiplier = Multiplier;
                }
                if (plustime != 0)
                {
                    RestTime += plustime;
                    StartCoroutine(Fontplusremain(plustime));
                    plustime = 0;
                }
            }
            else
            {
                if (mainUI.activeInHierarchy)
                {
                    mainUI.SetActive(false);
                    resultUI.SetActive(true);
                    maincams.enabled = false;
                    resultcam.enabled = true;
                    resulttxts.text = Score + "点分の美味しいピザを作ったよ！";
                    ResultUI.text = "ピザの美味しさ : " + Score + "pts." + "\n"
                        + "切った具材 : " + KillNum + "個" + "\n"
                        + "かかった時間 : " + CurrentTime.ToString("f1") + "秒";
                    EventSystem.current.SetSelectedGameObject(tweetbutton);
                }
            }
        }
        else
        {
            Countdowns -= Time.deltaTime;
        }
    }

    IEnumerator Fontplusremain(int pls)
    {
        float Plustime = 0.5f;
        string addornot = "";
        if (pls > 0)
        {
            addornot = "+";
        }
        for (float times = 0f; times < Plustime; times += Time.deltaTime)
        {
            Txtplustime.text = addornot + pls.ToString() + ".0";
            yield return null;
        }
        Txtplustime.text = "";
        yield return null;
    }

    IEnumerator FontMoreBig()
    {
        float Bigtime = 0.2f;
        float AddFontsize = 6f;
        float fontsize = TxtMultiplier.fontSize;
        for (float times = 0f; times < Bigtime; times += Time.deltaTime)
        {
            TxtMultiplier.fontSize = fontsize + ((AddFontsize * AddFontsize) - Mathf.Pow(AddFontsize * (times - Bigtime / 2f) / (Bigtime / 2f), 2f));
            yield return null;
        }
        TxtMultiplier.fontSize = Basefontsize;
        yield return null;
    }

    IEnumerator CountdownStart()
    {
        while (Countdowns > 0f) {
            TxtCountDown.text = "GET READY!";
            yield return null;
                }
            TxtCountDown.text = "GO!";
        yield return new WaitForSecondsRealtime(0.5f);
        TxtCountDown.text = "";
        yield return null;
    }
}

