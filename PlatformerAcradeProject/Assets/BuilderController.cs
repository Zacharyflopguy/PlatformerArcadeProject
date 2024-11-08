using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuilderController : MonoBehaviour
{
    public InputAction moveX;
    public InputAction build;
    public InputAction jump;
    public GameObject platform;
    Rigidbody2D rigidbody2d;
    public Grid grid;
    public float acceleration;
    public float speed;
    public float drag;
    public float jumpForce;
    public float depth;
    private float buildCooldown = 0;
    private int facing = 1;
    public bool active;
    // Start is called before the first frame update
    void Start()
    {
        moveX.Enable();
        build.Enable();
        jump.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();

        //REMOVE ONCE SLIME IMPLEMENTED
        active = false;
        GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            float move = moveX.ReadValue<float>();
            bool jumpInput = jump.ReadValue<float>() == 1;
            bool buildInput = build.ReadValue<float>() > 0;
            RaycastHit2D groundCheck = Physics2D.Raycast(transform.position, Vector2.down, depth, 8);
            rigidbody2d.AddForce(Vector2.right * move * acceleration);
            if (groundCheck.collider != null && jumpInput)
            {
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpForce);
                //rigidbody2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
            if (move == 0)
            {
                if (rigidbody2d.velocity.x > 0)
                {
                    rigidbody2d.AddForce(Vector2.left * drag * acceleration);
                }
                else if (rigidbody2d.velocity.x < 0)
                {
                    rigidbody2d.AddForce(Vector2.right * drag * acceleration);
                }
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
            if (buildCooldown == 0)
            {
                if (buildInput)
                {
                    if (groundCheck.collider == null)
                    {
                        RaycastHit2D checkPos =
                            Physics2D.Raycast(grid.CellToWorld(grid.WorldToCell(transform.position + new Vector3(0, -1.0f, 0))) +
                            new Vector3(grid.cellSize.x / 2, grid.cellSize.y / 2, 0), Vector2.up, 0.6f, 8);
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
                            new Vector3(grid.cellSize.x / 2, grid.cellSize.y / 2, 0), Vector2.up, 0.6f, 8);
                        if (checkPos.collider == null)
                        {
                            Instantiate(platform,
                                grid.CellToWorld(grid.WorldToCell(transform.position +
                                new Vector3(grid.cellSize.x * facing, -1.0f, 0))) +
                                new Vector3(grid.cellSize.x / 2, grid.cellSize.y / 2, 0),
                                quaternion.identity);
                        }
                    }
                    buildCooldown = 100;
                }
            }
            else if (buildCooldown > 0)
            {
                buildCooldown--;
            }
        }
    }
}
