using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private Player player;


    private void Start()
    {
        player = GetComponentInParent<Player>();    
    }


    private void PlayerAnimEvent()
    {
        player.OnMeeleAttackAnimEnd();
    }


    private void RangedAttack()
    {
        player.OnRangedAttakAnim();
    }
}
