using System;
using UnityEngine;

public class MyEnemyAttack : MonoBehaviour
{
    public int Dmage = 10;
    private bool PlayerInRange = false;
    private GameObject player;
    private MyPlayerHealth MyPlayerHealth;
    private float time = 0f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        MyPlayerHealth = player.GetComponent<MyPlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (PlayerInRange == true&&time>=0.8f)
        {
            //如果玩家离敌人很近，造成伤害
            Attack();
            time = 0f;
        }
    }

    private void Attack()
    {
        //获取玩家血量组件
        MyPlayerHealth.TakeDamage(Dmage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
            PlayerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            PlayerInRange= false;
    }
}
