using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class SpawnableController : MonoBehaviour
{
    [Header("Customizable")]
    public string Name;
    public Vector3 Destination;
    [Header("Ignore Below")]
    public ItemSpawnController Spawner;
    public TextMeshPro Desc;
    public JSONItem Data;
    public MeshRenderer MR;
    
    public bool IsSetup = false;

    public void Setup(ItemSpawnController s,JSONItem data)
    {
        Data = data;
        Spawner = s;
        Destination = Spawner.Destination;
        Name = Data.Text;
        SetColor();
        God.LM.Spawned.Add(gameObject);
    }

    void Update()
    {
        if (!IsSetup && Name != "" && God.LM?.Ruleset != null && God.LM?.Ruleset.Author != "")
        {
            
            Data = God.LM.GetItem(Name.ToString());
            //Debug.Log("SETUP! " + Name + " / " + Data.Text + " / " + God.LM?.Ruleset?.Author);
            Desc.text = Name.ToString();
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
        TakeEffects(pc);
        Spawner?.TakenFrom(pc);
        
            Destroy(gameObject);
    }

    public virtual string GetName()
    {
        if (Data != null) return Data.Text;
        return "TEST ITEM";
    }

    public virtual void TakeEffects(FirstPersonController pc)
    {
        float amt = Data.Amount > 0 ? Data.Amount : 1;
        switch (Data.Type)
        {
            case ItemTypes.Healing:
            {
                pc.TakeHeal((int)amt);
                break;
            }
            case ItemTypes.Points:
            {
                //##pc.GetPoint((int)amt);
                if (Destination != Vector3.zero)
                {
                    pc.transform.position = God.LM.transform.position + Destination;
                    //##pc.SetPosClientRPC(God.LM.transform.position + Destination);
                }
                break;
            }
            case ItemTypes.Jump:
            {
                Vector3 vel = pc.RB.velocity;
                if (Destination == Vector3.zero)
                {
//                    vel.y = amt;
//                    pc.RB.velocity = vel;
                    pc.TakeKnockback(new Vector3(0,amt,0));
                }
                else
                    pc.TakeKnockback(Destination);
                
                
                break;
            }
            case ItemTypes.Teleport:
            {
                pc.transform.position = God.LM.transform.position + Destination;
                //##pc.SetPosClientRPC(God.LM.transform.position + Destination);
                break;
            }
        }
//        Debug.Log("TOOK IT: " + gameObject.name);
        
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
        God.LM?.Spawned.Remove(gameObject);
    }
}
