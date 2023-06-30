using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    // 무기 위치
    public Transform defaultWeaponPosition; // 대기 원래위치
    public Transform weaponmountPos;             // 장착위치

    // 시작무기 설정 - Player가 가질 무기를 3개로 정했다
    public List<WeaponController> startingWeapons = new List<WeaponController>();
    public WeaponController[] weaponSlots = new WeaponController[3];

    public static WeaponController activeWeapon;

    private float layerAtkModeTime;
    private float layerSwapTime;
    private float countDown;

    [Tooltip("공격모드 유지 시간")]
    [SerializeField]
    private float attackTime = 5f;
    private bool attackMode;
    //public static bool isAttack; // 공격 애니메이션이 끝나는 타이밍을 갖기위해 만들었다.

    // 무기 교체
    private bool notChange = true;
    [SerializeField]
    private float changeweaponDelayTime = 1f;  // 무기 교체 딜레이 시간.
    [SerializeField]
    private float changeweaponEndDelayTime = 1f; // 무기 교체 딜레이 시간.

    private bool IsMounting
    {
        get { return PlayerNewInputController.animator.GetBool(AnimString.Instance.isMounting); }
        set { PlayerNewInputController.animator.SetBool(AnimString.Instance.isMounting, value); }
    }

    void Start()
    {
        // 지급받은 무기 장착
        foreach (var w in startingWeapons)
        {
            AddWeapon(w);
        }
        activeWeapon = weaponSlots[0];
    }

    void Update()
    {
        AttackMode();

        UpadateChange();
    }

    // 공격 애니매이션 실행 및 공격상태 유지 후 공격하지 않을때 대기상태
    public void AttackMode()
    {
        PlayerNewInputController.animator.SetLayerWeight(1, layerAtkModeTime);
        PlayerNewInputController.animator.SetBool(AnimString.Instance.attackMode, attackMode);

        if (InputManager.Instance.attackKey && PlayerNewInputController.animator.GetBool(AnimString.Instance.isGround) && notChange)
        {
            if(activeWeapon != null)
            {
                StartCoroutine(AttackbyType()); // 실제 공격

                layerAtkModeTime = 1;
                countDown = 0;
                attackMode = true;
            }
        }
        else if (countDown > attackTime && attackMode)
        {
            if (layerAtkModeTime > 0)
            {
                layerAtkModeTime -= Time.deltaTime;

                if (layerAtkModeTime <= 0)
                {
                    layerAtkModeTime = 0;

                    attackMode = false;
                }
            }
        }
        countDown += Time.deltaTime;
    }

    //
    
    // 타입별 공격
    IEnumerator AttackbyType()
    {
        switch (activeWeapon.WeaponType)
        {
            case WeaponAttackType.Boxing:
                // 애는 그냥 애니매이션으로 공격을 줬다.
                break;
            case WeaponAttackType.Sword:  //----------------------------------------------------------
                Collider collider = activeWeapon.GetComponent<Collider>();

                yield return new WaitForSeconds(0.3f);
                collider.enabled = true;
                yield return new WaitForSeconds(0.2f);
                collider.enabled = false;

                break;
            case WeaponAttackType.Gun:

                break;
        }
        yield return null;
    }

    public float layerSpeed = 1.5f;
    void UpadateChange()
    {
        
        // 무기 변환은 상체 애니 매이션만 쓸려고 마스크를 만들어 이용하였습니다.
        PlayerNewInputController.animator.SetLayerWeight(2, layerSwapTime);
        if (notChange)
        {
            if (InputManager.Instance.weapon1Key)
                StartCoroutine(WeaponInChange(0)); // 2번무기
            if (InputManager.Instance.weapon2Key)
                StartCoroutine(WeaponInChange(1)); // 2번무기
            if (InputManager.Instance.weapon3Key)
                StartCoroutine(WeaponInChange(2)); // 3번 무기

            layerSwapTime -= Time.deltaTime * layerSpeed;
            if (layerSwapTime <= 0)
            {
                layerSwapTime = 0;
            }
                
        }
    }

    // 무기를 바꾸거나 꺼내는 함수
    IEnumerator WeaponInChange(int ActiveWeaponIndex)
    {
        if (weaponSlots[ActiveWeaponIndex] == activeWeapon || PlayerNewInputController.animator.GetBool(AnimString.Instance.isAttack))
        {
            yield break; // 실행 하면 안되는 조건들
        }

        notChange = false;

        layerSwapTime = 1;
        

        if (IsMounting)
        {
            WeaponController oldWeapon = activeWeapon;

            IsMounting = false;
            yield return new WaitForSeconds(changeweaponDelayTime);

            // 원래자리 집어 넣기
            oldWeapon.transform.parent = defaultWeaponPosition;
            oldWeapon.transform.localPosition = Vector3.zero;          // transform값 들은 다 부모 값으로 지정
            oldWeapon.transform.localRotation = Quaternion.identity;


            // 무기 장착
            if (weaponSlots[ActiveWeaponIndex] != null)
            {
                activeWeapon = weaponSlots[ActiveWeaponIndex];
                activeWeapon.transform.parent = weaponmountPos;
                activeWeapon.transform.localPosition = Vector3.zero;          // transform값 들은 다 부모 값으로 지정
                activeWeapon.transform.localRotation = Quaternion.identity;
            }
            
            IsMounting = true;
            PlayerNewInputController.animator.SetTrigger(AnimString.Instance.weaponChange);
            yield return new WaitForSeconds(changeweaponEndDelayTime);
        }
        else
        {
            if (weaponSlots[ActiveWeaponIndex] != null)
            {
                activeWeapon = weaponSlots[ActiveWeaponIndex];
                activeWeapon.transform.parent = weaponmountPos;
                activeWeapon.transform.localPosition = Vector3.zero;          // transform값 들은 다 부모 값으로 지정
                activeWeapon.transform.localRotation = Quaternion.identity;
            }

            IsMounting = true;
            PlayerNewInputController.animator.SetTrigger(AnimString.Instance.weaponChange);
            yield return new WaitForSeconds(changeweaponEndDelayTime);
        }

        ChangeSet(activeWeapon);
        
        notChange = true;
    }

    /*// 처음 기본 무기로 돌아오는 함수
    IEnumerator WeaponOutChange()
    {
        if (IsMounting == false)
            yield break;

        notChange = false;
        PlayerNewInputController.animator.SetLayerWeight(2, 1f);

        IsMounting = false;
        yield return new WaitForSeconds(changeweaponDelayTime);

        WeaponController oldWeapon = activeWeapon;
        oldWeapon.transform.parent = defaultWeaponPosition;
        oldWeapon.transform.localPosition = Vector3.zero;          // transform값 들은 다 부모 값으로 지정
        oldWeapon.transform.localRotation = Quaternion.identity;

        activeWeapon = weaponSlots[0];

        ChangeSet(activeWeapon);

        PlayerNewInputController.animator.SetLayerWeight(2, 0f);
        notChange = true;
    }*/

    void ChangeSet(WeaponController changeWeapon) // WeaponAttackType순서를 attackStats 에 맞게 조절했다
    {
        if (changeWeapon == null) 
            return;
        PlayerNewInputController.animator.SetInteger(AnimString.Instance.weaponStats, (int)changeWeapon.WeaponType);

    }

    public bool AddWeapon(WeaponController weaponPrefab)
    {
        if(HasWeapon(weaponPrefab) != null)
        {
            Debug.Log("같은 무기 있음");
            return false;
        }

        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] == null)
            {
                // 새로 무기 생성
                WeaponController weaponInstance = Instantiate(weaponPrefab, defaultWeaponPosition);
                weaponInstance.transform.localPosition = Vector3.zero;          // transform값 들은 다 부모 값으로 지정
                weaponInstance.transform.localRotation = Quaternion.identity;

                weaponInstance.Owner = gameObject; // 플레이어의 무기로 결정
                weaponInstance.weapon.SourcePrefab = weaponPrefab.gameObject;
                weaponInstance.gameObject.SetActive(true); // 이상 하면 false 하기

                weaponSlots[i] = weaponInstance;

                return true;
            }
        }

        Debug.Log("3개 이상이다");
        return false;
    }

    // 무기 삭제
    public bool RemoveWeapon(WeaponController weaponInstance)
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] == weaponInstance)
            {
                weaponSlots[i] = null;

                Destroy(weaponInstance.gameObject);

                int windex = i <= 0 ? i + 1 : i - 1;
                StartCoroutine(WeaponInChange(windex)); // 일단 이렇게 하고 나중에 바꿀무기를 바로 바꾸어 주는 형식으로 가자
                
                return true;
            }
        }
        return false;
    }

    // 매개변수로 들어온 프리팹으로 생성된 무기가 있으면 생성된 무기를 반환 무기의 값을 반환
    public WeaponController HasWeapon(WeaponController weaponPrefab)
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            var w = weaponSlots[i];
            if(w != null && w.weapon.SourcePrefab == weaponPrefab.gameObject)
            {
                return w;
            }
        }
        return null;
    }
}
