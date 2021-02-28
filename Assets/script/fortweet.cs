using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class fortweet : MonoBehaviour
{
    public string text;
    string hashtags = "unity1week,PizzaOnRoll";
    string linkurl = "https://unityroom.com/games/pizzaonroll";

    public void Tweeting()
    {
        var url = "https://twitter.com/intent/tweet?"
            + "text=" + text
            + "&url=" + linkurl
            + "&hashtags=" + hashtags;

#if UNITY_EDITOR
        Application.OpenURL(url);
#elif UNITY_WEBGL
            // WebGLの場合は、ゲームプレイ画面と同じウィンドウでツイート画面が開かないよう、処理を変える
            Application.ExternalEval(string.Format("window.open('{0}','_blank')", url));
#else
            Application.OpenURL(url);
#endif
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
