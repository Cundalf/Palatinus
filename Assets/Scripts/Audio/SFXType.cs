using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXType : MonoBehaviour
{
    public enum SoundType
    {
        SWITCH_OPTION,
        SPELL1,
        SPELL2,
        SPELL3,
        PLAYER_DEATH,
        PLAYER_ATTACK,
        PLAYER_DAMAGE,
        PLAYER_JUMP,
        FOOTSTEP,
        ENEMYDETECT,
        OBJETIVE_COMPLETE,
        WIN,
        LOSE,
        ENEMY_DEATH,
        ENEMY_DAMAGE,
        BLOCK,
        HEAL,
        TARGET
    }

    public SoundType type;
}
