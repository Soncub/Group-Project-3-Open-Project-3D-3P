using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Vector3Helper;

public class EnemyRanged : EnemyBase
{
    //Config
    public LayerMask whatIsGround, whatIsPlayer;
    public float timeBetweenAttacks;
    public float sightRange, attackRange;
    private bool inSightRange, inAttackRange;
    public float throwSpeed = 15f;
    
    //Connections
    public NavMeshAgent navAgent;
    ObjectPool pool;
    Animator anim;

    //Data
    float timeLeftBeforeAttack;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        navAgent = GetComponent<NavMeshAgent>();
        pool = GetComponent<ObjectPool>();
        timeLeftBeforeAttack = timeBetweenAttacks;
        anim = GetComponentInChildren<Animator>();
    }
    
    private void Update()
    {
        inSightRange = distanceFromPlayer < sightRange;
        inAttackRange = distanceFromPlayer < attackRange;

        if (!inSightRange && !inAttackRange) Idle();
        if (inSightRange && !inAttackRange) ChasePlayer();
        if (inSightRange && inAttackRange) AttackPlayer();

        anim.SetFloat("Speed", navAgent.velocity.magnitude);
    }

    private void Idle()
    {

    }

    private void ChasePlayer()
    {
        navAgent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        navAgent.SetDestination(transform.position);
        transform.LookAt(player.position * Direction.XZ);

        if(timeLeftBeforeAttack > 0)
        {
            timeLeftBeforeAttack -= Time.deltaTime;
        }
        else
        {
            anim.SetTrigger("Attack");

            /*
            sphere.gameObject.SetActive(true);
            sphere.transform.position = transform.position;
            sphere.transform.eulerAngles = Vector3.zero;
            Rigidbody rb = sphere.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
             */

            timeLeftBeforeAttack = timeBetweenAttacks;
        }

    }

    public void AttackCallback()
    {
        audio.PlaySound("Attack");
        PoolableObject bullet = pool.Pump();
        bullet.Prepare_Basic(transform.position, Vector3.zero, transform.forward * throwSpeed + transform.up * 2f);
    }

}

