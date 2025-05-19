using System;
using System.Collections;
using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    public int currentHealth;
    private StateMachine_Zombie zombie;
    private RagdollActivator ragdoll;

    private void Awake()
    {
        zombie = GetComponent<StateMachine_Zombie>();
        ragdoll = GetComponent<RagdollActivator>();
    }
    
    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            Death();
        }
    }
    

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        zombie.state = ZombieState.Death;
        ragdoll.ActivateRagdoll();
        GameManager.Instance.Addscore();
        StartCoroutine(Wait());


    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
