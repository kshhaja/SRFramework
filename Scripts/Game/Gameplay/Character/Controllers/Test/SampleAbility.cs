using SlimeRPG.Gameplay.Character;
using SlimeRPG.Gameplay.Character.Ability;
using UnityEngine;


public class SampleAbility : MonoBehaviour
{
    public AbilityBase primary;
    public AbilityBase secondary;
    public AbilityBase tertiary;

    void Start()
    {
        var abc = GetComponent<PlayerCharacter>();

        if (primary)
            abc.GrantAbility(PlayerCharacter.ActionHandler.primary, primary);

        if (secondary)
            abc.GrantAbility(PlayerCharacter.ActionHandler.secondary, secondary);
        
        if (tertiary)
            abc.GrantAbility(PlayerCharacter.ActionHandler.tertiary, tertiary);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
