using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NadiaPlayerPower : GenericPower
{
    public float jumpForce = 10f;
    public float wallSlideSpeed = 2f;
    public float wallStickTime = 0.25f;
    public LayerMask wallLayer;

    private bool isOnWall = false;
    private bool isWallJumping = false;
    private float timeSinceWallStick = 0f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckWallCollision();

        if (isOnWall)
        {
            HandleWallStick();
        }
        else
        {
            timeSinceWallStick = 0f;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isOnWall)
            {
                WallJump();
            }
            else
            {
                NormalJump();
            }
        }
    }

    void FixedUpdate()
    {
        if (isOnWall && !isWallJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }
    }

    void CheckWallCollision()
    {
        isOnWall = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, 0.1f, wallLayer);
    }

    void HandleWallStick()
    {
        if (timeSinceWallStick < wallStickTime)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            timeSinceWallStick += Time.deltaTime;
        }
    }

    void NormalJump()
    {
        if (IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void WallJump()
    {
        isWallJumping = true;
        Invoke("ResetWallJump", 0.1f);

        float jumpDirection = isOnWall ? -transform.localScale.x : 1f;
        rb.velocity = new Vector2(jumpDirection * jumpForce, jumpForce);
    }

    void ResetWallJump()
    {
        isWallJumping = false;
    }

    bool IsGrounded()
    {
        // Implement your own grounded check logic here, for example, using raycasts or collider checks.
        return true;
    }
}