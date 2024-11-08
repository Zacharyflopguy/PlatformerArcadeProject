using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlimeController : MonoBehaviour
{
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
    // Start is called before the first frame update
    void Start()
    {
        x.Enable();
        y.Enable();
        jump.Enable();
        slime = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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
                slime.AddForce(jumpDir * jumpForce * maxForce, ForceMode2D.Impulse);
                dirLock = false;
            }
            else
            {
                dirLock = true;
                jumpCooldown = 100;
            }
        }
        jumpCooldown--;
    }
}
