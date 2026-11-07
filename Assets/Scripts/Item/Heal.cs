using UnityEngine;

public class Heal : Item
{
    [SerializeField] private int healAmout = 20;

    protected override void Use(Player player)
    {
        player.Heal(healAmout);
    }
}
