using UnityEngine;

public class FishData : MonoBehaviour
{
    public string reference;
    public float speed; // used when scaring away the fish

    [Tooltip("Base score is multiplied by species rarity:\nCommon (6 species): 1.0x\nUncommon (4 species): 1.2x\nRare (3 species): 1.5x\nExtraordinary (2 species): 2.0x ")]
    public Rarity rarity;

    public enum Rarity
    {
        Common = 10,
        Uncommon = 12,
        Rare = 15,
        Extraordinary = 20
    }

}