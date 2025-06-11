using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Points : MonoBehaviour
{
    public static Action<int> PointCollected;
    [SerializeField] private int _valuePoints;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collectable();
        }
    }

    private void Collectable()
    {
        PointCollected?.Invoke(_valuePoints);
        Destroy(gameObject);
    }
}