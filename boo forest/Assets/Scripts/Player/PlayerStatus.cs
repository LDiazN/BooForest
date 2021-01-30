using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Instance { get; private set; }
    public bool inLight = false;
    public DeathTrigger death { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Mira maldito, esto es un singleton ." + gameObject.name);
        }
        Instance = this;

        death = GetComponent<DeathTrigger>();
    }
}
