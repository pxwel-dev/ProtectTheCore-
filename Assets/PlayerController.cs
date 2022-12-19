using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public GameOverScript GameOverScreen;
    public int playerHealth = 100;
    public int playerArmour = 100;
    public static int playerScore = 0;

    public TextMeshProUGUI healthUI;
    public TextMeshProUGUI armourUI;
    public TextMeshProUGUI scoreUI;
    
    public Rigidbody2D rb;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public Camera cam;
    public Animator animator;
    
    private float _moveSpeed = 2.5f;
    private Vector2 _movementInput;
    private Vector2 _mousePos;
    private List<RaycastHit2D> _castCollisions = new List<RaycastHit2D>();
    private Collision2D lastCollision;
    private float death_timer = 1f;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        rb.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("isDead", false);
        animator.SetBool("hasExploded", false);
        timer = 0f;
        playerScore = 0;
    }

    void Update()
    {
        healthUI.text = playerHealth.ToString();
        armourUI.text = playerArmour.ToString();
        scoreUI.text = playerScore.ToString();
        
        _movementInput.x = Input.GetAxisRaw("Horizontal");
        _movementInput.y = Input.GetAxisRaw("Vertical");

        _mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        int coreHealth = CoreScript.coreHealth;

        if (playerHealth <= 0)
        {
            if (timer == death_timer)
            {
                GameOver();
            }
            else
            {
                if (lastCollision.collider.tag == "Bullet")
                {
                    animator.SetBool("isDead", true);
                    timer += Time.fixedDeltaTime;
                }
                else if (lastCollision.collider.tag == "RPG")
                {
                    animator.SetBool("hasExploded", true);
                    timer += Time.fixedDeltaTime;
                }
            }
        }
        else if (coreHealth <= 0)
        {
            GameOver();
        }
    }

    void FixedUpdate()
    {
        Vector2 lookDirection = _mousePos - rb.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;

        if (Input.GetKey(KeyCode.E))
        {
            TryPickUpItem();
        }
   
        if (_movementInput != Vector2.zero)
        {
            bool success = TryMove(_movementInput);

            if (!success)
            {
                success = TryMove(new Vector2(_movementInput.x, 0));

                if (!success)
                {
                    success = TryMove(new Vector2(0, _movementInput.y));
                }
            }
            
            animator.SetBool("isMoving", success);
            
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _moveSpeed = 5f;
                animator.SetBool("isRunning", success);
            }
            else
            {
                _moveSpeed = 2.5f;
                animator.SetBool("isRunning", false);
            }

        }
        else
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("isRunning", false);
        }
    }

    private bool TryMove(Vector2 direction)
    {
        int count = rb.Cast(
            direction, 
            movementFilter, 
            _castCollisions, 
            _moveSpeed * Time.fixedDeltaTime + collisionOffset);
        
        if (count == 0)
        {
            rb.MovePosition(rb.position + direction * _moveSpeed * Time.fixedDeltaTime);
            return true;
        }
        return false;
    }

    private void TryPickUpItem()
    {
        Vector2 lookDirection = _mousePos - rb.position;
        RaycastHit2D hit = Physics2D.Raycast(rb.position, lookDirection, 
            1, 
            LayerMask.GetMask("Powerups"));
        String tag = hit.collider.tag;
        if (tag == "Health")
        {
            playerHealth += 50;
            if (playerHealth > 100)
            {
                playerHealth = 100;
            }
        }
        else if (tag == "Armour")
        {
            playerArmour += 50;
        }
        else if (tag == "SMGAmmo")
        {
            PlayerShooting.smgAmmo += Random.Range(25, 100);
        }
        else if (tag == "PlasmaAmmo")
        {
            PlayerShooting.rpgAmmo += Random.Range(1, 5);
        }
        Destroy(hit.collider.gameObject);
        
    }
    
    private void TakeDamage(int damage)
    {
        if (playerArmour >= 50)
        {
            playerArmour -= damage;
        }
        else if (playerArmour < 50 & playerArmour > 0)
        {
            playerArmour -= damage / 2;
            playerHealth -= damage / 2;
        }
        else
        {
            playerHealth -= damage;
        }
        if (playerArmour < 0)
        {
            playerArmour = 0;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        lastCollision = col;
        String tag = col.collider.tag;
        if (tag == "Bullet")
        {
            TakeDamage(5);
        }
        else if (tag == "RPG")
        {
            TakeDamage(20);
        }
    }

    public void GameOver()
    {
        GameOverScreen.Setup(playerScore);
        Destroy(gameObject);
    }
    
}
