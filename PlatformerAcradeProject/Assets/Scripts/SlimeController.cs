using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlimeController : Character
{
    private static readonly int Squishing = Animator.StringToHash("Squishing");
    public InputAction x;
    public InputAction y;
    public InputAction jump;
    public GameObject cross;
    Rigidbody2D slime;
    Vector2 jumpDir;
    bool dirLock = false;
    float jumpForce = 1;
    public float maxForce;
    bool rising;
    public float powShiftSpeed;
    int jumpCooldown;
    public float depth;
    [Header("Animation Settings")] 
    public Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        despawn();
        mainControl = master.GetComponent<MasterControl>();
        other = otherCharacter.GetComponent<Character>();
        slime = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D groundCheck = Physics2D.Raycast(transform.position, Vector2.down, depth, 256);
        if (groundCheck.collider != null)
        {
            groundCheck.collider.gameObject.GetComponent<Platform>().slimed();
        }
        bool jumpInput = jump.ReadValue<float>() == 1;
        if (dirLock == false)
        {
            jumpDir = new Vector2(x.ReadValue<float>(), y.ReadValue<float>());
            jumpDir.Normalize();
            cross.transform.position = new Vector2(transform.position.x, transform.position.y) + jumpDir;
        }
        else
        {
            if (rising)
            {
                jumpForce += powShiftSpeed;
                if (jumpForce >= 1)
                {
                    rising = false;
                }
            }
            else
            {
                jumpForce -= powShiftSpeed;
                if(jumpForce <= 0.5f)
                {
                    rising = true;
                }
            }
            cross.transform.position = new Vector2(transform.position.x, transform.position.y) + jumpDir * jumpForce;
        }
        if (jumpInput && slime.velocity.y == 0 && jumpCooldown <= 0)
        {
            if (dirLock)
            {
                slime.AddForce(jumpDir * (jumpForce * maxForce), ForceMode2D.Impulse);
                dirLock = false;
                animator.SetBool("squishing", false);
            }
            else
            {
                dirLock = true;
                animator.SetBool("squishing", true);
                jumpCooldown = 100;
            }
        }
        jumpCooldown--;
    }
    public override void spawn()
    {
        x.Enable();
        y.Enable();
        jump.Enable();
        dirLock = false;
        transform.position = spawnPoint.transform.position;
    }
    public override void despawn()
    {
        x.Disable();
        y.Disable();
        jump.Disable();
        dirLock = false;
        transform.position = despawnPoint.transform.position;
    }
}
