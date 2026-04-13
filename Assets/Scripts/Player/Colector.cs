using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))] // Asegura que el GameObject tenga un CircleCollider2D
public class Colector : MonoBehaviour
{
    public Player player;
    public CircleCollider2D coll;

    private Vector2 moveInput;
    private bool facingRight = true;
    private SpriteRenderer spriteRenderer;

    private Vector3 originalLocalPosition; // Para recordar dónde va normalmente
    private Vector3 frozenWorldPosition;   // Para recordar dónde congelarlo

    private void Awake()
    {
        inputs = new();
        coll = GetComponent<CircleCollider2D>();
        coll.isTrigger = true; // Asegurarnos de que sea trigger

        // Inicializamos el SpriteRenderer
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // Intentar obtener la referencia al Player
        if (player == null)
        {
            player = GetComponentInParent<Player>();
        }
    }

    private void Start()
    {
        // Guardamos su distancia (offset) original respecto al Player
        originalLocalPosition = transform.localPosition;
    }

    private void Update()
    {
        // Si el Player está congelado (LeftShift), el Colector se mueve independientemente en el mundo
        // Si el Player no está congelado, el Colector es hijo del Player y se mueve con él
        bool playerIsFrozen = Input.GetKey(KeyCode.LeftShift) && player != null;

        // Aplicar movimiento del Colector cuando el Player está congelado
        if (playerIsFrozen && moveInput != Vector2.zero)
        {
            MovementMechanism(moveInput);       
        }

        const float flipThreshold = 0.01f;
        if (Mathf.Abs(moveInput.x) > flipThreshold && spriteRenderer != null)
        {
            if (moveInput.x < 0f && facingRight)
            {
                spriteRenderer.flipX = true;
                facingRight = false;
            }
            else if (moveInput.x > 0f && !facingRight)
            {
                spriteRenderer.flipX = false;
                facingRight = true;
            }
        }

        // 1. En el instante exacto en que presionas Controll, guardamos su posición actual en el mundo
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            frozenWorldPosition = transform.position;
        }

        // 2. Mientras mantengas Controll presionado, lo forzamos a quedarse en esa coordenada      
        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.position = frozenWorldPosition;
        }
    }

    private void OnEnable()
    {
        inputs.Enable();
        inputs.Player.Move.performed += OnMovement;
        inputs.Player.Move.canceled += OnMovement;
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void MovementMechanism(Vector2 input)
    {
        transform.position += (Vector3)input * player.Stats.Speed * Time.deltaTime;
    }


    private void OnDrawGizmosSelected()
    {       
        if (coll != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, coll.radius);
        }
        else
        {
            // Intenta leer el radio temporalmente si aún no estamos en Play
            var tempColl = GetComponent<CircleCollider2D>();
            if (tempColl != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, tempColl.radius);
            }
        }
    }
    public InputSystem_Actions inputs;
}