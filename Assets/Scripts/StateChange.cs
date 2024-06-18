using Mirror;
using UnityEngine;


public class StateChange : NetworkBehaviour
{

    //variables for gameobjects animal, intoto & textured
    [Tooltip("variables for gameobjects animal, intoto & textured")]
    [Header("Components")]
    public GameObject animalGO;
    public GameObject intotoGO;
    public GameObject texturedGO;
    public GameObject digGO;
    public GameObject excGO;
    public GameObject nerGO;
    public GameObject cirGO;
    public GameObject resGO;
    public GameObject repGO;
    public GameObject repMaGO;
    public GameObject intGO;
    public GameObject poiGO;
    public GameObject glaGO;

    [SerializeField]
    [SyncVar(hook = nameof(OnEnableHighlight))]
    private bool hovered = false;

    void OnEnableHighlight(bool previous, bool now){}

    public void OnChangeHighlight(bool highlightBool)
    {
        highlightBool = hovered;
    }
}

