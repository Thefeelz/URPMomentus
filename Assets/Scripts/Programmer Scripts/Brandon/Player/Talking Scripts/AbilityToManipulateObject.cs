using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AbilityToManipulate { groundSlide, wallRun, movement, input, airDash, bladeDance, mouselook, swordThrow, playerAttack, swordSlash, containedHeat, shield };

[CreateAssetMenu(fileName = "AbilityToggle", menuName = "MessageCreator/CreateAbilityToggle", order = 2)]
public class AbilityToManipulateObject : ScriptableObject
{ 
    
    public AbilityToManipulate abilitiesToManipulate;
    public bool turnOnAbility;
}
