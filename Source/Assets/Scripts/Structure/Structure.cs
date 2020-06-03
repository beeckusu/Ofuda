using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{

    public delegate void OnDamageTaken(int healthRemaining);
    public OnDamageTaken onDamageTaken;
    public int maxHealth;
    private int health;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            gameObject.SetActive(false);
        onDamageTaken?.Invoke(health);
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
