using System.Collections.Generic;

public static class ActionExtention
{
    public static void ExecuteAll<TAction>(this ICollection<TAction> collection, params ActionParameter[] parameters) where TAction : ActionBase
    {
        if (collection == null)
            return;

        foreach (var action in collection)
            action.ExecuteAction(parameters);
    }
}
