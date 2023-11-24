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
    public float hunger;

    //Battle Settings
    public float damage;
    public float attackRange = 1f;
    public float attackFrequency;
    public float timeSinceLastAttack = 0f;
    public float totalAmountToCure = 0; //valor total para curar 
    private float tickHealTimer = 0; //Tempo atual para curar
    public float tickDamageTimer = 0;
    public bool isDead;
    public bool wasEaten;

    //Movement Settings
    public float currentSpeed;
    public float maxSpeed;

    public virtual void Start()
    {
        isDead = false;
        wasEaten = false;
        attackFrequency = 1f;
        maxHealth = Random.Range(1, 10);
        maxSpeed = 2f;
        damage = Random.Range(1, 4);
        changeWeigth(Random.Range(0, 10));
    }


    public virtual bool TakeDamage(float damageValue)
    {

        if (tickDamageTimer > 0.8f)
        {
            currentHealth -= damageValue;
            tickDamageTimer = 0;
            if (currentHealth <= 0)
            {
                Die(DeathType.Fighting);
                return true;
            }

        }
        return false;

    }
    public virtual void Die(DeathType deathType)
    {
         
        isDead = true;

    }

    public virtual void Attack(Entity target)
    {
        if (timeSinceLastAttack >= attackFrequency)
        {
            timeSinceLastAttack = 0;
            if (target.TakeDamage(damage) || target.isDead)
            {
                Eat(target);
            }

        }
    }
    public virtual void Update()
    {
        tickDamageTimer += Time.deltaTime;
        if (creatureCategory != CreatureCategory.Food)
        {

            if (hunger > 1)
            {
                if (TakeDamage(1)) Die(DeathType.Hungry);
            }
            if (totalAmountToCure > 0)
            {
                tickHealTimer += Time.deltaTime;
                if (tickHealTimer > 0.5f)
                {
                    Cure();
                }
            }
            if (currentAction == CreatureAction.Pursuiting)
            {
                timeSinceLastAttack += Time.deltaTime;

            }
            if (isDead && wasEaten && creatureCategory != CreatureCategory.Food)
                gameObject.SetActive(false);
            else if (isDead && wasEaten && creatureCategory == CreatureCategory.Food)
                Destroy(gameObject);

        }
    }

    public virtual void Eat(Entity deadTarget)
    {
        changeWeigth(deadTarget.weight); 
        totalAmountToCure = deadTarget.weight;
        hunger -= deadTarget.weight / 10;
        currentHealth = Mathf.Min(currentHealth += deadTarget.weight / 2, currentHealth, maxHealth);
        Debug.Log("EATED");
        deadTarget.wasEaten = true;
    }
    public virtual void Cure()
    {
        tickHealTimer = 0;
        var valToCure = totalAmountToCure - (maxHealth / 10) > 0 ? maxHealth / 10 : totalAmountToCure;
        currentHealth += totalAmountToCure / 10;

        var cureValor = maxHealth / 10;
        totalAmountToCure = totalAmountToCure - cureValor > 0 ? totalAmountToCure - cureValor : 0;

        currentHealth = currentHealth + valToCure > maxHealth ? maxHealth : currentHealth + valToCure;
    }

    public virtual void changeWeigth(float valor)
    {
        weight += valor;
        currentSpeed = maxSpeed / weight;
        maxHealth += valor;
        damage += Random.Range(0.2f, 2) * weight;
    }



}
