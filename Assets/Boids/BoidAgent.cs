using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;




[RequireComponent(typeof(Collider2D))]
public class BoidAgent : Entity
{
    Boid boidAgent;
    public Boid AgentBoid { get { return boidAgent; } }

    [Header("Movement Settings")]
    public Transform target;

    public float evadeRadius;


    [Space]
    [Header("Hunger Settings")]

    private float baseLookForFoodRadius = 20f;
    public float lookForFoodRadius = 20f;
    private float timeToDeathByHunger = 100f;
    private float hungerMultiplier;


    Rigidbody2D rb;
    Collider2D agentCollider;
    public Collider2D AgentCollider { get { return agentCollider; } }

    public override void Start()
    {
        base.Start();
        agentCollider = GetComponent<Collider2D>();
        changeWeigth(Random.Range(1, 7));
        hungerMultiplier = (Random.Range(0.9f, 3f));
        baseLookForFoodRadius = (Random.Range(1, 10));
        lookForFoodRadius = baseLookForFoodRadius;
        creatureCategory = SelectCreatureType();
        currentAction = CreatureAction.None;
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        rb.mass = weight;
        evadeRadius = weight * 2f;

    }
    CreatureCategory SelectCreatureType()
    {
        var num = Random.Range(0, 100);
        if (num >= 85)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
            return CreatureCategory.Agressive;
        }
        else
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
            return CreatureCategory.Passive;
        }
    }
    public void Initialize(Boid boid)
    {
        boidAgent = boid;

    }
    public void setAction(CreatureAction action)
    {
        this.currentAction = action;
    }
    public void checkAction(Vector2 moveVelocity)
    {
        switch (currentAction)
        {
            case CreatureAction.Exploring:
                if (target != null)
                    target = null;
                Move(moveVelocity);
                break;
            case CreatureAction.Pursuiting:
                Move(moveVelocity);
                Pursuit();
                break;
            case CreatureAction.GoingToFood:
                Move(moveVelocity);
                Pursuit();
                break;

            case CreatureAction.Evading:
                Evade();
                break;
        }
    }

    public void Evade()
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(transform.position, evadeRadius);

        foreach (Collider2D c in contextColliders)
        {
            if (c != this.AgentCollider && c.transform.GetComponent<Entity>().creatureCategory == CreatureCategory.Agressive)
            {
                context.Add(c.transform);

                Vector2 targetDirection = -(c.transform.position - transform.position).normalized;
                transform.up = targetDirection;
                transform.position += (Vector3)targetDirection *currentSpeed * Time.fixedDeltaTime;
                //Debug.DrawLine(agent.transform.position,c.transform.position, Color.blue, 0.5f, false);
            }
        }
    }
    public void Pursuit()
    {
        if (target == null)
            target = FindNewTarget();
        if (target != null)
        {

            Vector2 targetDirection = (target.position - transform.position).normalized;
            float distanceToTarget = Vector2.Distance(transform.position, target.position);
            if (distanceToTarget > lookForFoodRadius)
            {
                target = null;
                return;
            }
            if (distanceToTarget > attackRange)
            {
                // transform.up = targetDirection;
                // transform.position += (Vector3)targetDirection * Time.deltaTime;

            }
            else
            {
                Attack(target.GetComponent<Entity>()); 
            }

            // distanceShow = Vector3.Distance(transform.position, target.position);
            // Debug.DrawRay(transform.position, target.position, Color.yellow);
            // if (Vector3.Distance(transform.position, target.position) < attackRange)
            // {
            //     Debug.Log(transform.name + " is attacking " + target.name);
            //     setAction(CreatureAction.Attacking);
            //     Attack(target.GetComponent<Entity>());
            // }
        }
    }

    public Transform FindNewTarget()
    {
        if (creatureCategory == CreatureCategory.Passive)
        {
            var context = new List<Transform>();
            Collider2D[] contextColliders = Physics2D.OverlapCircleAll(transform.position, lookForFoodRadius);

            foreach (Collider2D c in contextColliders)
            {
                if (c != this.agentCollider && c.gameObject.CompareTag("Food"))
                {
                    context.Add(c.transform);
                }
            }
            if (context.Count == 0)
            {
                target = null;
            }
            else
            {
                int randomIndex = Random.Range(0, context.Count);
                return context[randomIndex];
            }
        }
        else
        {
            var context = new List<Transform>();
            Collider2D[] contextColliders = Physics2D.OverlapCircleAll(transform.position, lookForFoodRadius);

            foreach (Collider2D c in contextColliders)
            {
                if (c != this.agentCollider && !c.gameObject.CompareTag("Player"))
                {
                    context.Add(c.transform);
                }
            }
            if (context.Count == 0)
            {
                target = null;
            }
            else
            {
                int randomIndex = Random.Range(0, context.Count);
                return context[randomIndex];
            }
        }
        return null;
    }
    public float distanceShow = 0;
    public void Move(Vector2 velocity)
    {

        if (target != null)
        {
            if (target.GetComponent<Entity>().wasEaten)
            {
                target = null;
                return;
            }

            Vector2 targetDirection = (target.position - transform.position).normalized;
            transform.up = targetDirection;
            transform.position += (Vector3)targetDirection * Time.fixedDeltaTime;
            Debug.DrawRay(transform.position, targetDirection * 1f, Color.magenta);
        }
        else
        {
            transform.up = velocity;
            transform.position += (Vector3)velocity * Time.fixedDeltaTime;
            // transform.up = velocity;
            // transform.position += (Vector3)velocity * Time.fixedDeltaTime;
            Debug.DrawRay(transform.position, velocity.normalized * 1f, Color.blue);
        }
        spendEnergy();
    }
    public override void changeWeigth(float valor)
    {
        base.changeWeigth(valor);
        transform.localScale = new Vector3(weight * 0.7f / 1.2f, weight * 0.7f);
        attackRange = transform.localScale.y + 1;
        currentSpeed = maxSpeed  / weight;
    }
    public void spendEnergy()
    {
        hunger += Time.deltaTime * 1 * hungerMultiplier / timeToDeathByHunger;
        lookForFoodRadius = Mathf.Clamp(baseLookForFoodRadius + (hunger * 100), 1, weight * 3);


    }
    public override void Eat(Entity deadTarget)
    {
        base.Eat(deadTarget);
        target = null;
        setAction(CreatureAction.Exploring);
    }
    public override void Die(DeathType deathType)
    {
        base.Die(deathType);
        target = null;
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.gray;
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            if (currentAction == CreatureAction.Pursuiting)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(transform.position, lookForFoodRadius);

                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, attackRange);
            }
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, evadeRadius);

        }

    }

    public Transform GetTarget(){
        return target;
    }
}
