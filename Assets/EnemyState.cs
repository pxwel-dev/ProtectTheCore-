using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Pathfinding;
using UnityEngine;
using Color = UnityEngine.Color;
using Random = UnityEngine.Random;

public class EnemyState : MonoBehaviour
{
    public float enemyHealth;
    public Animator animator;
    public float bulletForce;
    public Transform firePoint;

    public GameObject SmgAmmo;
    public GameObject RPGAmmo;
    public GameObject Health;
    public GameObject Armour;

    public GameObject smg;
    public GameObject rpg;
    
    private GameObject _enemy;
    private string _currentCollider;
    private bool _isDead;
    private GameObject enemyWeapon;
    private float _rpgReload = 5f;
    private float _smgFirerate = 0.2f;
    private float timer;

    // Update is called once per frame

    void Start()
    {
        _enemy = EnemySpawner.newEnemy;
        animator.SetBool("isMoving", false);
        animator.SetBool("hasExploded", false);
        animator.SetBool("isDead", false);
        EnemyWeaponChoice();
    }

    void FixedUpdate()
    {
        StartCoroutine(checkMovement());
        if (_isDead)
        {
            DeathAnimation();
        }

        if (canShoot())
        {
            if (enemyWeapon == smg)
            { 
                if (timer >= _smgFirerate)
                {
                    ShootSmg();
                    timer = 0f;
                }
                else
                {
                    timer += Time.fixedDeltaTime;
                }
            }
            else if (enemyWeapon == rpg)
            {
                if (timer >= _rpgReload)
                {
                    ShootPlasma();
                    timer = 0f;
                }
                else
                {
                    timer += Time.fixedDeltaTime;
                }

            }
        }
    }

    private void EnemyWeaponChoice()
        {
            int weaponSelect = Random.Range(1, 5);
            if (weaponSelect == 1)
            {
                enemyWeapon = rpg;
            }
            else
            {
                enemyWeapon = smg;
            }
        }

        private bool canShoot()
        {
        Vector2 currPos = firePoint.position;
        Vector2 targetPos = AIDestinationSetter.currentTarget;
        Vector2 direction = targetPos - currPos;
        RaycastHit2D hit = Physics2D.Raycast(currPos, direction, 100);
        if (hit.collider != null)
        {
            Debug.DrawRay(currPos, direction* 100, Color.red);
            if (hit.collider.tag == "Player" || hit.collider.tag == "Core")
            {
                return true;
            }
        }
        return false;
    }
    
    private void ShootSmg()
    {
        GameObject bullet = Instantiate(enemyWeapon, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }
    
    private void ShootPlasma()
    {
        GameObject plasma = Instantiate(enemyWeapon, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = plasma.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }

    private IEnumerator checkMovement()
    {
        Vector2 startPos = _enemy.transform.position;
        yield return new WaitForSeconds(0.2f);
        Vector2 finalPos = _enemy.transform.position;
        if (startPos.x != finalPos.x || startPos.y != finalPos.y)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    private void DropPowerups()
    {
        int i = Random.Range(1, 4);
        if (i == 1)
        {
            if (enemyWeapon == smg)
            {
                GameObject p = Instantiate(SmgAmmo, _enemy.transform.position, Quaternion.identity);
            }
            else if (enemyWeapon == rpg)
            {
                GameObject p = Instantiate(RPGAmmo, _enemy.transform.position, Quaternion.identity);
            }
        }
        else if (i == 2)
        {
            GameObject p = Instantiate(Health, _enemy.transform.position, Quaternion.identity);
        }
        else if (i == 3)
        {
            GameObject p = Instantiate(Armour, _enemy.transform.position, Quaternion.identity);
        }
    }

    private void DeathAnimation(){
        animator.SetBool("isMoving", false);
        if (_currentCollider == "RPG")
        {
            animator.SetBool("hasExploded", true);
        }

        else if (_currentCollider == "Bullet")
        {
            animator.SetBool("isDead", true);
        }
        StartCoroutine(FinishAndDestroy());
    }
    
    private IEnumerator FinishAndDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        DropPowerups();
        Destroy(_enemy);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Bullet")
        {
            enemyHealth -= 20;
            PlayerController.playerScore += 5;
        }
        else if (col.collider.tag == "RPG")
        {
            enemyHealth = 0;
            PlayerController.playerScore += 100;
        }

        if (enemyHealth == 0)
        {
            _isDead = true;
        }

        _currentCollider = col.collider.tag;
    }

}
