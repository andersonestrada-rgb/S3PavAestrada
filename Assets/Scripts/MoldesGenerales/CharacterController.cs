using System;
using UnityEngine;
using UnityEngine.InputSystem;



public class CharacterController : BaseEntity, IDamageable
{
    public InputSystem_Actions inputs;
    public Rigidbody2D rigibody;

    public bool IsGrounded;
    public float MoveInput;
    public float Speed;
    public float JumpForce;

    private const int Maxjumps = 2;
    private int jumpcurrent;
    private bool canJump = true;

    private void Awake()
    {
        inputs = new();
    }
    private void OnEnable()
    {
        inputs.Enable();
        inputs.Player1.Move.performed += OnMovementStart;
        inputs.Player1.Move.canceled += OnMovementFinish;

        inputs.Player1.jump.performed += OnJumpStart;
        /*inputs.Player.Dash.performed += OnDashStart;
        inputs.Player.StepBack.performed += OnStepBack;*/
    }
    void Start()
    {

    }
    private void Update()
    {
        if (MoveInput != 0)
        {
            Vector2 dir = new Vector2(MoveInput, rigibody.linearVelocity.y);
            //  rigibody.linearVelocity = dir * Speed;

            rigibody.linearVelocity = new Vector2(MoveInput * Speed, rigibody.linearVelocity.y);
        }

    }
    private void OnMovementStart(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>().x;
    }
    private void OnMovementFinish(InputAction.CallbackContext context)
    {
        MoveInput = 0;
    }
    private void OnJumpStart(InputAction.CallbackContext context)
    {
        rigibody.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
    }

    public void TakeDamage()
    {

    }

   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (jumpcurrent >= Maxjumps)
        {
            canJump = false;
        }

        if (jumpcurrent < Maxjumps && canJump)
        {
            if (collision.gameObject.tag == "Ground")
            {
                rigibody.linearDamping = 30;
                IsGrounded = true;
            }
            jumpcurrent++;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        jumpcurrent = 1;
        if (jumpcurrent >= Maxjumps)
        {
            canJump = false;
        }

        if (jumpcurrent < Maxjumps && canJump)
        {
            if (collision.gameObject.tag == "Ground")
            {
                rigibody.linearDamping = 1;
                IsGrounded = false;
                jumpcurrent++;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<ICollectable>() != null)
        {
            collision.gameObject.GetComponent<ICollectable>().Collect();
        }

    }
}