using System.Data;
using UnityEngine;
using System;
using UnityEngine.UI;

public class MyPlayerShooting : MonoBehaviour
{
    float time = 0f;
    public float timeBetween = 0.15f;
    private AudioSource GunAudio;
    private Light gunLight;
    private float efforts = 0.2f;
    private LineRenderer Gunline;
    private ParticleSystem GunParticle;
    public int damage = 20;
    private GameObject Player;

    private Ray ShootRay;
    private RaycastHit ShootHit;
    private int ShootMask;

    private void Awake()
    {
        GunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
        Gunline = GetComponent<LineRenderer>();
        GunParticle = GetComponent<ParticleSystem>();
        ShootMask = LayerMask.GetMask("Enemy");
        Player = transform.parent.gameObject;
    }
    void Update()
    {
        time = time + Time.deltaTime;
        if (Input.GetButton("Fire1")&&time>=timeBetween)
        {
            Shoot();
        }

        if (time >= timeBetween*efforts)
        {
            gunLight.enabled = false;
            Gunline.enabled = false;
        }

    }

    void Shoot()
    {
        time = 0;

        gunLight.enabled = true;

        Gunline.enabled = true;
        Gunline.positionCount = 2;
        Gunline.SetPosition(0,transform.position);

        GunParticle.Play();

        GunAudio.Play();

        ShootRay.origin = transform.position;
        ShootRay.direction = transform.forward;
        if (Physics.Raycast(ShootRay, out ShootHit, 100, ShootMask))
        {
            Gunline.SetPosition(1, ShootHit.point);
            MyEnemyHealth health = ShootHit.collider.GetComponent<MyEnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(damage, ShootHit.point);
            }
        }
        else
        {
            Gunline.SetPosition(1, transform.position + transform.forward * 100);
        }
    }
}
