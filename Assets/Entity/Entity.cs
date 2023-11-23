using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("Entity Settings")]
    //Datatypes
    public CreatureCategory creatureCategory;
    public CreatureAction currentAction;

    //Unit settings
    public float maxHealth;
    public float currentHealth;
    public float weight;

    //Battle Settings
    public bool isDead;
    public bool wasEaten;
    public float damage;
    private float attackFrequency;
    private float timeSinceLastAttack = 0f;
    public float totalAmountToCure = 0; //valor total para curar 
    private float tickTimer = 0; //Tempo atual para curar
    private bool isHealing = false;

    //Movement Settings
    public float currentSpeed;
    public float maxSpeed;

    public virtual void Start()
    {
        isDead=false;
        wasEaten=false;
        attackFrequency = Random.Range(0.5f, 2f) / 1;
        maxSpeed = 2f;
        changeWeigth(Random.Range(0,10));
        
    }


    public virtual bool TakeDamage(float damageValue)
    {
        currentHealth -= maxHealth <= 0 ? 0 : currentHealth;
        if (currentHealth <= 0)
        {
            Die();
            return true;
        }
        return false;
    }
    public virtual void Die()
    {
        isDead = true;

    }

    public virtual void Attack(Entity target)
    {
        if (timeSinceLastAttack >= attackFrequency)
        {
            timeSinceLastAttack = 0;
            if(target.TakeDamage(damage)){
                Eat(target);
            }

        }
    }
    public virtual void Update()
    {
        if (currentAction == CreatureAction.Attacking)
        {
            timeSinceLastAttack += Time.deltaTime;

        }
        if(isDead && wasEaten)
            Destroy(gameObject);

        if(isHealing){
            tickTimer+=Time.deltaTime;
            if(tickTimer >= 0.5f){

            }
        }
    }

    public virtual void Eat(Entity target){
        changeWeigth(target.weight);
        target.wasEaten = true;
        totalAmountToCure=target.weight;
    }
    public virtual void Cure(){
        var valToCure = totalAmountToCure - (maxHealth/10) > 0 ? maxHealth/10: totalAmountToCure;
        currentHealth += valToCure;
    }

    public virtual void changeWeigth(float valor)
    {
        weight += valor;
        currentSpeed = maxSpeed / weight;
        maxHealth+=valor;
    }
}
