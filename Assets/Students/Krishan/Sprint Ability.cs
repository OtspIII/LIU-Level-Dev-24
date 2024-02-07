using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintAbility : GenericPower
{
    public float addSpeed;
    public float upTime;
    public float upTimeVal = 1;
    public bool cdstart;
    public float cooldown;
    public float cooldownVal = 3;

    // Start is called before the first frame update
    void Start()
    {
        upTime = upTimeVal;
        cooldown = cooldownVal;
    }

    // Update is called once per frame
    

    public void Update()
    {

        addSpeed = Player.Speed;
        
        if (Input.GetKey(KeyCode.X) && upTime > 0)
        {
            upTime -= Time.deltaTime;
            addSpeed += (Time.deltaTime * 20);
            cdstart = false;
        }
        
        if (Input.GetKeyUp(KeyCode.X) && !cdstart)
        {
            cdstart = true;
        }

        if (upTime <= 0 && !cdstart)
        {
            cdstart = true;
        }
        
        if (cooldown <= 0)
        {
            cdstart = false;
            cooldown = cooldownVal;
            upTime = upTimeVal;

        }
        
        if (cdstart)
        {
            cooldown -= Time.deltaTime;
            Player.Body.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            Player.Body.color = new Color(1, 1, 1, 1f);
        }

        if (cdstart && Player.OnGround() && addSpeed > 10)
        {
            addSpeed -= (Time.deltaTime * 50);
        }
        
        else if (cdstart && addSpeed > 10)
        {
            addSpeed -= (Time.deltaTime * 2);
        }

        if (addSpeed < 10)
        {
            addSpeed = 10f;
        }

        Player.Speed = addSpeed;

    }
    
}
