using UnityEngine;

public class MyEnemyManager : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject CreatEnemyPoint;
    public float CreatBetween = 1f;
    public float CreatFirst = 0f;

    private void Start()
    {
        InvokeRepeating("Spawn",CreatFirst,CreatBetween);
    }

    private void Spawn()
    {
        Instantiate(Enemy,CreatEnemyPoint.transform.position,CreatEnemyPoint.transform.rotation);
    }
}
