using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;



public class Weapon : MonoBehaviourPunCallbacks //clase padre
{
    //El objeto Sistema de Partículas (Particle System) que creará los agujeros de las balas
    [SerializeField] protected GameObject particle;
    //La cámara (nos ayudará a encontrar el centro de la pantalla)
    [SerializeField] protected GameObject cam;
    //El modo de disparo del arma
    protected bool auto = false;
    //El intervalo entre los disparos y el contador que lleva el tiempo
    protected float cooldown = 0;
    protected  float timer = 0;

    protected int ammoCurrent; //add 4 numero de balas en el cargador 
    protected int ammoMax; // maxima capacidad del cargador add 4

    protected int ammoBackPack; //municion de reserva

    [SerializeField] TMP_Text ammoText; //representa el texto UI 

    [SerializeField] AudioSource shoot;
    [SerializeField] AudioClip bulletSound, noBulletSound, reload;

    //Al arrancar el juego, definimos el contador para que sea igual al tiempo de enfriamiento del arma
    //Esto nos asegura que se produzca el primer disparo sin demora
    private void Start()
    {
        timer = cooldown;
    }
    private void Update()
    {
        if(photonView.IsMine)
        {
            //Arrancando el contador
            timer += Time.deltaTime;
            //Si el jugador tiene el botón izquierdo del mouse presionado, llamamos la función Shoot
            if (Input.GetMouseButton(0))
            {
                Shoot();
            }
            AmmoTextUpdate(); //4 

            //Si un jugador presiona la tecla R
            if (Input.GetKeyDown(KeyCode.R))
            {
            //Si nuestro cargador no está lleno, O si tenemos al menos una bala en las reservas, entonces
            if(ammoCurrent != ammoMax || ammoBackPack != 0)
            {
                //Activar la función de recarga con un ligero retraso.
                //Puedes configurar el retraso con cualquier número que desees
                shoot.PlayOneShot(reload);
                Invoke("Reload", 1);
            }
            }
        }
    }

    
    //Revisando si el arma puede ser disparada o no
    public void Shoot()
    {
        if (Input.GetMouseButtonDown(0) || auto)
        {
            if (timer > cooldown)
            {
                if(ammoCurrent>0)//add
                {
                    OnShoot();
                    timer = 0;
                    ammoCurrent=ammoCurrent-1;//add 4
                    shoot.PlayOneShot(bulletSound);
                    shoot.pitch=Random.Range(1f, 1.5f);
                }
                else
                {
                    shoot.PlayOneShot(noBulletSound);
                }
                
            }
        }
    }
    //Y esta función definirá qué pasa cuando se dispare el arma. Ya que tiene los modificadores protected y virtual, las clases que se derivan de esta podrán definir su propia lógica de disparo
    protected virtual void OnShoot()
    {
    }

    private void AmmoTextUpdate()
    {
        ammoText.text = ammoCurrent + "/" + ammoBackPack; //4
    }

    private void Reload() //add 4
    {
    //declarando una variable y calculando el número de balas que debemos añadir al cargador
    int ammoNeed = ammoMax - ammoCurrent;
    //Si la cantidad de balas de reserva que tenemos es mayor o igual a la cantidad de balas necesarias para recargar, entonces
    if (ammoBackPack >= ammoNeed)
    {
        //restando el número de balas necesarias de las reservas
        ammoBackPack -= ammoNeed;
        //añadiendo el número necesario de balas al cargador
        ammoCurrent += ammoNeed;
    }
    //de lo contrario (si las reservas tienen menos balas de las necesarias para una recarga completa)
    else
    {
        //añadiendo toda nuestra munición de reserva al cargador
        ammoCurrent += ammoBackPack;
        //establecer la munición de reserva en 0
        ammoBackPack = 0;
    }
    }
}
