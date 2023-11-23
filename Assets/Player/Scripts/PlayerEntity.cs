using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : Entity
{
    public override void Start()
    {
        base.Start();
        maxHealth = 10;
        currentHealth = 10;
        damage = 1;
    }
    public override void Update()
    {
        base.Update();
    }

    public override void Die()
    {
        base.Die();
    }
    public override bool TakeDamage(float damageValue)
    {
        return base.TakeDamage(damageValue);
    }
    public override void Attack(Entity target)
    {
        base.Attack(target);
    }
    public override void Cure()
    {
        base.Cure();
    }
    public override void Eat(Entity target)
    {
        base.Eat(target);

    }

}
