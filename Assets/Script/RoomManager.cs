using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;

    [SerializeField] RoomManager[] roomManagers= null;
    private void Awake()
    {
        roomManagers = FindObjectsOfType<RoomManager>();

        if (roomManagers.Length > 1)
        {
            for(int i =0;i < roomManagers.Length - 1; i++)
            {
                Destroy(roomManagers[i].gameObject);
            }
        }

        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    public override void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 2)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }
    }
}