using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private Animator animator;

    private Vector3 spherePosition;
    [SerializeField]
    private float GroundedOffset;
    [SerializeField]
    private LayerMask groundLayer;

    // ĳ���� ��Ʈ�ѷ��� Radius�� ���ƾ��Ѵ�.
    [SerializeField]
    private float GroundedRadius = 0.28f;
    [SerializeField]
    private bool drawGizmo;

    public bool Grounded;
    

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
   
    private void Update()
    {
        GroundedCheck();
    }
    private void OnDrawGizmos()
    {
        if (!drawGizmo) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position - transform.up * GroundedOffset, GroundedRadius);
    }

    public void GroundedCheck()
    {
        // �ٴ� üũ
        spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);

        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, groundLayer, // Ground Layer�� ������������ Ground�� �ִ°ɷ� �Ǻ�
            QueryTriggerInteraction.Ignore);

        // �ִ�
        animator.SetBool("IsGround", Grounded);
    }
}
