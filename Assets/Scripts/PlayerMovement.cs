using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private FixedJoystick joystick;
    private Vector2 movement;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        FindJoystick();
    }

    // INPUT ONLY
    void Update()
    {
        if (joystick == null)
            return;

        float moveX = -joystick.Horizontal;
        float moveY = -joystick.Vertical;

        movement = new Vector2(moveX, moveY);

        if (movement.magnitude > 1f)
            movement.Normalize();

        animator.SetFloat("MoveSpeed", movement.magnitude);

        if (moveX < -0.01f)
            spriteRenderer.flipX = false;
        else if (moveX > 0.01f)
            spriteRenderer.flipX = true;
    }

    // PHYSICS ONLY
    void FixedUpdate()
    {
        Vector2 targetPos = rb.position + movement * speed * Time.fixedDeltaTime;
        rb.MovePosition(targetPos);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindJoystick();
    }

    void FindJoystick()
    {
        joystick = FindObjectOfType<FixedJoystick>();
    }
}