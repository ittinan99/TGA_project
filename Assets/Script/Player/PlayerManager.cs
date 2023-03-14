using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;

    GameObject controller;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
        }
    }

    void CreateController()
    {
        Transform SpawnPoint = SpawnManager.instance.GetSpawnpoint();
        controller = PhotonNetwork.Instantiate(Path.Combine("Photonprefabs", "PlayerController"), SpawnPoint.position, SpawnPoint.rotation, 0,new object[] { PV.ViewID});
        Debug.Log("===== Instantiated player Controller =====");
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateController();
    }
}
