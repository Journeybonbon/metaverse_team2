using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [Header("Boxcast Property")]
    [SerializeField] private Vector3 boxSize;
    [SerializeField] private float maxDistance;
    [SerializeField] private LayerMask groundLayer;

    [Header("Debug")]
    [SerializeField] private bool drawGizmo;

    private void OnDrawGizmos()
    {
        if (!drawGizmo) return;

        Gizmos.color = Color.cyan;
        //Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSize);

        // 함수 파라미터 : 현재 위치, Box의 절반 사이즈, Ray의 방향, RaycastHit 결과, Box의 회전값, BoxCast를 진행할 거리
        if (true == Physics.BoxCast(transform.position, boxSize / 2.0f, -transform.up, out RaycastHit hit, transform.rotation, maxDistance))
        {
            // Hit된 지점까지 ray를 그려준다.
            Gizmos.DrawRay(transform.position, -transform.up * hit.distance);

            // Hit된 지점에 박스를 그려준다.
            Gizmos.DrawWireCube(transform.position + - transform.up * hit.distance, boxSize);
        }
        else
        {
            // Hit가 되지 않았으면 최대 검출 거리로 ray를 그려준다.
            Gizmos.DrawRay(transform.position, -transform.up * maxDistance);
        }

    }

    public bool IsGrounded()
    {
        return Physics.BoxCast(transform.position, boxSize / 2.0f, -transform.up, out RaycastHit hit, transform.rotation, maxDistance, groundLayer);
    }
}
