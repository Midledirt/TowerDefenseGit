using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scrDamageText : MonoBehaviour
{
    public TextMeshProUGUI DamageText => GetComponentInChildren<TextMeshProUGUI>();

    public void ReturnTextToPool()
    {
        transform.SetParent(null);
        ObjectPooler.ReturnToPool(gameObject);
    }
}
