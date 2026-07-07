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
        // Update is called once per frame
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
        //Debug.Log($"Shoot() 被调用 — 帧: {Time.frameCount}, GunParticle: {GunParticle}, GunAudio: {GunAudio}, gunLight:{ gunLight}, Gunline: { Gunline}");
        time = 0;

        gunLight.enabled = true;

        //Debug.Log($"positionCount={Gunline.positionCount}, useWorldSpace={Gunline.useWorldSpace}");
        Gunline.enabled = true;
        Gunline.positionCount = 2;   // ← 加这行，确保是2
        Gunline.SetPosition(0,transform.position);

        GunParticle.Play();

        //Debug.Log(DateTime.Now.ToString("HH:mm:ss:fff"));
        GunAudio.Play();


        ShootRay.origin = transform.position;
        ShootRay.direction = transform.forward;
        if (Physics.Raycast(ShootRay, out ShootHit, 100, ShootMask))
        {
            //Debug.Log($"射线命中: {ShootHit.collider.gameObject.name}, 碰撞体: {ShootHit.collider.name}, 坐标:{ ShootHit.point}");
            Gunline.SetPosition(1, ShootHit.point);
            MyEnemyHealth health = ShootHit.collider.GetComponent<MyEnemyHealth>();
            if (health != null)
            {
                //Debug.Log($"找到 MyEnemyHealth，造成 {damage} 伤害");
                health.TakeDamage(damage, ShootHit.point);
            }
            else
            {
                //Debug.LogWarning($"命中 {ShootHit.collider.name} 但没找到 MyEnemyHealth 组件");
                EnemyHealth oldHealth = ShootHit.collider.GetComponent<EnemyHealth>();
                if (oldHealth != null)
                {
                    oldHealth.TakeDamage(damage, ShootHit.point);
                }
            }
            //Debug.Log(health.health);
        }
        else
        {
            //Debug.Log("射线未命中任何敌人");
            Gunline.SetPosition(1, transform.position + transform.forward * 100);
        }
        //Debug.Log($"弹道: 起点={Gunline.GetPosition(0)}, 终点={Gunline.GetPosition(1)}");
    }
}
