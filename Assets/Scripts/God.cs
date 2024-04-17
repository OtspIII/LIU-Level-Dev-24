using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public static class God
{
    public static GameManager GM;
    public static NetworkManager NM;
    public static LevelManager LM;
    public static LevelSpawner LS;
    public static LibraryController Library;
    public static TextMeshProUGUI HPText;
    public static TextMeshProUGUI StatusText;
    public static TextMeshProUGUI AnnounceText;
    public static TextMeshProUGUI UpdateText;
    public static Camera Camera;
    public static List<ActorController> Actors = new List<ActorController>();
    public static FirstPersonController Player;
    public static List<FirstPersonController> Players = new List<FirstPersonController>();
    public static Dictionary<string,FirstPersonController> PlayerDict = new Dictionary<string, FirstPersonController>();
    public static string NamePick;
    public static LevelManager TestLevel;
    public static RoundManager RM;
    
    public static NetStatus GetStatus()
    {
        if (NetworkManager.Singleton == null) return NetStatus.None;
        return NetworkManager.Singleton.IsHost ? NetStatus.Host : NetworkManager.Singleton.IsServer ? NetStatus.Server : NetStatus.Client;
    }

    public static FirstPersonController GetPlayer(string name)
    {
        if (!PlayerDict.ContainsKey(name)) return null;
        return PlayerDict[name];
    }
}


public enum NetStatus
{
    None,
    Host,
    Server,
    Client
}
