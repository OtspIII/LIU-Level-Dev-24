using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAbility_ChristianS : GenericPower
{
    // Start is called before the first frame update
    private bool isActive;
    public GameObject dottedLine;
    private float coolDown;
    
    private Vector3 topPos = new Vector3(3.53f, 0.72f, 0f);
    private Vector3 startPos = new Vector3(3.53f, 0f, 0f);
    private Vector3 bottomPos = new Vector3(3.53f, -0.56f, 0f);
    private float topRot = 11.36f;
    private float bottomRot = -11.36f;
    private float loopTimer;
    private bool movingDown;
    public override void Activate()
    {
        if (coolDown > 0 || isActive) return;
        dottedLine.transform.position = topPos;
        dottedLine.transform.localEulerAngles = new Vector3(0f, 0f, topRot);
        dottedLine.SetActive(true);
        isActive = true;
        movingDown = true;
        loopTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (coolDown > 0) coolDown -= Time.deltaTime;
        if (isActive)
        {
            if (movingDown)
            {
                dottedLine.transform.position += Vector3.Lerp(topPos, bottomPos, loopTimer / 2f);
                if (dottedLine.transform.position.y < bottomPos.y)
                {
                    dottedLine.transform.position = bottomPos;
                }
            }

            if (!movingDown)
            {
                dottedLine.transform.position += Vector3.Lerp(bottomPos, topPos, loopTimer / 2f);
                if (dottedLine.transform.position.y > topPos.y)
                {
                    dottedLine.transform.position = topPos;
                }
            }
            loopTimer += Time.deltaTime;
            if (loopTimer > 2f)
            {
                movingDown = !movingDown;
                loopTimer = 0f;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isActive = false;
                coolDown = 5f;
                dottedLine.SetActive(false);
                Player.GetComponent<Rigidbody2D>().velocity = dottedLine.transform.right * 2f;
            }
            
        }
    }
}
