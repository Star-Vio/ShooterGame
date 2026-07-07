using UnityEngine;

public class MyPlayerMovement : MonoBehaviour
{
    public float Speed = 6f;

    private Rigidbody rb;
    private Animator animator;

    // 在 Update 中读取输入，FixedUpdate 中使用
    private float horizontal;
    private float vertical;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update：读取输入 + 更新动画参数
    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        // 切换动画
        Animator(horizontal, vertical);
    }

    // FixedUpdate：物理移动 + 旋转
    private void FixedUpdate()
    {
        Move(horizontal, vertical);
        Turning();
    }

    void Move(float h, float v)
    {
        Vector3 movementv3 = new Vector3(h, 0, v);
        movementv3 = movementv3.normalized * Speed * Time.fixedDeltaTime;

        rb.MovePosition(transform.position + movementv3);
    }

    void Turning()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        int floorLayer = LayerMask.GetMask("Floor");
        //Debug.Log($"floorLayer mask值: {floorLayer}");


        RaycastHit floorHit;
        bool isTouchFloor = Physics.Raycast(cameraRay, out floorHit, 100, floorLayer);
        //Debug.Log($"isTouchFloor: {isTouchFloor}");


        if (isTouchFloor)
        {
            Vector3 v3 = floorHit.point - transform.position;
            v3.y = 0;
            Quaternion quaternion = Quaternion.LookRotation(v3);
            rb.MoveRotation(quaternion);
        }
    }

    void Animator(float h, float v)
    {
        bool isWalking = (h != 0 || v != 0);
        animator.SetBool("IsWalking", isWalking);
    }
}
