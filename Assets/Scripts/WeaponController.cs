using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Customizable")]
    public string Name;// = new NetworkVariable<FixedString64Bytes>();
    [Header("Ignore Below")]
    public WeaponSpawnController Spawner;
    public TextMeshPro Desc;
    public JSONWeapon Data;
    public MeshRenderer MR;
    public bool IsSetup = false;
    

    public void Setup(WeaponSpawnController s,JSONWeapon data)
    {
        Data = data;
        Spawner = s;
        //NO.Spawn();
        Name = Data.Text;
        SetColor();
        God.LM.Spawned.Add(gameObject);
    }
    
    void Update()
    {
        if (!IsSetup && Name != "" && God.LM?.Ruleset != null && God.LM?.Ruleset.Author != "")
        {
            
            Data = God.LM.GetWeapon(Name.ToString());
            SetColor();
        }
    }
    
    public void SetColor()
    {   
        IsSetup = true;
        Desc.text = GetName();
        if (Data.Color != IColors.None)
        {
            MR.material = God.Library.GetColor(Data.Color);
        }
    }
    public void GetTaken(FirstPersonController pc)
    {
        pc.SetWeapon(Data);
        Spawner.TakenFrom(pc);
        Destroy(gameObject);
    }

    public virtual string GetName()
    {
        if (Data != null) return Data.Text;
        return "TEST ITEM";
    }


    void OnTriggerEnter(Collider other)
    {
        FirstPersonController pc = other.gameObject.GetComponent<FirstPersonController>();
//        Debug.Log("OCE: " + pc + " / " + other.gameObject);
        if(pc != null)
            GetTaken(pc);
    }
    
    public void OnDestroy()
    {
        //base.OnDestroy();
        God.LM?.Spawned.Remove(gameObject);
    }
}
