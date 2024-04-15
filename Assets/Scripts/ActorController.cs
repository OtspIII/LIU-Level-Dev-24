using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class ActorController : TriggerableController
{
    [Header("Ignore Below")]
    public Rigidbody RB;
    public MeshRenderer MR;
    public Collider Coll;

    public List<GameObject> Floors;
    public float ShotCooldown;
    public bool JustKnocked = false;
    public MeleeBox MB;
    
    public JSONActor JSON;
    public JSONWeapon CurrentWeapon;
    public JSONWeapon DefaultWeapon;
    
    public ParticleSystem Muzzle;
    
    public Vector3 Fling;
    public int HP;
    public int Ammo;

    public bool InControl = true;
    public bool CanWalk = true;
    public bool Invincible = false;
    public GameObject AimObj;
        
    [Header("Customizable")]
    public float JumpPower = 7;
    public float MoveSpeed = 10;
    public float SprintSpeed = 1.5f;

    void Awake()
    {
        MB = GetComponentInChildren<MeleeBox>();
    }
    
    void Start()
    {
      OnStart();  
    }

    public virtual void OnStart()
    {
        God.Actors.Add(this);
    }

    void Update()
    {
        OnUpdate();
    }

    public virtual void OnUpdate()
    {
        JustKnocked = false;
        if (transform.position.y < -100)
            Die();
        ShotCooldown -= Time.deltaTime;

    }

    public void ImprintJSON(JSONActor j)
    {
        //Debug.Log("IMPRINT JSON: " + name + " / " + j.Name + " / " + j.Weapon);
        JSON = j;
        MoveSpeed = j.MoveSpeed;
        SprintSpeed = j.SprintSpeed;
        HP = j.HP;
        DefaultWeapon = God.LM.GetWeapon(j.Weapon);
        //Debug.Log("JSON IMP: " + j.Weapon + " / " + DefaultWeapon.Text);
    }
    
    public void SetWeapon(JSONWeapon wpn)
    {
        CurrentWeapon = wpn;
        Ammo = wpn.Ammo;
    }


    public JSONWeapon GetWeapon()
    {
        // return God.LM.GetWeapon(Weapon.Value.ToString());
        if (CurrentWeapon != null && CurrentWeapon.RateOfFire > 0) return CurrentWeapon;
        if (DefaultWeapon != null && DefaultWeapon.RateOfFire > 0) return DefaultWeapon;
        return null;
    }
    
    public virtual void Die(ActorController source=null)
    {
        Destroy(gameObject);
        
        // if(God.LM.Respawn(this))
        //     Reset();
        // else
    }
    
    public virtual void HandleMove(Vector3 move,bool jump, float xRot,float yRot,bool sprint)
    {
        transform.Rotate(0,xRot,0);
        Vector3 eRot = AimObj.transform.localRotation.eulerAngles;
        eRot.x += yRot;
        if (eRot.x < -180) eRot.x += 360;
        if (eRot.x > 180) eRot.x -= 360;
        eRot = new Vector3(Mathf.Clamp(eRot.x, -90, 90),0,0);
        AimObj.transform.localRotation = Quaternion.Euler(eRot);
        if (!InControl || !CanWalk) return;
        bool onGround = OnGround();
        move = move.normalized * (sprint ? GetSprintSpeed() : GetMoveSpeed());
        if (jump && onGround)
            move.y = JumpPower;
        else
            move.y = RB.velocity.y;
        if (Fling.x != 0)
            move.x += Fling.x;
        if (Fling.z != 0)
            move.z += Fling.z;
        if (Fling != Vector3.zero && move.y == 0) move.y = 3;
        RB.velocity = move;
        
    }
    
    
    public void Shoot(Vector3 pos,Quaternion rot)
    {
        if (!InControl || ShotCooldown > 0) return;
        JSONWeapon wpn = GetWeapon();
        //Debug.Log("B: " + wpn?.Text + " / " + wpn?.RateOfFire);
        if (wpn == null || wpn.RateOfFire <= 0) return;
        
        ShotCooldown = wpn.RateOfFire;

        if (Ammo > 0)
        {
            Ammo--;
            if (Ammo <= 0)
                SetWeapon(DefaultWeapon);
        }

        //Debug.Log("SHOOT: " + wpn.Text + " / " + wpn.Type);
        if (wpn.Type == WeaponTypes.Melee)
        {
            MB.Swing(wpn);
        }
        else if (wpn.Type == WeaponTypes.Hitscan)
        {
            for (int n = 0; n < Mathf.Max(1, wpn.Shots); n++)
            {
                GameObject muzz = Muzzle != null ? Muzzle.gameObject : AimObj;
                Vector3 ro = AimObj.transform.rotation.eulerAngles;
                ro.y += Random.Range(-wpn.Accuracy, wpn.Accuracy);
                ro.x += Random.Range(-wpn.Accuracy, wpn.Accuracy);
                muzz.transform.rotation = Quaternion.Euler(ro);
                if(Muzzle != null) Muzzle.Emit(1);
                if (Physics.Raycast(AimObj.transform.position, muzz.transform.forward, out RaycastHit hit))
                {
                    ActorController pc = hit.collider.gameObject.GetComponentInParent<ActorController>();
                    if (pc != null && pc != this)
                    {
                        pc.TakeDamage(wpn.Damage, this);
                        if (wpn.Knockback > 0 && wpn.ExplodeRadius <= 0)
                            pc.TakeKnockback(transform.forward * wpn.Knockback);
                    }

                    if (wpn.ExplodeRadius > 0)
                    {
                        ExplosionController exp = Instantiate(God.Library.Explosion, hit.point,
                            Quaternion.Euler(0, 0, 0));
                        exp.Setup(this, wpn);
                    }

                    ParticleGnome partic = pc != null ? God.Library.Blood : God.Library.Dust;
                    ParticleGnome pg = Instantiate(partic, hit.point, Quaternion.identity);
                    pg.Setup(wpn.Damage);
                }
            }
        }
        else
        {

            for (int n = 0; n < Mathf.Max(1, wpn.Shots); n++)
            {
                Vector3 r = rot.eulerAngles;
                r.y += Random.Range(-wpn.Accuracy, wpn.Accuracy);
                r.x += Random.Range(-wpn.Accuracy, wpn.Accuracy);
                ProjectileController p = Instantiate(God.Library.Projectile, pos, Quaternion.Euler(r));
                p.Setup(this, wpn);
            }
        }
    }
    

    public bool OnGround()
    {
        return Floors.Count > 0 && Physics.Raycast(transform.position,transform.up * -1,1.5f);
    }
    
    public virtual void TakeDamage(int amt, ActorController source = null)
    {
        if (Invincible || amt <= 0) return;
        HP -= amt;
        if (HP <= 0)
        {
            Die(source);
        }
    }
    
    
    public void TakeHeal(int amt)
    {
        HP += amt;
        if (HP > GetMaxHP())
        {
            HP = GetMaxHP();
        }
    }
    
    public void TakeKnockback(Vector3 kb)
    {
        RB.velocity = kb;
        Fling = new Vector3(kb.x,0,kb.z);
//        Debug.Log("KB: " + kb);
        JustKnocked = true;
        RB.velocity = kb;
        JustKnocked = true;
    }
    
    public virtual int GetMaxHP()
    {
        return JSON != null ? JSON.HP : 100;
    }
    
    public float GetMoveSpeed()
    {
        return MoveSpeed;
        //return God.LM != null && God.LM.Ruleset != null && God.LM.Ruleset.MoveSpeed > 0 ? God.LM.Ruleset.MoveSpeed : 10;
    }
    
    public float GetSprintSpeed()
    {
        float move = GetMoveSpeed();
        if (SprintSpeed > 0) move *= SprintSpeed;
        //Debug.Log("SPRINT SPEED: " + move + " / " + name);
        return move;
        //return God.LM != null && God.LM.Ruleset != null && God.LM.Ruleset.SprintSpeed > 0 ? God.LM.Ruleset.SprintSpeed * move : move * 1.5f;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!Floors.Contains(other.gameObject))
            Floors.Add(other.gameObject);
        if (Fling != Vector3.zero && !JustKnocked)
        {
//            Debug.Log("ENDFLING");
            Fling = Vector3.zero;
            
        }
    }

    private void OnCollisionExit(Collision other)
    {
        Floors.Remove(other.gameObject);
    }

    private void OnDestroy()
    {
        God.Actors.Remove(this);
    }

    public override void Trigger(string type = "", GameObject target = null)
    {
        base.Trigger(type, target);
        switch (type)
        {
            case "Die":case "Destroy":
            {
                Die(target != null ? target.GetComponent<ActorController>() : null);
                break;
            }
        }
    }
}
