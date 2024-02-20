using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float playerDamage;
    public float playerHp;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        
    }
}
