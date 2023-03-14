using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

public class PhotonMainMenu : MonoBehaviourPunCallbacks
{
    public static PhotonMainMenu instant;

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNametext;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject roomListItemprefab;
    [SerializeField] GameObject playerListItemprefab;
    [SerializeField] GameObject startbutton;

    private void Awake()
    {
        instant = this;
    }

    private void Start()
    {
        Debug.Log("connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MainmenuController.instance.OpenMenu("title");
        Debug.Log("Joined Lobbe");
        PhotonNetwork.NickName = "Player "+ Random.Range(0,1000).ToString();
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MainmenuController.instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom()
    {
        MainmenuController.instance.OpenMenu("room");
        roomNametext.text = PhotonNetwork.CurrentRoom.Name;

        Player[] player = PhotonNetwork.PlayerList;

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < player.Count(); i++)
        {
            Instantiate(playerListItemprefab, playerListContent).GetComponent<PlayerListItem>().SetUp(player[i]);
        }

        startbutton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startbutton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creating Faild : " + message;
        MainmenuController.instance.OpenMenu("error");
    }

    public void Leaveroom()
    {
        PhotonNetwork.LeaveRoom();
        MainmenuController.instance.OpenMenu("loading");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MainmenuController.instance.OpenMenu("loading");

    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(2);
    }

    public override void OnLeftRoom()
    {
        MainmenuController.instance.OpenMenu("title");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for(int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListItemprefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemprefab , playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    
}
