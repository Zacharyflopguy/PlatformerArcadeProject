using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.UIElements.Experimental;
using UnityEngine.VFX;

public class BuilderController : Character
{
    public InputAction moveX;
    public InputAction build;
    public InputAction jump;
    
    [Header("Builder Settings")]
    public GameObject platform;
    public float buildCooldown = 0.5f;
    
    Rigidbody2D rigidbody2d;
    public Grid grid;
    public float acceleration;
    public float speed;
    public float drag;
    public float jumpForce;
    public float depth;
    private float buildCdTime = 0;
    private int facing = 1;
    private bool grounded = false;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        mainControl = master.GetComponent<MasterControl>();
        other = otherCharacter.GetComponent<Character>();
        spawn();
    }

    // Update is called once per frame
    void Update()
    {
        float move = moveX.ReadValue<float>();
        bool jumpInput = jump.ReadValue<float>() == 1;
        bool buildInput = build.ReadValue<float>() > 0;
        RaycastHit2D groundCheck = Physics2D.Raycast(transform.position, Vector2.down, depth, 264);

        //Set grounded to true if we hit the ground
        grounded = groundCheck.collider != null;

        rigidbody2d.AddForce(Vector2.right * (move * acceleration));
        if (grounded && jumpInput)
        {
          rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpForce);
          //rigidbody2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        if (move == 0)
        {
            //Lerp velocity to zero accounting for drag
            float newVelocityX = Mathf.Lerp(rigidbody2d.velocity.x, 0, drag * Time.deltaTime);
            rigidbody2d.velocity = new Vector2(newVelocityX, rigidbody2d.velocity.y);
        }
        else if (move > 0)
        {
            facing = 1;
        }
        else
        {
            facing = -1;
        }
        if (rigidbody2d.velocity.x > speed)
        {
            rigidbody2d.velocity = new Vector2(speed, rigidbody2d.velocity.y);
        }
        if (rigidbody2d.velocity.x < speed * -1)
        {
            rigidbody2d.velocity = new Vector2(speed * -1, rigidbody2d.velocity.y);
        }

        HandleBuild(buildInput, groundCheck);
    }

    
    private void HandleBuild(bool buildInput, RaycastHit2D groundCheck)
    {
        //Make sure build cooldown has passed and we are pressing the build button
        if (!(Time.time > buildCdTime)) return;
        if (!buildInput) return;
        if (!mainControl.useMaterial()) return;
        if (grounded)
        {
            RaycastHit2D checkPos =
                    Physics2D.Raycast(grid.CellToWorld(grid.WorldToCell(transform.position + new Vector3(0, -1.0f, 0))) +
                    new Vector3(grid.cellSize.x / 2, grid.cellSize.y / 2, 0), Vector2.up, 0.6f, 328);
            if (checkPos.collider == null)
            {
                Instantiate(platform,
                    grid.CellToWorld(grid.WorldToCell(transform.position + new Vector3(0, -1.0f, 0))) +
                    new Vector3(grid.cellSize.x / 2, grid.cellSize.y / 2, 0),
                    quaternion.identity);
            }
        }
        else
        {
            RaycastHit2D checkPos = Physics2D.Raycast(grid.CellToWorld(grid.WorldToCell(transform.position +
                    new Vector3(grid.cellSize.x * facing, -1.0f, 0))) +
                    new Vector3(grid.cellSize.x / 2, grid.cellSize.y / 2, 0), Vector2.up, 0.6f, 328);
            if (checkPos.collider == null)
            {
                Instantiate(platform,
                    grid.CellToWorld(grid.WorldToCell(transform.position +
                    new Vector3(grid.cellSize.x * facing, -1.0f, 0))) +
                    new Vector3(grid.cellSize.x / 2, grid.cellSize.y / 2, 0),
                    quaternion.identity);
            }
        }

        //Set new build cooldown time
        buildCdTime = Time.time + buildCooldown;
    }
    public override void spawn()
    {
        moveX.Enable();
        build.Enable();
        jump.Enable();
        transform.position = spawnPoint.transform.position;
    }
    public override void despawn()
    {
        moveX.Disable();
        build.Disable();
        jump.Disable();
        transform.position = despawnPoint.transform.position;
    }
}//Class End


