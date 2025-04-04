using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //trabajar el canvas
using Photon.Pun;




public class PlayerController : MonoBehaviourPunCallbacks
{
    public enum Weapons //m3l4 
    {
        None,
        Pistol,
        Rifle,
        MiniGun
    }
    Weapons weapons = Weapons.None; //m3l4
    
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] GameObject pistol, rifle, miniGun; //m3l4
    bool isPistol, isRifle, isMiniGun; //m3l4
    float currentSpeed;


    

    [SerializeField] Rigidbody rb;
    Vector3 direction;

    [SerializeField] float shiftSpeed = 10f;
    [SerializeField] float jumpForce = 7f;
    float stamina = 5f;

    bool isGrounded = true;

    [SerializeField] Animator anim;  
    [SerializeField] Image pistolUI, rifleUI, miniGunUI, cursor;
    
    private int health; // la salud de personaje

    [SerializeField] AudioSource characterSounds; // Hacemos referencia al AudioSource
    [SerializeField] AudioClip jump; // Hacemos referencia al clip de sonido del salto

   
    void Start()
    {        
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        currentSpeed = movementSpeed;
        health=100; 
       if (!photonView.IsMine)
        {                       
            transform.Find("Main Camera").gameObject.SetActive(false);
            Destroy(GetComponent<PlayerController>());
        }
    }

    public void ChangeHealth(int count)
    {
        health -= count; //restando vida 
        if (health<=0) // si la salud llega a cero algo pasará 
        {
            anim.SetBool("Die", true); //animación
            ChooseWeapon(Weapons.None); //el arma desaparece
            this.enabled = false; // no nos vamos a poder mover 
        }
    }



    
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        direction = new Vector3(moveHorizontal, 0.0f, moveVertical);
        direction = transform.TransformDirection(direction);
        if(direction.x != 0 || direction.z != 0)
        {
            anim.SetBool("Run", true);
            if(!characterSounds.isPlaying&& isGrounded)
            {
                characterSounds.Play();
            }
        }
        if(direction.x == 0 && direction.z == 0)
        {
            anim.SetBool("Run", false);
            characterSounds.Stop();
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            isGrounded = false;
            characterSounds.Stop(); //se desactiva el sonido de correr
            AudioSource.PlayClipAtPoint(jump, transform.position);
            anim.SetBool("Jump", true);
        }
        if(Input.GetKey(KeyCode.LeftShift))
        {
            if(stamina > 0)
            {
                stamina -= Time.deltaTime;
                currentSpeed = shiftSpeed;
            }
            else
            {
                currentSpeed = movementSpeed;
            }
        }
        else if (!Input.GetKey(KeyCode.LeftShift))
        {            
            stamina += Time.deltaTime;                      
            currentSpeed = movementSpeed;
        }
        if(stamina > 5f)
        {
            stamina = 5f;
        }
        else if (stamina < 0)
        {
            stamina = 0;
        }
        if(Input.GetKeyDown(KeyCode.Alpha1) && isPistol) //m3l4
        {
            ChooseWeapon(Weapons.Pistol);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && isRifle)
        {
            ChooseWeapon(Weapons.Rifle);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && isMiniGun)
        {
            ChooseWeapon(Weapons.MiniGun);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ChooseWeapon(Weapons.None);
        }

    }
    public void ChooseWeapon(Weapons weapons) //m3l4
    {
        anim.SetBool("Pistol", weapons == Weapons.Pistol);
        anim.SetBool("Assault", weapons == Weapons.Rifle);
        anim.SetBool("MiniGun", weapons == Weapons.MiniGun);
        anim.SetBool("NoWeapon", weapons == Weapons.None);
        pistol.SetActive(weapons == Weapons.Pistol);
        rifle.SetActive(weapons == Weapons.Rifle);
        miniGun.SetActive(weapons == Weapons.MiniGun);

        if(weapons != Weapons.None) //activamos o desactivamos el cursor 
        {
            cursor.enabled=true;
        }
        else
        {
            cursor.enabled=false;
        }

        
    }
    
    void FixedUpdate()
    {
        rb.MovePosition(transform.position + direction * currentSpeed * Time.deltaTime);
    }
    void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
        anim.SetBool("Jump", false);
    }
    private void OnTriggerEnter(Collider other) //m3l4
    {
        switch (other.gameObject.tag) 
        {
            case "pistol":
                if (!isPistol)
                {
                    isPistol = true;
                    pistolUI.color=Color.green; //el arma cuando la tenga cambie a V.
                    ChooseWeapon(Weapons.Pistol);
                }
                break;

            case "rifle":
                if (!isRifle)
                {
                    isRifle = true;
                    rifleUI.color=Color.green;
                    ChooseWeapon(Weapons.Rifle);
                }
                break;

            case "minigun":
                if (!isMiniGun)
                {
                    isMiniGun = true;
                    miniGunUI.color=Color.green;
                    ChooseWeapon(Weapons.MiniGun);
                }
                break;
            default:
                break;
        }
        Destroy(other.gameObject);
    }   



}
