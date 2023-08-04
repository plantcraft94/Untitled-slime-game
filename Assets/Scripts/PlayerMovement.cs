using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashTime = 0.5f;
    private Vector2 _dashDir;
    bool isDashing;
    bool canDash = true;


    [Header("Checker")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheckRadius;


    [Header("Gravity")]
    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private float fallMultiplier = 2.5f;

    [Header("Assist")]
    [SerializeField] private float jumpBufferLength = 0.2f;
    [SerializeField] private float jumpBufferTimer;
    bool jumpBuffer;


    [SerializeField] private float cayoteTimeLength = 0.2f;
    

    [Header("Variables")]
    bool isGrounded;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool jumpInput;
    private float x;
    private Animator anim;
    TrailRenderer tr;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        tr = GetComponent<TrailRenderer>();
    }
    private void Update()
    {

        FlipSprite();
        Animate_movement();
        Animate_Jump();
        x = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(x * speed, rb.velocity.y);
        
        isGrounded = Physics2D.OverlapCapsule(groundCheck.position, groundCheckRadius, CapsuleDirection2D.Horizontal, 0, groundLayer);
        if (isGrounded == true)
        {
            canDash = true;
            cayoteTimeLength = 0.2f;
        }
        else
        {
            cayoteTimeLength -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            jumpBuffer = true;
            jumpBufferTimer = jumpBufferLength;
            jumpInput = true;
            anim.SetBool("Jumping",true);
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
        }
        if (Input.GetKeyDown(KeyCode.C) && canDash)
        {
            isDashing = true;
            canDash = false;
            tr.emitting = true;
            anim.SetBool("Dashing", true);
            _dashDir = new Vector2(x, Input.GetAxisRaw("Vertical"));
            if (_dashDir == Vector2.zero)
            {
                _dashDir = new Vector2(sr.flipX ? -1 : 1, 0);
            }
            StartCoroutine(stopDash());
        }

        if (isDashing)
        {
            rb.gravityScale = 0;
            rb.velocity = _dashDir.normalized * dashSpeed;
            return;
        }

        if (rb.velocity.y > 0f)
        {
            cayoteTimeLength = 0f;
        }
        else if (rb.velocity.y < 0f)
        {
            rb.gravityScale = gravityScale * fallMultiplier;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -15f));
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
    }

    private void FixedUpdate()
    {
        // Process jump in the FixedUpdate method
        if (jumpBuffer == true)
        {
            jumpBufferTimer -= Time.deltaTime;
            if (jumpBufferTimer > 0 && (cayoteTimeLength > 0 || (isGrounded && jumpInput)))
            {              
                jumpBuffer = false;
                rb.gravityScale = gravityScale;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);

                // Reset the jump buffer and cayote time states
                jumpInput = false;
                
            }
            else if (jumpBufferTimer <= 0)
            {
                jumpBuffer = false;
            }
        }

        if (jumpBuffer == false)
        {
            jumpBufferTimer = 0;
        }
    }
    private void FlipSprite()
    {
        if(x > 0)
        {
            sr.flipX = false;
        }
        else if (x < 0)
        {
            sr.flipX = true;
        }
    }
    private void Animate_movement()
    {
        if (rb.velocity.x != 0)
        {
            anim.SetBool("Moving", true);
        }
        else
        {
            anim.SetBool("Moving", false);

        }
    }
    private void Animate_Jump()
    {
        if (rb.velocity.y > 0)
        {
            anim.SetBool("Jumping", true);
            anim.SetBool("Up", true);
        }
        if (rb.velocity.y == 0 && !isGrounded)
        {
            anim.SetBool("Up", false);
        }
        else if (rb.velocity.y < 0)
        {
            anim.SetBool("Jumping", false);
            anim.SetBool("Up", false);
        }
        if (isGrounded)
        {
            anim.SetBool("isGrounded", true);
            anim.SetBool("Up", true);
            anim.SetBool("Jumping", false);
        }
        else if (!isGrounded)
        {
            anim.SetBool("isGrounded", false);
        }
        
    }
    IEnumerator stopDash()
    {
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        anim.SetBool("Dashing", false);
        rb.gravityScale = gravityScale * fallMultiplier;
        tr.emitting = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
