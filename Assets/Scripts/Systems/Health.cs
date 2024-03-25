using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    public enum DamageType { Generic, Melee, Projectile }
    [System.Flags]
    public enum EntityType { Player, Enemy, Boss, Object, Other }
    
    public int maxHealth { get; private set; } = 100;
    public int currentHealth { get; private set; } = 100;
    [SerializeField] public EntityType entityType;

    //Messages
    [SerializeField] string onDamageMessage = "OnDamage";
    [SerializeField] string onDepleteMessage = "OnDeplete";
    [SerializeField] string onHealMessage = "OnHeal";
    [SerializeField] string onHealthUpdateMessage = "OnHealthUpdate";


    public void Damage(int amount, DamageType type)
    {
        currentHealth -= amount;

        SendMessage(onDamageMessage);
        SendMessage(onHealthUpdateMessage);
    
        if (currentHealth<=0)
        {
            currentHealth = 0;
            HealthDeplete();
        }
    }
    
    void HealthDeplete()
    {
        SendMessage(onDepleteMessage);
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth >= maxHealth) currentHealth = maxHealth;
        
        SendMessage(onHealMessage);
        SendMessage(onHealthUpdateMessage);

    }

    new void SendMessage(string name) { if (name != null) gameObject.SendMessage(name, SendMessageOptions.DontRequireReceiver); }

}
