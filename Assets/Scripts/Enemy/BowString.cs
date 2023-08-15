using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowString : MonoBehaviour
{
    public Vector3 startTransform;
    public Transform rightHand;
    public bool readyAim;
    private void Update()
    {
        if (readyAim)
        {
            transform.position = rightHand.position;
            transform.rotation = rightHand.rotation;
        }
        else
        {
            transform.localPosition = startTransform;
            transform.rotation = Quaternion.identity;
        }
    }
}
