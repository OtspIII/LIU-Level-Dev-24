﻿using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharController : ThingController
{
    public MonsterData Data;
    public int HP;
    public int MaxHP;
    public bool Player;
    public float BulletCooldown = 999;
    public bool Tile = true;
    public bool BulletImmune = false;
    public float Buff = 1;
    public int Ammo = 0;
    public float Reload = 1;

    public override void OnAwake()
    {
        base.OnAwake();
        RB = GetComponent<Rigidbody2D>();
        Coll = GetComponent<Collider2D>();
        Mobile = true;
    }


    void Start()
    {
        OnStart();
    }
    
    public virtual void OnStart()
    {
        
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        BulletCooldown += Time.deltaTime;
        if (Data.Ammo > 0)
        {
            if (Ammo <= 0)
            {
                Reload -= Time.deltaTime;
                if (Reload <= 0)
                {
                    Ammo = Data.Ammo;
                }
            }
        }
    }


    public void Setup(MonsterData data,bool applyColor=false)
    {
        Data = data;
        gameObject.name = data.Color + "(" + data.Creator + ")";
        if(applyColor) SetColor(data.Color);
        if(data.Color == MColors.Player)GameSettings.CurrentPlayerSpeed = data.Speed;
        float skinny = data.Type == MTypes.Leaper ? 0.25f : 0.5f;
        transform.localScale = new Vector3(data.Size*0.5f,data.Size*skinny,1);
        if (HP == 0)
        {
            HP = data.HP;
            MaxHP = HP;
        }
        //if (data.Color == MColors.Player) Debug.Log(data.Color);
        Ammo = Data.Ammo;
    }
    
    public virtual void TakeDamage(int amt)
    {
        HP -= amt;
        if (HP <= 0)
            Die();
        else
            Shaking = 0.2f;
    }

    public virtual void Knockback(Vector2 src, float amt)
    {
        Vector2 dir = (Vector2)transform.position - src;
        Knock = dir * amt * GameSettings.Knockback;
    }

    public virtual void Die()
    {
        SR.color = Color.gray;
        Alive = false;
        if(RB != null) RB.velocity = Vector2.zero;
        Coll.enabled = false;
        if (JSON.Drop != ' ')
        {
            ThingController thing = GameManager.Me.SpawnThing(JSON.Drop,GameManager.Me.Creator,transform.position);
            if (thing != null)
            {
                thing.transform.position += new Vector3(0, 0, -0.1f);
                thing.Source = this;
            }
        }
        
        if(JSON.Toggle != "")
            GameManager.Me.Toggle(JSON.Toggle);

        Vector3 pos = transform.position;
        pos.z = 10;
        transform.position = pos;
        if (JSON.Audio)GameManager.Me.PlaySound(JSON.Audio);
    }
    
    public virtual void Reset()
    {
        Buff = 1;
    }

    public virtual void Shoot()
    {
        if (Data.Ammo > 0 && Ammo <= 0) return;
        if (BulletCooldown < Data.AttackRate / Buff) return;
        if (Data.Ammo > 0)
        {
            Ammo--;
            if (Ammo <= 0)
            {
                Reload = Data.ReloadTime;
            }
        }

        //Debug.Log("PEW: " + Time.time);
        Vector3 rot = transform.rotation.eulerAngles;
        Vector3 pos = transform.position + (transform.right * transform.localScale.x * -0.5f);
        if (Data.AttackSpread > 0) rot.z += Random.Range(0, Data.AttackSpread) - (Data.AttackSpread / 2);
        BulletController b = Instantiate(GameManager.Me.BPrefab, pos, Quaternion.Euler(rot));
        b.Setup(this);
        JSONData js = GameManager.Me.GetBullet(JSON.Bullet);
        if(js != null)
            b.ApplyJSON(js);
        BulletCooldown = 0;
    }

    public override void ApplyJSON(JSONData data)
    {
        base.ApplyJSON(data);
        if (data.Special == "BulletImmune")
            BulletImmune = true;
    }
}
