using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    private InputAction moveAction;
    private InputAction jumpAction;
    private Rigidbody rb;

    public float speed = 0f;
    public float jumpForce = 0f;

    private bool isGrounded = false;
    private bool jumpQueued = false;

    private bool gameStarted = false;

    public GameObject startButton;
    public GameObject powerUp;
    public GameObject powerUpText;
    public GameObject winText;

    private int count = 0;
    public int totalCollectibles = 6;
    private bool isPoweredUp = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStarted) return;

        if (isGrounded && jumpAction.triggered)
        {
            jumpQueued = true;
        }
        
    }

    private void FixedUpdate()
    {
        if (!gameStarted) return;
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(moveInput.x, 0.0f, moveInput.y);
        rb.AddForce(move * speed);

        if (jumpQueued)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpQueued = false;
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("BreakableWall") && isPoweredUp)
        {
            collision.gameObject.SetActive(false);
        }
    }

    public void StartGame()
    {
        gameStarted = true;
        rb.useGravity = true;
        startButton.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            count++;
            other.gameObject.SetActive(false);
            if (count >= totalCollectibles)
            {
                powerUp.SetActive(true);
            }
        }
        else if (other.gameObject.CompareTag("PowerUp"))
        {
            other.gameObject.SetActive(false);
            speed *= 2f;
            GetComponent<Renderer>().material.color = Color.red;
            isPoweredUp = true;
            powerUpText.SetActive(true);
        }
        else if (other.gameObject.CompareTag("Goal"))
        {
            winText.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
