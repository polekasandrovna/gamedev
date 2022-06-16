using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDiller : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<HunterScript>() == null && other.tag != "Wall")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
