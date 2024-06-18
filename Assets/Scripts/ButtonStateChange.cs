
using UnityEngine;
using Mirror;


public class ButtonStateChange : NetworkBehaviour
{
    [SyncVar (hook =nameof(OnClickIntotoButton))][SerializeField] private bool button_pressed = false;
    [SyncVar(hook = nameof(OnClickDigButton))][SerializeField] private bool dig_pressed = false;
    [SyncVar(hook = nameof(OnClickExcButton))][SerializeField] private bool exc_pressed = false;
    [SyncVar(hook = nameof(OnClickCirButton))][SerializeField] private bool cir_pressed = false;
    [SyncVar(hook = nameof(OnClickResButton))][SerializeField] private bool res_pressed = false;
    [SyncVar(hook = nameof(OnClickRepButton))][SerializeField] private bool rep_pressed = false;
    [SyncVar(hook = nameof(OnClickRepMaButton))][SerializeField] private bool repMa_pressed = false;
    [SyncVar(hook = nameof(OnClickNerButton))][SerializeField] private bool ner_pressed = false;
    [SyncVar(hook = nameof(OnClickGlaButton))][SerializeField] private bool gla_pressed = false;
    [SyncVar(hook = nameof(OnClickPoiButton))][SerializeField] private bool poi_pressed = false;


    void OnClickIntotoButton(bool oldBool, bool newBool) 
    {
        stateChange.intotoGO.SetActive(button_pressed);
        stateChange.intGO.SetActive(button_pressed);
        stateChange.texturedGO.SetActive(!button_pressed);
    }
    void OnClickDigButton(bool oldBool, bool newBool) => stateChange.digGO.SetActive(dig_pressed);
    void OnClickExcButton(bool oldBool, bool newBool) => stateChange.excGO.SetActive(exc_pressed);
    void OnClickCirButton(bool oldBool, bool newBool) => stateChange.cirGO.SetActive(cir_pressed);
    void OnClickResButton(bool oldBool, bool newBool) => stateChange.resGO.SetActive(res_pressed);
    void OnClickRepButton(bool oldBool, bool newBool) => stateChange.repGO.SetActive(rep_pressed);
    void OnClickRepMaButton(bool oldBool, bool newBool) 
    {
        if (stateChange.repMaGO != null) stateChange.repMaGO.SetActive(repMa_pressed);
    } 
    void OnClickNerButton(bool oldBool, bool newBool) => stateChange.nerGO.SetActive(ner_pressed);
    void OnClickGlaButton(bool oldBool, bool newBool) { if (stateChange.glaGO != null) stateChange.glaGO.SetActive(gla_pressed); }
    void OnClickPoiButton(bool oldBool, bool newBool) { if (stateChange.poiGO != null) stateChange.poiGO.SetActive(poi_pressed); }


    public GameObject[] system_buttons;
    public GameObject group_buttons_one;
    public GameObject group_buttons_two;
    public RectTransform canvasSize;
    [SerializeField] private GameObject spawnedAnimal;
    public StateChange stateChange;

    public void OnIsolateDig() 
    {
        dig_pressed = !dig_pressed;
        stateChange.digGO.SetActive(dig_pressed);
    }
    public void OnIsolateCir()
    {
        cir_pressed = !cir_pressed;
        stateChange.cirGO.SetActive(cir_pressed);
    }
    public void OnIsolateExc()
    {
        exc_pressed = !exc_pressed;
        stateChange.excGO.SetActive(exc_pressed);
    }
    public void OnIsolateRes()
    {
        res_pressed = !res_pressed;
        stateChange.resGO.SetActive(res_pressed);
    }
    public void OnIsolateRep()
    {
        rep_pressed = !rep_pressed;
        stateChange.repGO.SetActive(rep_pressed);
    }
    public void OnIsolateRepMa()
    {
        repMa_pressed = !repMa_pressed;
        if (stateChange.repMaGO != null) stateChange.repMaGO.SetActive(repMa_pressed);
    }
    public void OnIsolateNer()
    {
        ner_pressed = !ner_pressed;
        stateChange.nerGO.SetActive(ner_pressed);
    }
    public void OnIsolateGla()
    {
        gla_pressed = !gla_pressed;
        if (stateChange.glaGO != null) stateChange.glaGO.SetActive(gla_pressed);
    }
    public void OnIsolatePoi()
    {
        poi_pressed = !poi_pressed;
        if (stateChange.poiGO != null) stateChange.poiGO.SetActive(poi_pressed);
    }


    public void ChangeState()
    {
        spawnedAnimal = GameObject.FindGameObjectWithTag("Respawn");

        if (spawnedAnimal != null) stateChange = spawnedAnimal.GetComponent<StateChange>();

        if (button_pressed == false)
        {
            button_pressed = true;
            canvasSize.sizeDelta = new Vector2(1000, 450);
            group_buttons_one.SetActive(true);
            group_buttons_two.SetActive(true);
            stateChange.intotoGO.SetActive(button_pressed);
            stateChange.texturedGO.SetActive(!button_pressed);
            for (int i = 0; i < system_buttons.Length; i++)
            {

                system_buttons[i].SetActive(true);
            }

        }
        else
        {
            button_pressed = false;
            canvasSize.sizeDelta = new Vector2(1000, 170);
            group_buttons_one.SetActive(false);
            group_buttons_two.SetActive(false);
            stateChange.intotoGO.SetActive(button_pressed);
            stateChange.texturedGO.SetActive(!button_pressed);
            for (int i = 0; i < system_buttons.Length; i++)
            {

                system_buttons[i].SetActive(false);
            }
        }
    }

}
