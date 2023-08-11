using UnityEngine;
using UnityEngine.UI;

public class PlayerTargeting : MonoBehaviour
{
    private Vector3 spherePosition;
    [SerializeField] 
    private float aroundOffset;
    [SerializeField]
    private LayerMask EnemyLayer;
    [SerializeField] 
    private float radius; // 캐릭터 컨트롤러 스크립트의 Radius랑 같아야한다.

    [SerializeField] 
    private bool drawGizmo;

    public new Collider[] collider;

    public static GameObject enemy = null;
    public static bool fastentargeting;
    public static GameObject targetEnemy = null;

    public RectTransform targetUI;
    public RawImage targetImage; // 타겟 이미지를 가리키는 Image 컴포넌트를 저장할 변수

    private Camera cam;
    private Plane[] planes;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        Targeting();
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmo) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position - transform.up * aroundOffset, radius);
    }
   
    public void Targeting() // 가장가까운 타켓잡기 공격할떄만 타겟을 잡았다. //델리게이트로 공격 헀으때의 이벤트로 포함 시키면 될거 같다.
    {
        spherePosition = new Vector3(transform.position.x, transform.position.y - aroundOffset,
            transform.position.z);

        collider = Physics.OverlapSphere(spherePosition, radius, EnemyLayer,
            QueryTriggerInteraction.Ignore);        
        planes = GeometryUtility.CalculateFrustumPlanes(cam); // 시야에 보이는 적만 찾기위해

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

        SetTargetImage(); // 타겟팅된 적Enemy에 타겟 이미지 보이게 설정
    }

    void SetTargetImage()
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
}
