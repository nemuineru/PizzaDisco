using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Menusystem : MonoBehaviour
{
    public TextMeshProUGUI gui;
    public SubmitType type = SubmitType.Menumove;
    public string Loadscene;
    public GameObject ParentObj;
    public GameObject LoadMenuObj;
    public GameObject SubMenufirst;

    public enum SubmitType
    {
        SceneMove,
        Menumove
    }


    public void OnSelected()
    {
        SelectCols(Color.red);
    }

    public void OnDeselected()
    {
        SelectCols(Color.black);
    }
    
    public void OnSubmit()
    {
        SelectCols(Color.black);
        if (type == SubmitType.Menumove) {
            LoadMenuObj.SetActive(true);
            ParentObj.SetActive(false);
            EventSystem.current.SetSelectedGameObject(LoadMenuObj);
            if (SubMenufirst != null) {
                EventSystem.current.SetSelectedGameObject(SubMenufirst);
            }
        }
        if (type == SubmitType.SceneMove)
        {
            SceneManager.LoadScene(Loadscene);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SelectCols(Color col) {
        if (gui != null)
        {
            gui.color = col;
        }
    }
}
