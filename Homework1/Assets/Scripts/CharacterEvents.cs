using UnityEngine;

public static class CharacterEvents
{
    public static event System.Action<GameObject> OnCharacterDied;
    public static event System.Action<GameObject> OnCharacterRevived;

    public static void BroadcastDeath(GameObject character)
    {
        OnCharacterDied?.Invoke(character);
    }

    public static void BroadcastRevival(GameObject character)
    {
        OnCharacterRevived?.Invoke(character);
    }
}
