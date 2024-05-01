using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZoneController : TriggerableController
{
    public float RateOfDamage;
    public int Damage;
    private float Timer;
    public AudioSource AS;
    public List<TriggerableController> Inside;
    public TriggerableController Owner;
    
    void Start()
    {
        Timer = 0;
    }

    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            Timer = RateOfDamage;
            foreach (TriggerableController tc in Inside.ToArray())
            {
                if (tc == Owner) continue;
                if (tc == null)
                {
                    Inside.Remove(tc);
                    continue;
                }
                tc.TakeDamage(Damage);
                ParticleGnome pg = Instantiate(God.Library.Blood, tc.transform.position, Quaternion.identity);
                pg.Setup(Damage);
            }
        }
        if(Inside.Count > 0 && !AS.isPlaying && AS.clip != null) AS.Play();
        else if (AS.isPlaying && Inside.Count == 0) AS.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        TriggerableController tc = other.gameObject.GetComponent<TriggerableController>();
        if (tc != null && !Inside.Contains(tc))
        {
            Inside.Add(tc);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        TriggerableController tc = other.gameObject.GetComponent<TriggerableController>();
        if (tc != null)
        {
            Inside.Remove(tc);
        }
    }
}
