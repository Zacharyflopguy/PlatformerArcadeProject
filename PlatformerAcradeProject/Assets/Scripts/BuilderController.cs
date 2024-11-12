
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.UIElements.Experimental;
using UnityEngine.VFX;

public class BuilderController : Character
{
    private static readonly int Jumping = Animator.StringToHash("jumping");
    private static readonly int Walking = Animator.StringToHash("walking");
    private static readonly int Building = Animator.StringToHash("building");
    public InputAction moveX;
    public InputAction build;
    public InputAction jump;
    
    [Header("Builder Settings")]
    public GameObject platform;
    public float buildCooldown = 0.5f;

    [Header("Animation Settings")] 
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    
    Rigidbody2D rigidbody2d;
    public Grid grid;
    public float acceleration;
    public float speed;
    public float drag;
    public float jumpForce;
    [FormerlySerializedAs("depth")] public float raycastDepth;
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
        RaycastHit2D groundCheckFront = Physics2D.Raycast(transform.position + Vector3.right * 0.1f, Vector2.down, raycastDepth, 264);
        RaycastHit2D groundCheckBack = Physics2D.Raycast(transform.position + Vector3.right * -0.1f, Vector2.down, raycastDepth, 264);

        //Set grounded to true if we hit the ground
        grounded = groundCheckFront.collider != null || groundCheckBack.collider != null;
        
        //Set jumping animation to true if we are not grounded (don't walk in air)
        animator.SetBool(Jumping, !grounded);

        rigidbody2d.AddForce(Vector2.right * (move * acceleration));
        if (grounded && jumpInput)
        {
          rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpForce);
        }
        if (move == 0)
        {
            //Lerp velocity to zero accounting for drag
            float newVelocityX = Mathf.Lerp(rigidbody2d.velocity.x, 0, drag * Time.deltaTime);
            rigidbody2d.velocity = new Vector2(newVelocityX, rigidbody2d.velocity.y);
            
            animator.SetBool(Walking, false);
        }
        else if (move > 0)
        {
            facing = 1;
            spriteRenderer.flipX = false;
            animator.SetBool(Walking, true);
        }
        else
        {
            facing = -1;
            spriteRenderer.flipX = true;
            animator.SetBool(Walking, true);
        }
        if (rigidbody2d.velocity.x > speed)
        {
            rigidbody2d.velocity = new Vector2(speed, rigidbody2d.velocity.y);
        }
        if (rigidbody2d.velocity.x < speed * -1)
        {
            rigidbody2d.velocity = new Vector2(speed * -1, rigidbody2d.velocity.y);
        }

        HandleBuild(buildInput);
    }

    
    private void HandleBuild(bool buildInput)
    {
        //Make sure build cooldown has passed and we are pressing the build button
        if (!(Time.time > buildCdTime)) return;
        if (!buildInput) return;
        if (mainControl.materials <= 0) return;
        if (grounded)
        {
            RaycastHit2D checkPos = Physics2D.Raycast(grid.CellToWorld(grid.WorldToCell(transform.position +
                    new Vector3(grid.cellSize.x * facing, -1.0f, 0))) +
                    new Vector3(grid.cellSize.x / 2, grid.cellSize.y / 2, 0), Vector2.up, 0.6f, 840);
            if (checkPos.collider == null)
            {
                Instantiate(platform,
                    grid.CellToWorld(grid.WorldToCell(transform.position +
                    new Vector3(grid.cellSize.x * facing, -1.0f, 0))) +
                    new Vector3(grid.cellSize.x / 2, grid.cellSize.y / 2, 0),
                    quaternion.identity);
                mainControl.materials--;
                StartCoroutine(HandleBuildAnimation());
            }
            
        }
        else
        {
            RaycastHit2D checkPos =
                    Physics2D.Raycast(grid.CellToWorld(grid.WorldToCell(transform.position + new Vector3(0, -1.0f, 0))) +
                    new Vector3(grid.cellSize.x / 2, grid.cellSize.y / 2, 0), Vector2.up, 0.6f, 840);
            if (checkPos.collider == null)
            {
                Instantiate(platform,
                    grid.CellToWorld(grid.WorldToCell(transform.position + new Vector3(0, -1.0f, 0))) +
                    new Vector3(grid.cellSize.x / 2, grid.cellSize.y / 2, 0),
                    quaternion.identity);
                mainControl.materials--;
                StartCoroutine(HandleBuildAnimation());
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

    private IEnumerator HandleBuildAnimation()
    {
        animator.SetBool(Building, true);
        
        yield return new WaitForSeconds(0.3f);
        
        animator.SetBool(Building, false);
    }
    
}//Class End


