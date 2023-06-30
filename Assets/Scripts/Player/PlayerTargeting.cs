using UnityEngine;
using UnityEngine.UI;

public class PlayerTargeting : MonoBehaviour // Ground üũ�ϴ� Ŭ������ ��ġ���� �θ� Ŭ������ ���� �ɰ� ����.
{
    private Vector3 spherePosition;
    [SerializeField] 
    private float aroundOffset;
    [SerializeField]
    private LayerMask EnemyLayer;

    
    // ĳ���� ��Ʈ�ѷ��� Radius�� ���ƾ��Ѵ�.
    [SerializeField] 
    private float radius;

    [SerializeField] 
    private bool drawGizmo;

    public new Collider[] collider;

    public static GameObject enemy = null;
    public static bool fastentargeting;
    public static GameObject targetEnemy = null;

    public RectTransform targetUI;
    public RawImage targetImage; // Ÿ�� �̹����� ����Ű�� Image ������Ʈ�� ������ ����

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
   
    public void Targeting() // ���尡��� Ÿ����� �����ҋ��� Ÿ���� ��Ҵ�. //��������Ʈ�� ���� �������� �̺�Ʈ�� ���� ��Ű�� �ɰ� ����.
    {
        spherePosition = new Vector3(transform.position.x, transform.position.y - aroundOffset,
            transform.position.z);

        collider = Physics.OverlapSphere(spherePosition, radius, EnemyLayer,
            QueryTriggerInteraction.Ignore);

        // ���⼭�� �������� �־� �� ī�޶� ��� ��鸸 ã��
        planes = GeometryUtility.CalculateFrustumPlanes(cam);

        float minDistance = float.MaxValue;

        if (collider != null && collider.Length > 0 && GeometryUtility.TestPlanesAABB(planes, collider[0].bounds)) // ���⼭ ���� �߰� ī�޶� ����ȭ�鿡 ���̴� ���� ã�� �ִ��� Ȯ��
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

            /*if (InputManager.Instance.attackKey)  // ���� �� ���� �����ҋ� �ڵ����� ����� ���� ������ ���ؼ� ������ // ������ �ڵ����� �Ĵٺ��� �����ؼ� ������ ������ ����.
            {
                if (fastentageting && targetEnemy) // ������ �ϸ� ���������� ������ ���� ������ ����
                {
                    transform.LookAt(targetEnemy.transform.position);
                    Debug.Log(targetEnemy.name);
                    
                }
                else // ������ ���� ������ ����� ���� ������ ����
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

        SetTargetImage(); // Ÿ���õ� ��Enemy�� Ÿ�� �̹��� ���̰� ����
    }

    void SetTargetImage()
    {
        if (fastentargeting && targetEnemy)
        {
            targetImage.gameObject.SetActive(true);
            Vector3 targetCenter = targetEnemy.GetComponent<Collider>().bounds.center; // �ݶ��̴��� ������ġ�� Ȯ��
            Vector3 viewportPosition = Camera.main.WorldToViewportPoint(targetCenter); // ī�޶� ���̰� �ϱ����� ����Ʈ�����ͷ� ��ȯ

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
