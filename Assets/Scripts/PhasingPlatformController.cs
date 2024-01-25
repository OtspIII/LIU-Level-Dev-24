using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhasingPlatformController : MonoBehaviour
{
    public float Timer;
    public float PhaseInTime = 3;
    public float PhaseOutTime = 1;
    public bool PhasedIn = true;

    public Collider2D Collider;
    public SpriteRenderer Body;
	Color StartColor;
	Color FadeColor;

    public float WarningTime = 0.5f;

	void Start(){
		StartColor = Body.color;
		FadeColor = Body.color;
		FadeColor.a = 0.1f;
	}
    
    void Update()
    {
        Timer -= Time.deltaTime;
        if (PhasedIn && Timer <= WarningTime)
        {
            Body.transform.localPosition = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
        }
        if (Timer <= 0)
        {
            PhasedIn = !PhasedIn;
            Body.transform.localPosition = Vector3.zero;
            if (PhasedIn)
            {
                Collider.enabled = true;
                Body.color = StartColor;
                Timer = PhaseInTime;
            }
            else
            {
                Collider.enabled = false;
                Body.color = FadeColor;
                Timer = PhaseOutTime;
            }
        }
    }
}
