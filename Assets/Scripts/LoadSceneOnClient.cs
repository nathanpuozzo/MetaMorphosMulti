using Mirror;
using Mirror.Examples.AdditiveLevels;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClient : NetworkBehaviour
{
    [Scene, Tooltip("Which scene to send player from here")]
    public string destinationScene;

    [Tooltip("Where to spawn player in Destination Scene")]
    public Vector3 startPosition;

    public GameObject _Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNewScene() 
    {
        if(isServer) StartCoroutine(SendPlayerToNewScene(_Player));
    }

    [ServerCallback]
    IEnumerator SendPlayerToNewScene(GameObject player)
    {
        if (player.TryGetComponent(out NetworkIdentity identity))
        {
            NetworkConnectionToClient conn = identity.connectionToClient;
            if (conn == null) yield break;

            // Tell client to unload previous subscene. No custom handling for this.
            conn.Send(new SceneMessage { sceneName = gameObject.scene.path, sceneOperation = SceneOperation.UnloadAdditive, customHandling = true });

            yield return new WaitForSeconds(AdditiveLevelsNetworkManager.singleton.fadeInOut.GetDuration());

            NetworkServer.RemovePlayerForConnection(conn, false);

            // reposition player on server and client
            player.transform.position = startPosition;
            player.transform.LookAt(Vector3.up);

            // Move player to new subscene.
            SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByPath(destinationScene));

            // Tell client to load the new subscene with custom handling (see NetworkManager::OnClientChangeScene).
            conn.Send(new SceneMessage { sceneName = destinationScene, sceneOperation = SceneOperation.LoadAdditive, customHandling = true });

            NetworkServer.AddPlayerForConnection(conn, player);

            // host client would have been disabled by OnTriggerEnter above
            if (NetworkClient.localPlayer != null && NetworkClient.localPlayer.TryGetComponent(out PlayerController playerController))
                playerController.enabled = true;
        }
    }
}
