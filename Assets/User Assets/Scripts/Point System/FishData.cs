using Unity.VisualScripting;
using UnityEngine;

// on the prefab; pass coresponding SO
public class FishData : MonoBehaviour
{
    [SerializeField] private string reference;
    [SerializeField] private float speed;
    [SerializeField] private float rarity;

    public string Reference { get => reference; }
    public float Speed { get => speed; }
    public float Rarity { get => rarity; }


}
