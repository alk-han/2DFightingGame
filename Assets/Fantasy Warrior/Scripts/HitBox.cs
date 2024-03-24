using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    private GameObject enemy;
    private Player player;

    private void Start()
    {
        player = GetComponentInParent<Player>();
        enemy = player.enemy;
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(enemy.tag)) 
        {
            player.isEnemyNear = true;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(enemy.tag))
        {
            player.isEnemyNear = false;
        }
    }
}
