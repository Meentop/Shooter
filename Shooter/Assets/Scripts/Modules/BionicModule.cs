using UnityEngine;

public class BionicModule : Module
{
    [SerializeField] private BionicModuleBehaviour behaviour;

    public PlayerCharacteristics ApplyBehaviour(PlayerCharacteristics movement, InfoForBionicModule info)
    {
        return behaviour.ApplyBehaviour(movement, info);
    }

    public override string GetDescription(int lvl)
    {
        return behaviour.GetDescription(lvl);
    }

    public BionicModuleSave GetSave()
    {
        BionicModuleSave moduleSave = new BionicModuleSave
        {
            number = number,
            level = Level
        };
        return moduleSave;
    }
}
