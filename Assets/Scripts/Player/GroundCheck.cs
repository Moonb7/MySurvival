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

    // 캐릭터 컨트롤러의 Radius랑 같아야한다.
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
        // 바닥 체크
        spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);

        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, groundLayer, // Ground Layer에 접촉해있으면 Ground에 있는걸로 판별
            QueryTriggerInteraction.Ignore);

        // 애니
        animator.SetBool("IsGround", Grounded);
    }
}
