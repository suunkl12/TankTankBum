using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public static MainMenuUI Singleton { get; private set; }
    [SerializeField] private TMP_InputField nameInputField;
    public Server server;
    public Client client;

    private Animator mainMenuAnimator;

    public Action<string> OnHostOrJoinRoom;
    public Action OnLobbyLeft;

    private void Awake()
    {
        if (Singleton != null)
            return;

        Singleton = this;
    }

    private void Start()
    {
        this.mainMenuAnimator = GetComponent<Animator>();

    }

    public void OnOnlineBtn()
    {
        this.mainMenuAnimator.SetTrigger("ToHostJoinMenu");
    }

    public void OnSettingBtn()
    {

    }

    /*
    Switch in to Lobby Menu
    Take the name from ipf + assign it to the player
    */
    public void OnHostBtn()
    {
        server.Init(8007, 10); //This need to change (Stop hard-coded)
        string playerName = this.nameInputField.text != "" ? this.nameInputField.text : "I forgot to name myself";

        client.Init("127.0.0.1", 8007, playerName);

        this.OnHostOrJoinRoom?.Invoke(playerName);
        this.mainMenuAnimator.SetTrigger("ToLobbyMenu");
    }

    public void OnJoinBtn()
    {
        this.mainMenuAnimator.SetTrigger("ToConnectMenu");
    }

    public void OnConnectBtn()
    {
        client.Init("127.0.0.1", 8007, this.nameInputField.text);
        this.OnHostOrJoinRoom?.Invoke(this.nameInputField.text);

        this.mainMenuAnimator.SetTrigger("ToLobbyMenu");
    }

    public void OnStartBtn()
    {

    }

    public void OnReadyBtn()
    {

    }

    public void OnLeaveBtn()
    {
        if (PlayerInformation.Singleton.MyPlayerInformation.Id == 0)
        {
            Debug.Log("Server");
            server.Shutdown();
        }
        else
        {
            Debug.Log("Client");
            client.Shutdown();
        }

        this.OnLobbyLeft?.Invoke();
        this.mainMenuAnimator.SetTrigger("ToOnlineSettingMenu");
    }
}
