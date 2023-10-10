using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    public GameObject[] bodyParts;

    public Material baseMaterial;
    public Material spwanMaterial;
    public Material dissolveMaterial;
    private Material activeMaterial;

    [Range(-1, 1)] // ���°� ������ �Ⱥ��� ������ ��
    public float minValue;
    [Range(-1, 1)] // ���°� ������ ���� ������ ��
    public float maxValue;
    [Range(-1, 1)] // ���� ���°� ��
    public float getValue;
    public float speed;

    private CharacterStats characterStats;
    private bool isSpwan;
    private bool isDissolve;

    private List<Material> dissolveMaterials = new List<Material>(); // activeMaterial�� �̿��ؼ� �Ҷ��ߴ��� �ణ�� ���װ� �߻��Ͽ� ������ �߰��Ͽ� �ذ��ߴ�.

    void Start()
    {
        characterStats = GetComponent<CharacterStats>();
        isSpwan = true;

        foreach (var part in bodyParts) 
        {
            var renderer = part.GetComponent<SkinnedMeshRenderer>();
            renderer.material = new Material(spwanMaterial);

            activeMaterial= renderer.material;
        }
        getValue = minValue;
    }
    void Update()
    {
        if (isSpwan)
        {
            SpawnObj();
        }

        if (characterStats.isDeath) // ������ ������ ��Ʈ����� �����Ͽ� ������
        {
            DissolveObj();
        }
    }

    public void SpawnObj()
    {
        getValue += speed * Time.deltaTime;

        if(getValue < maxValue) // ���� ���� maxValue ���� ������
        {
            activeMaterial.SetFloat("_Value", getValue);
        }
        else
        {
            activeMaterial.SetFloat("_Value", maxValue);

            foreach (var part in bodyParts)
            {
                var renderer = part.GetComponent<SkinnedMeshRenderer>();
                renderer.material = baseMaterial;

                activeMaterial = renderer.material;
            }

            isSpwan = false;
        }
    }

    private void DissolveObj() // �̰� ���� ���� ���̶� Į ���д� �ȵȴ� ��Ʈ������ ���� ������ �ʿ��� ����
    {
        if(isDissolve == false)
        {
            
            foreach (var part in bodyParts)
            {
                var renderer = part.GetComponent<SkinnedMeshRenderer>();
                renderer.material = new Material(dissolveMaterial);

                dissolveMaterials.Add(renderer.material);
            }
            isDissolve = true;
        }
        
        getValue -= speed * Time.deltaTime;

        if(getValue > minValue) // ���� ����  minValue ���� ũ��
        {
            foreach (var material in dissolveMaterials)
            {
                material.SetFloat("_Value", getValue);
            }
        }
        else
        {
            foreach (var material in dissolveMaterials)
            {
                material.SetFloat("_Value", minValue);
            }
        }
    }
}
