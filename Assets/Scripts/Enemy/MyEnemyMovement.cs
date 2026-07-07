using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MyEnemyMovement : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent nav;
    private MyEnemyHealth Health;
    private MyPlayerHealth MyPlayerHealth;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        nav = GetComponent<NavMeshAgent>();
        Health = GetComponent<MyEnemyHealth>();
        MyPlayerHealth=player.GetComponent<MyPlayerHealth>();
    }
    // Update is called once per frame
    void Update()
    {
        //ÊÇ·ñËÀÍö
        if(Health.health>0&&!MyPlayerHealth.isPlayerDead)
            nav.SetDestination(player.transform.position);
        else
            nav.enabled = false;
    }
}
