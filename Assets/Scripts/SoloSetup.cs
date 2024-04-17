using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoloSetup : MonoBehaviour
{
    public TextMeshProUGUI HPText;
    public TextMeshProUGUI StatusText;
    public TextMeshProUGUI AnnounceText;
    public TextMeshProUGUI UpdateText;

    private void Awake()
    {
        God.HPText = HPText;
        God.StatusText = StatusText;
        God.AnnounceText = AnnounceText;
        God.UpdateText = UpdateText;
    }
}
