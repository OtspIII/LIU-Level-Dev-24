﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MonsterData
{
    public string Creator;
    public MColors Color;
    public MTypes Type;
    public int HP;
    public float Speed;
    public int Damage;
    public float TurnSpeed;
    public float Size;
    public float AttackRate;
    public float AttackRange;
    public float AttackSpeed;
    public float AttackSpread;
    public float Cost;
    public int MinLevel;
    public float VisionRange;
    public float Knockback;
    public int Ammo;
    public float ReloadTime;

    public MonsterData(string[] data)
    {
        int n = 0;
        try
        {
            Creator = data[n]; n++;
            Color = (MColors)Enum.Parse(typeof(MColors), data[n]);n++;
            Type = (MTypes)Enum.Parse(typeof(MTypes), data[n]);n++;
            HP = int.Parse(data[n]);n++;
            Speed = float.Parse(data[n]) * GameSettings.BaseSpeed;n++;
            Damage = int.Parse(data[n]);n++;
            TurnSpeed = GameSettings.BaseTurn * float.Parse(data[n]);n++;
            Size = float.Parse(data[n]);n++;
            AttackRate = float.Parse(data[n]);n++;
            AttackRange = float.Parse(data[n]);n++;
            AttackSpeed = float.Parse(data[n]);n++;
            AttackSpread = float.Parse(data[n]);n++;
            Cost = float.Parse(data[n]);n++;
            MinLevel = int.Parse(data[n]);n++;
            VisionRange = data.Length > n ? float.Parse(data[n]) : 9;n++;
            Knockback = data.Length > n ? float.Parse(data[n]) : 0;n++;
            Ammo = data.Length > n ? int.Parse(data[n]) : 0;n++;
            ReloadTime = data.Length > n ? float.Parse(data[n]) : 1;n++;
        }
        catch
        {
            Debug.Log("INVALID: " + String.Join(',',data) + " / " + data[n]);
            Creator = data[n]; n++;
            Color = (MColors)Enum.Parse(typeof(MColors), data[n]);n++;
            Type = (MTypes)Enum.Parse(typeof(MTypes), data[n]);n++;
            HP = int.Parse(data[n]);n++;
            Speed = float.Parse(data[n]) * GameSettings.BaseSpeed;n++;
            Damage = int.Parse(data[n]);n++;
            TurnSpeed = GameSettings.BaseTurn * float.Parse(data[n]);n++;
            Size = float.Parse(data[n]);n++;
            AttackRate = float.Parse(data[n]);n++;
            AttackRange = float.Parse(data[n]);n++;
            AttackSpeed = float.Parse(data[n]);n++;
            AttackSpread = float.Parse(data[n]);n++;
            Cost = float.Parse(data[n]);n++;
            MinLevel = int.Parse(data[n]);n++;
            VisionRange = data.Length > n ? float.Parse(data[n]) : 9;n++;
            Knockback = data.Length > n ? float.Parse(data[n]) : 0;n++;
            Ammo = data.Length > n ? int.Parse(data[n]) : 0;n++;
            ReloadTime = data.Length > n ? float.Parse(data[n]) : 1;n++;
        }
    }

    public override string ToString()
    {
        return "Monster Creator: " + Creator + " / Color: " + Color + " / Type: " + Type +
               " / HP: " + HP + " / Speed: " + Speed + " / Damage: " + Damage;
    }
}



public enum MTypes
{
    None=0,
    Player=1,
    Charger=2,
    Shooter=3,
    Leaper = 4,
    Bouncer=5,
    Spinner=6,
}