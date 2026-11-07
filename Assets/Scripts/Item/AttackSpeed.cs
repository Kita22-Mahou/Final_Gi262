using UnityEngine;

public class AttackSpeed : Item
{
    [SerializeField] private int speedAmout = 1;
    [SerializeField] private float duration = 5f;

    protected override void Use(Player player)
    {
        player.AddSpeed(speedAmout, duration);
    }
}