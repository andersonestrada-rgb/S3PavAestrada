using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimatorController : MonoBehaviour
{
    public InputSystem_Actions inputs;

    public Animator animator;
    public Rigidbody2D rigidbody;
    public Vector2 MoveInput;
    public float speed;

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
    }



    void Start()
    {

    }
    void Update()
    {
        if (MoveInput != 0)
        {
            Vector2 dir = new Vector2(MoveInput, 0);
            rigidbody.linearVelocity = dir * speed;
             transform.Translate(dir * Time.deltaTime * 5);
        }

    }

    private void OnMovementStart(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>().x;       

    }






    private void OnMovementStart3(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();

        if (MoveInput.x != 0)
            animator.SetBool("IsMoving", true);

        // (MoveInput.x == -1) GetComponent<SpriteRenderer>().flipX = true;

        GetComponent<SpriteRenderer>().flipX = (MoveInput.x == -1 && MoveInput.x != 0) ? true : false;

    }
    private void OnMovementFinish(InputAction.CallbackContext context)
    {
        MoveInput = Vector2.zero;
        animator.SetBool("IsMoving", false);
    }
    private void OnJumpStart(InputAction.CallbackContext context)
    {
        animator.SetTrigger("OnJump");
    }

}