using UnityEngine;

public static class GameUtils
{
    public static bool IsInMask(GameObject gameObject, LayerMask layerMask)
    {
        return (layerMask | 1 << gameObject.layer) == layerMask;
    }
}