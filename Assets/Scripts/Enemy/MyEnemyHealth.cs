using System;
using UnityEngine;
using UnityEngine.AI;

public class MyEnemyHealth : MonoBehaviour
{
    public int health = 100;
    public AudioClip DeathClip;
    private AudioSource enemyHurt;
    private ParticleSystem enemyParticle;
    private Animator animator;
    private CapsuleCollider enemyCapsuleCollider;
    private bool isSiking = false;
    private bool isDead = false;

    private void Awake()
    {
        enemyHurt = GetComponent<AudioSource>();
        enemyParticle = GetComponentInChildren<ParticleSystem>();
        animator = GetComponent<Animator>();
        enemyCapsuleCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSiking)
        {
            transform.Translate(-transform.up*Time.deltaTime);
        }
    }

    public void TakeDamage(int Dmage,Vector3 ShootHit)
    {
        if (isDead) 
            return;
        if (enemyHurt != null)
            enemyHurt.Play();

        if (enemyParticle != null)
        {
            enemyParticle.transform.position = ShootHit;
            enemyParticle.Play();
        }

        health -= Dmage;
        //Debug.Log($"µÐÈË¿ÛÑª: {Dmage}, Ê£ÓàÑªÁ¿: {health}");
        //Debug.Log(health);

        if (health <= 0)
            Death();
    }

    private void Death()
    {
        MyPlayerScores.Scores += 10;
        isDead = true;
        if (enemyCapsuleCollider != null)
            enemyCapsuleCollider.enabled = false;

        if (animator != null)
            animator.SetTrigger("Death");

        if (enemyHurt != null)
        {
            enemyHurt.clip = DeathClip;
            enemyHurt.Play();
        }

        var nav = GetComponent<NavMeshAgent>();
        if (nav != null)
            nav.enabled = false;

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;
    }
    public void StartSinking()
    {
        isSiking = true;
        Destroy(gameObject,2f);
    }
}
