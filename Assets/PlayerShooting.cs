using UnityEngine;
using TMPro;
public class PlayerShooting : MonoBehaviour
{
    public Transform firePoint;

    public GameObject smg;
    public GameObject rpg;
    public TextMeshProUGUI smgAmmoUI;
    public TextMeshProUGUI rpgAmmoUI;

    public float bulletForce = 20f;
    
    public static int smgAmmo = 250;
    public static int rpgAmmo = 5;
    
    private GameObject _selected_weapon;


    private void Start()
    {
        _selected_weapon = smg;
    }

    // Update is called once per frame
    void Update()
    {
        smgAmmoUI.text = smgAmmo.ToString();
        rpgAmmoUI.text = rpgAmmo.ToString();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _selected_weapon = smg;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _selected_weapon = rpg;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (_selected_weapon == smg)
            {
                if (smgAmmo > 0)
                {
                    smgAmmo -= 1;
                    ShootSmg();
                }
            }

            else if (_selected_weapon == rpg)
            {
                if (rpgAmmo > 0)
                {
                    rpgAmmo -= 1;
                    ShootPlasma();
                }
            }
        }
    }

    void ShootSmg()
    {
        GameObject bullet = Instantiate(_selected_weapon, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }
    
    void ShootPlasma()
    {
        GameObject plasma = Instantiate(_selected_weapon, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = plasma.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }
}
