using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<WeaponBase> startingWeapons = new List<WeaponBase>();
    public WeaponBase[] weaponSlots = new WeaponBase[2];    // ���� ��ȹ�� �°� 2���� ��� �ٴ� �� �ְ� �س��Ҵ�. ���߿� privet���� ����� ��� Ȯ�ο�
    public Transform defaultWeaponPos;                                  // ���� �����ġ
    public Transform weaponmountPos;                                    // ���� ������ġ
    public static WeaponBase activeWeapon;                        // ��� �ִ� ����

    [SerializeField]
    private float attackModeTime = 5f;  // ���ݸ�� ���ӽð�
    private float attackDelay = 0;      //
    
    private bool attackMode;            // ���ݸ�� ������

    public float layerAtkModeTime;      
    private float countDown;

    private bool isChangeReady = true;         // ���� ��ü��������
    private bool isAttackReady;         // ���� ��������

    private void Start()
    {
        foreach (var w in startingWeapons) 
        {
            AddWeapon(w);
        }
        activeWeapon = weaponSlots[0];
    }

    private void Update()
    {
        AttackMode();
    }

    public void AttackMode()
    {
        PlayerNewInputController.animator.SetLayerWeight(1, layerAtkModeTime);
        PlayerNewInputController.animator.SetBool(AnimString.Instance.attackMode, attackMode);

        attackDelay += Time.deltaTime;
        isAttackReady = attackDelay > activeWeapon.weaponScriptable.rateSpeed;

        if (activeWeapon != null && InputManager.Instance.attackKey && PlayerNewInputController.animator.GetBool(AnimString.Instance.isGround) // ����Ű�� ������ ���� �ְ� ���ⱳü���°� �ƴϰ� �����غ�������
            && isChangeReady && isAttackReady)
        { 
            Attack();                   // ���� ����

            layerAtkModeTime = 1;
            countDown = 0;
            attackDelay = 0;
            attackMode = true;
        }
        else if (countDown > attackModeTime && attackMode)
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

    public bool AddWeapon(WeaponBase weapon)
    {
        if (HasWeapon(weapon) != null)   // �̹� ���Ⱑ ������
        {
            Debug.Log("���� ���� ����");
            return false;
        }

        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] == null)
            {
                // ���� ���� ����
                WeaponBase weaponInstance = Instantiate(weapon, defaultWeaponPos);
                weaponInstance.transform.localPosition = Vector3.zero;
                weaponInstance.transform.localRotation = Quaternion.identity;
                weaponInstance.gameObject.SetActive(true); 

                weaponSlots[i] = weaponInstance;

                return true;
            }
        }
        return false;
    }

    public WeaponBase HasWeapon(WeaponBase weapon)
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            var w = weaponSlots[i];
            if (w != null && w == weapon)
            {
                return w;
            }
        }
        return null;
    }

    private void Attack() // ������ ����
    {
        PlayerNewInputController.animator.SetTrigger(AnimString.Instance.attack);
    }
}
