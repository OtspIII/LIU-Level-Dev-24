﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : ThingController
{
    public MonsterData Shooter;
    public float Speed = 10;
    public float Lifetime = 0;
    public float HP = 1;
    public BulletController JustHit = null;
    
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    public override void ApplyJSON(JSONData data)
    {
        base.ApplyJSON(data);
        if (data.Audio)GameManager.Me.PlaySound(data.Audio);
        if (data.Lifetime > 0) Lifetime = data.Lifetime;
        if (data.Amount > 0) HP = data.Amount;
    }

    void Update()
    {
        RB.velocity = transform.right * -Speed;
        if (Lifetime > 0)
        {
            Lifetime -= Time.deltaTime;
            if (Lifetime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Setup(CharController shooter)
    {
        Shooter = shooter.Data;
        Source = shooter;
        Speed = Shooter.AttackSpeed;
        if (shooter.Player)
            gameObject.layer = 10;
        if (!IsJSON)
        {
            Lifetime = Shooter.AttackRange / Mathf.Max(0.1f, Shooter.AttackSpeed);
        }
        GetComponent<BoxCollider2D>().enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        CharController c = other.gameObject.GetComponent<CharController>();
        
        if(c != null && !c.BulletImmune) HitChar(c);
        BulletController b = other.gameObject.GetComponent<BulletController>();
        if (b != null)
        {
            if (b != JustHit)
            {
                b.JustHit = this;
                float tempHP = HP - b.HP;
                b.HP -= HP;
                HP = tempHP;
                if(b.HP <= 0)
                    b.Despawn();
                if(HP <= 0)
                    Despawn();
            }
        }
        else
            Despawn();
    }

    public void HitChar(CharController c)
    {
        if (!c.Tile || Shooter.Color == MColors.Player)
        {
            c.Knockback(transform.position, Shooter.Knockback);
            c.TakeDamage(Shooter.Damage);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        CharController c = other.gameObject.GetComponent<CharController>();

        if (c != null && !c.BulletImmune)
        {
            HitChar(c);
            Despawn();
        }
    }

    public void Despawn()
    {
        if (JSON.Drop != ' ')
        {
            ThingController drop = GameManager.Me.SpawnThing(JSON.Drop,GameManager.Me.Creator,transform.position);
            if (drop != null) drop.Source = Source;
        }
        Destroy(gameObject);
    }
}
