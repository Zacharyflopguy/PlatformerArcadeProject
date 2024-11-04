using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuilderController : MonoBehaviour
{
    public InputAction moveX;
    public InputAction build;
    public InputAction jump;
    Rigidbody2D rigidbody2d;
    public float acceleration;
    public float speed;
    public float drag;
    public float jumpForce;
    public float depth;
    // Start is called before the first frame update
    void Start()
    {
        moveX.Enable();
        build.Enable();
        jump.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float move = moveX.ReadValue<float>();
        bool jumpInput = jump.ReadValue<float>() == 1;
        RaycastHit2D groundCheck = Physics2D.Raycast(transform.position, Vector2.down, depth, 8);
        rigidbody2d.AddForce(Vector2.right*move*acceleration);
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
            }else if(rigidbody2d.velocity.x < 0)
            {
                rigidbody2d.AddForce(Vector2.right * drag * acceleration);
            }
        }
        if(rigidbody2d.velocity.x > speed)
        {
            rigidbody2d.velocity = new Vector2(speed, rigidbody2d.velocity.y);
        }
        if (rigidbody2d.velocity.x < speed * -1)
        {
            rigidbody2d.velocity = new Vector2(speed * -1, rigidbody2d.velocity.y);
        }
    }
}
