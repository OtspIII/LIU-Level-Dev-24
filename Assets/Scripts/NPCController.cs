using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCController : ActorController
{
    public string Type;
    public NPCBehavior Behavior;
    public bool Aggro;
    public ActorController Attacking;
    public List<Vector3> PatrolPath;
    public int PatrolProgress = -1;
    private Transform Destination;
    public float DestTime = 0;
    public NPCSpawner Spawner; 
    
    public override void OnStart()
    {
        base.OnStart();
        gameObject.layer = 10;
        foreach (Collider c in gameObject.GetComponentsInChildren<Collider>())
            c.gameObject.layer = 10;
        if(God.LM.UseJSON) ImprintJSON(God.LM.GetActor(Type));
        Destination = new GameObject().transform;
        Destination.transform.position = transform.position;
    }

    public override void OnUpdate()
    {
        if (LevelManager.MidCutscene) return;
        base.OnUpdate();
        //Debug.Log("VIS: " + JSON.Vision);
        if (Aggro && !Attacking && God.Player != null 
            && Vector3.Distance(transform.position,God.Player.transform.position) < GetVision())
        {
            //Debug.Log("LOOK FOR PLAYER");
            if(Physics.Raycast(transform.position,(God.Player.transform.position - transform.position),out RaycastHit hit))
            {
                FirstPersonController p = hit.collider.GetComponentInParent<FirstPersonController>();
                if (p == God.Player) Attacking = p;
            }
        }

        if (Attacking != null)
        {
            LookAt(Attacking.transform);
            Vector3 dist = AimObj.transform.position - Attacking.transform.position;
            dist.y = 0;
            //Debug.Log("DIST: " + dist.magnitude);
            if(dist.magnitude > 3)
                HandleMove(transform.forward, false, 0, 0, true);
            else
                HandleMove(Vector3.zero, false, 0, 0, true);
            Shoot(AimObj.transform.position + AimObj.transform.forward, AimObj.transform.rotation);
        }
        else IdleAI();
    }

    public void LookAt(Transform pos)
    {
        AimObj.transform.LookAt(pos);
        transform.rotation = Quaternion.Euler(new Vector3(0,AimObj.transform.rotation.eulerAngles.y,0));
        AimObj.transform.LookAt(pos);
    }

    public void IdleAI()
    {
        if (Behavior == NPCBehavior.Idle) return;
        Vector3 dist = Destination.transform.position - transform.position;
        dist.y = 0;
        DestTime -= Time.deltaTime;
        if (dist.magnitude < 0.5f || DestTime <= 0)
        {
            DestTime = Behavior == NPCBehavior.Wander ? 5 : 60;
            if (Behavior == NPCBehavior.Patrol)
            {
                if (PatrolPath.Count == 0) return;
                PatrolProgress++;
                if (PatrolProgress >= PatrolPath.Count) PatrolProgress = 0;
                Destination.transform.position = PatrolPath[PatrolProgress];
            }

            if (Behavior == NPCBehavior.Wander)
            {
                Destination.transform.position += new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));
            }
        }

        Vector3 d = Destination.transform.position;
        d.y = transform.position.y;
        Destination.position = d;
        transform.LookAt(Destination);
        HandleMove(transform.forward, false, 0, 0, false);
    }

    public override void TakeDamage(int amt, ActorController source = null)
    {
        if (source != null) Attacking = source;
        base.TakeDamage(amt, source);
    }

    public float GetVision()
    {
        return JSON.Vision > 0 ? JSON.Vision : 999;
    }

    void OnDestroy()
    {
        God.Actors.Remove(this);
        if (Spawner != null) Spawner.Children.Remove(this);
    }
}

public enum NPCBehavior
{
    None=0,
    Idle=1,
    Patrol=2,
    Wander=3,
}