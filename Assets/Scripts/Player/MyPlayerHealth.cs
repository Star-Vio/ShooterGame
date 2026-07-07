using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyPlayerHealth : MonoBehaviour
{
    //���Ѫ��
    public int PlayerHealth = 100;
    //����Ƿ�����
    public bool isPlayerDead = false;
    private AudioSource PlayerAudioSource;
    public AudioClip PlayerDeathAudio;
    private CapsuleCollider PlayerCapsuleCollider;
    private Animator PlayerAnimator;
    private MyPlayerMovement MyPlayerMovement;
    private MyPlayerShooting MyPlayerShooting;
    private bool isDamage = false;
    public Text PlayerHealthUI;
    public Image PlayerHurtUI;
    public Color FlashColor = new Color(1f, 0f, 0f, 0.2f);
    

    private void Awake()
    {
        PlayerAudioSource = GetComponent<AudioSource>();
        PlayerCapsuleCollider = GetComponent<CapsuleCollider>();
        PlayerAnimator = GetComponent<Animator>();
        MyPlayerMovement = GetComponent<MyPlayerMovement>();
        MyPlayerShooting = GetComponentInChildren<MyPlayerShooting>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDamage) 
        { 
            PlayerHurtUI.color = FlashColor;
            isDamage = false;
        }
        else 
        {
            PlayerHurtUI.color=Color.Lerp(PlayerHurtUI.color,Color.clear,5f*Time.deltaTime);
        }
    }

    public void TakeDamage(int Damage)
    {
        if(isPlayerDead)
            return;
        PlayerHealth -= Damage;

        //更新玩家血量UI
        PlayerHealthUI.text = PlayerHealth.ToString();

        isDamage = true;
        if (PlayerHealth <= 0) { 
            isPlayerDead = true;
            Death();
        }
        //Debug.Log(PlayerHealth);
        PlayerAudioSource.Play();
    }
    private void Death()
    {
        //Enemy.SphereCollider.enable = false;
        MyPlayerMovement.enabled = false;
        MyPlayerShooting.enabled = false;
        //������������
        PlayerAnimator.SetTrigger("Death");

        // 敌人是运行时动态生成的，用到时再找
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy != null)
            enemy.GetComponent<Animator>().SetTrigger("PlayerDeath");

        //����������Ч
        PlayerAudioSource.clip = PlayerDeathAudio;
        PlayerAudioSource.Play();

        GetComponent<Rigidbody>().isKinematic = true;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }
}
