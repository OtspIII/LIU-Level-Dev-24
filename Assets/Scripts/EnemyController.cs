using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : CharController
{
    public CharController Target;
    public float Rotation;
    public bool Active = false;
    public float Windup = 0;
    public float Leaping = 0;
    public Vector2 LeapStart;
    public NPCTeam Team = NPCTeam.Enemy;
    
    
    public override void OnStart()
    {
        if(Team ==NPCTeam.Enemy)
            GameManager.Me.Enemies.Add(this);
        GameManager.Me.AllEnemies.Add(this);
    }

    private void OnDestroy()
    {
        GameManager.Me.AllEnemies.Remove(this);
        GameManager.Me.Enemies.Remove(this);
    }


    public override void OnUpdate()
    {
        base.OnUpdate();
        Vector2 vel = Knock;
        if (!Active && GameManager.LevelMode)
        {
            PlayerController pc = GameManager.Me.PC;
//            Debug.Log("DIST: " + Vector2.Distance(transform.position,pc.transform.position) + " / " + Data.VisionRange);
            if(PlayerController.Moved && Vector2.Distance(transform.position,pc.transform.position) < Data.VisionRange)
                Activate();
        }
        if (Active)
        {
            if (Team == NPCTeam.Friendly)
            {
                if (Target != null && !Target.Alive) Target = null;
                if (Target == null)
                {
                    EnemyController best = null;
                    float dist = 9999;
                    foreach (EnemyController ec in GameManager.Me.Enemies)
                    {
                        if (ec.Team != NPCTeam.Enemy) continue;
                        float d = Vector3.Distance(transform.position, ec.transform.position);
                        if (d < dist)
                        {
                            best = ec;
                            dist = d;
                        }
                    }

                    if (best != null) Target = best;
                    else Team = NPCTeam.Asleep;
                }
            }

            if (Target == null) return;
            float speed = Data.Speed;
            if (Leaping <= 0)
            {
                float desired = Mathf.Atan2(transform.position.y - Target.transform.position.y,
                                    transform.position.x - Target.transform.position.x) * Mathf.Rad2Deg;
                Rotation = Mathf.LerpAngle(Rotation, desired, 0.05f);
                transform.rotation = Quaternion.Euler(0, 0, Rotation);
            }
            else
            {
                Leaping -= Time.deltaTime / 5f;
                if (Vector2.Distance(LeapStart, transform.position) >= Leaping)
                    Leaping = 0;
                else
                    speed = Data.AttackSpeed;
            }
            if (Windup > 0)
            {
//                RB.velocity = vel;
                Windup -= Time.deltaTime;
                if (Windup > 0)
                    Shaking = 1;
                else
                {
                    Shaking = 0;
                    SR.transform.localPosition = Vector3.zero;
                    if (Data.Type == MTypes.Leaper)
                    {
                        Vector3 rot = transform.rotation.eulerAngles;
                        if (Data.AttackSpread > 0) rot.z += Random.Range(0, Data.AttackSpread) - (Data.AttackSpread / 2);
                        transform.rotation = Quaternion.Euler(rot);
                        Leaping = Data.AttackRange * 2f;
                        LeapStart = transform.position;
                    }
                }
            }
            else
            {
                float dist = Vector2.Distance(Target.transform.position, transform.position);
                if (Data.Type == MTypes.Shooter && dist < Data.AttackRange)
                {
                    if (dist < Data.AttackRange * 0.75f)
                        speed *= -1;
                    else
                        speed = 0;
                }
                    
                vel += (Vector2)transform.right * -speed;
            }
        }

        if (Target != null && Vector2.Distance(transform.position, Target.transform.position) < Data.AttackRange)
        {
            switch (Data.Type)
            {
                case MTypes.Shooter: Shoot();
                    break;
                case MTypes.Leaper:
                    if (Windup > 0 || Leaping > 0) break;
                    Windup = Data.AttackRate;
                    break;
            }
        }

        RB.velocity = vel;

    }

    public override void ApplyJSON(JSONData data)
    {
        base.ApplyJSON(data);
        if (data.Special == "Friendly")
        {
            Team = NPCTeam.Friendly;
            Player = true;
            gameObject.layer = 8;
            
        }
    }

    public void Activate()
    {
        
        Active = true;
        if (Team == NPCTeam.Enemy)
        {
            Target = GameManager.Me.PC;
            Rotation = Mathf.Atan2(transform.position.y - Target.transform.position.y,
                transform.position.x - Target.transform.position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, Rotation);
        }

        BulletCooldown = Random.Range(0, Data.AttackRate);
    }

    public override void Die()
    {
        base.Die();
        GameManager.Me.Enemies.Remove(this);
    }

    public override void Reset()
    {
        Destroy(gameObject);
    }

    public override void TakeDamage(int amt)
    {
        base.TakeDamage(amt);
        if(!Active) Activate();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall")) Leaping = 0;
        if (Data.Type == MTypes.Shooter) return;
        PlayerController pc = other.gameObject.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.Knockback(transform.position,Data.Knockback);
            pc.TakeDamage(Data.Damage);
        }
    }

}

public enum NPCTeam
{
    None,
    Enemy,
    Friendly,
    Asleep
}