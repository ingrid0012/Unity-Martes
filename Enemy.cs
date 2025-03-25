using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int health = 100; // la salud en 100 
    [SerializeField] protected float attackDistance = 3; //distancia max para que pueda atacar
    [SerializeField] protected int damage = 10; //daño que el enemigo va hacer
    [SerializeField] protected float cooldown = 2; //el tiempo entre ataques
    protected GameObject player; //jugador 
    protected Animator anim; //animaciones
    protected Rigidbody rb; 
    protected float distance; //almacenar la distancia entre el personaje y enemigo
    protected float timer; //Lleva un registro del tiempo transcurrido
    bool dead = false; // Indica si el personaje está muerto.

    public virtual void Move() //funcion para el movimiento
    {
    }
    public virtual void Attack() //funcion para el ataque
    {
    }
    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        if (!dead)
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
    if (!dead)
    {
        Move();
    }
    }

    public void ChangeHealth(int count)
    {
        health -= count;
        if(health <= 0)
        {
            dead = true;
            GetComponent<Collider>().enabled = false;
            anim.SetBool("Die", true);
        }
    }
}