using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAbility_ChristianS : GenericPower
{
    // Start is called before the first frame update
    private bool isActive;
    private bool hasLanded = true;
    public GameObject dottedLine;
    private float coolDown;
    
    //private Vector3 topPos = new Vector3(3.53f, 0.72f, 0f);
    //private Vector3 topPos = new Vector3(3.53f, 0.9f, 0f);
    private Vector3 topPos = new Vector3(2.91f, 1.22f, 0f);
    private Vector3 startPos = new Vector3(2.91f, 0f, 0f);

    //private Vector3 bottomPos = new Vector3(3.53f, -0.56f, 0f);
    //private Vector3 bottomPos = new Vector3(3.53f, -0.56f, 0f);
    private Vector3 bottomPos = new Vector3(2.91f, -0.56f, 0f);
    //private Vector3 topRot = new Vector3(0f, 0f, 18.36f);
    private Vector3 topRot = new Vector3(0f, 0f, 31.22f);
    private Vector3 bottomRot = new Vector3(0f, 0f, -11.36f);
    private float loopTimer;
    private bool movingDown;
    private float gravity;
    private float timerMax;
    public override void Activate()
    {
        if (coolDown > 0 || isActive) return;
        dottedLine.transform.position = topPos;
        dottedLine.transform.localEulerAngles = topRot;
        dottedLine.SetActive(true);
        isActive = true;
        movingDown = true;
        loopTimer = 0f;
        coolDown = 6f;
        GetComponent<PlayerController>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = new Vector3();
    }

    void Start()
    {
        gravity = 1.25f;
        timerMax = .5f;
    }
    // Update is called once per frame
    void Update()
    {
        if (coolDown > 0) coolDown -= Time.deltaTime;
        if (isActive)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && hasLanded)
            {
                isActive = false;
                coolDown = 1f;
                dottedLine.SetActive(false);
                GetComponent<PlayerController>().enabled = true;
                return;
            }
            if (movingDown)
            {
                dottedLine.transform.localPosition = Vector3.Lerp(topPos, bottomPos, loopTimer / timerMax);
                dottedLine.transform.localEulerAngles = Vector3.Lerp(topRot, bottomRot, loopTimer/ timerMax);
                if (dottedLine.transform.localPosition.y < bottomPos.y)
                {
                    dottedLine.transform.localPosition = bottomPos;
                }
            }

            if (!movingDown)
            {
                dottedLine.transform.localPosition = Vector3.Lerp(bottomPos, topPos, loopTimer / timerMax);
                dottedLine.transform.localEulerAngles = Vector3.Lerp(bottomRot, topRot, loopTimer/ timerMax);
                if (dottedLine.transform.localPosition.y > topPos.y)
                {
                    dottedLine.transform.localPosition = topPos;
                }
            }
            loopTimer += Time.deltaTime;
            if (loopTimer > timerMax)
            {
                movingDown = !movingDown;
                loopTimer = 0f;
            }
            if (Input.GetKeyUp(KeyCode.X))
            {
                isActive = false;
                coolDown = 1f;
                dottedLine.SetActive(false);

                Vector2 direction = new Vector2();
                if (GetComponent<PlayerController>().FaceLeft) direction = new Vector2(-dottedLine.transform.right.x, -dottedLine.transform.right.y*1.2f);
                else direction = new Vector2(dottedLine.transform.right.x, dottedLine.transform.right.y*1.2f);
                GetComponent<Rigidbody2D>().velocity = new Vector3(direction.x * 11f, direction.y * 14f, 0f);
                GetComponent<Rigidbody2D>().gravityScale = gravity;
                isActive = false;
                hasLanded = false;
            }
            
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (!hasLanded)
        {
            if (other.gameObject.CompareTag("Platforms"))
            {
                hasLanded = true;
                GetComponent<PlayerController>().enabled = true;
                GetComponent<Rigidbody2D>().gravityScale = 0f;
            }
        }
    }
}
