using UnityEngine;

public class BionicModule : Module
{
    [SerializeField] private BionicModuleBehaviour behaviour;
    [SerializeField] private BionicModuleConfig config;

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
            number = config.GetIndex(this),
            level = Level
        };
        return moduleSave;
    }
}
