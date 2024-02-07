using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTargeting : MonoBehaviour
{
    [SerializeField] 
    private float aroundOffset;
    [SerializeField]
    private LayerMask enemyLayer;
    [SerializeField] 
    private float radius; // 캐릭터 컨트롤러 스크립트의 Radius랑 같아야한다.

    [SerializeField] 
    private bool drawGizmo;

    public new Collider[] collider;

    public GameObject enemy = null;
    public bool fastentargeting;
    public GameObject targetEnemy = null;

    public RectTransform targetUI;
    public RawImage targetImage; // 타겟 이미지를 가리키는 Image 컴포넌트를 저장할 변수
    private Plane[] planes;

    [Header("Aim 총무기시 이용할 예정")]
    [SerializeField] // 조준 할떄 쓸 카메라 총무기를 들떄만 이용할 거다
    private CinemachineVirtualCamera aimCam;
    [SerializeField]
    private GameObject aimImage;

    //private float distanceToTarget;
    //public float toTarget; // 확인용

    private void Update()
    {
        SetTargetImage();
        AimCheck();
    }
   
    public void Targeting() // 가장가까운 타켓잡기 공격할떄만 타겟을 잡았다. // 이거는 업데이트 보단 한번 실행으로 하는게 좋을거 같다
    {
        collider = Physics.OverlapSphere(transform.position, radius, enemyLayer,
            QueryTriggerInteraction.Ignore);        
        planes = GeometryUtility.CalculateFrustumPlanes(Camera.main); // 카메라 시야에 보이는 적만 찾기위해

        float minDistance = float.MaxValue;

        if (collider != null && collider.Length > 0 && GeometryUtility.TestPlanesAABB(planes, collider[0].bounds)) // 여기서 조건 추가 카메라에 콜라이더가 붙은 게임화면에 보이는 적만 찾고 있는지 확인
        {
            for (int i = 0; i < collider.Length; i++)
            {
                float distance = Vector3.Distance(transform.position, collider[i].transform.position);

                if (distance < minDistance) 
                {
                    minDistance = distance;
                    enemy = collider[i].gameObject;
                }

            }
            /*if (InputManager.Instance.attackKey)  // 범위 안 적을 공격할떄 자동으로 가까운 적을 지정을 정해서 때린다 // 이제는 자동으로 쳐다보게 고정해서 쓸일이 없을거 같다.
            {
                if (fastentageting && targetEnemy) // 고정을 하면 마지막으로 고정한 적만 떄리게 설정
                {
                    transform.LookAt(targetEnemy.transform.position);
                    Debug.Log(targetEnemy.name);
                    
                }
                else // 고정을 하지 않으면 가까운 적을 때리게 설정
                {
                    transform.LookAt(enemy.transform.position);
                    Debug.Log(enemy.name);
                }
            }*/
        }
        else
        {
            fastentargeting = false;
            targetEnemy = null;
        }
    }

    void SetTargetImage()
    {
        if (WeaponManager.activeWeapon.weaponScriptable.weaponType == WeaponType.meleeweapon) // 근거리 일때의 타켓팅
        {
            if (fastentargeting && targetEnemy)
            {
                targetImage.gameObject.SetActive(true);
                Vector3 targetCenter = targetEnemy.GetComponent<Collider>().bounds.center; // 콜라이더의 센터위치값 확인
                Vector3 viewportPosition = Camera.main.WorldToViewportPoint(targetCenter); // 카메라에 보이게 하기위해 뷰포트포인터로 변환

                Vector2 canvasSize = targetUI.sizeDelta;
                Vector2 anchoredPosition = new Vector2(viewportPosition.x * canvasSize.x - canvasSize.x * 0.5f, viewportPosition.y * canvasSize.y - canvasSize.y * 0.5f);

                targetImage.rectTransform.anchoredPosition = anchoredPosition;
            }
            else
            {
                targetImage.gameObject.SetActive(false);
            }
        }
        else if (WeaponManager.activeWeapon.weaponScriptable.weaponType == WeaponType.rangedweapon) // 원거리 일때의 타켓팅은 하지않는게 좋을거 같다
        {
            /*RaycastHit hit;
            //Physics.Raycast(레이저를 발사할 위치, 발사방향, 히트 충돌체 정보, 최대거리)
            if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit))
            {
                distanceToTarget = hit.distance;
                toTarget = distanceToTarget;
            }*/
        }
    }
    private void AimCheck()
    {
        Transform camTransform = Camera.main.transform;
        RaycastHit hit;

        if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, Mathf.Infinity,enemyLayer))
        {
            Debug.Log("Name : " + hit.transform.gameObject.name);
        }

        if (InputManager.Instance.AimKey) // 조건 추가 해야 한다 총무기를 들었을때만 이용하게 만들예정
        {
            aimCam.gameObject.SetActive(true);
            aimImage.SetActive(true);
        }
        else
        {
            aimCam.gameObject.SetActive(false);
            aimImage.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmo) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position - transform.up * aroundOffset, radius);
    }
}
