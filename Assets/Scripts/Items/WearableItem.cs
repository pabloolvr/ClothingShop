using UnityEngine;

public enum WearableSlot
{
    Hair,
    Head,
    Chest,
    Legs,
    Feet,
    Hands,
    Ring,
    Neck,
    Back,
}

public enum WearableLayer
{
    Under,
    Body,
    Sock,
    Foot1,
    Lower1,
    Shirt1,
    Lower2,
    Foot2,
    Lower3,
    Hand,
    Outerwear,
    Neckwear,
    Facewear,
    Hair,
    Headwear,
    Over
}

[CreateAssetMenu(fileName = "Wearable Item Data", menuName = "Item System/Wearable")]
public class WearableItem : ItemData
{
    public WearableSlot Slot => _slot;
    public WearableLayer Layer => _layer;
    public GameObject ModelPrefab => _modelPrefab;
    public Sprite[] IdleUpSprites => _idleUpSprites;
    public Sprite[] IdleDownSprites => _idleDownSprites;
    public Sprite[] IdleLeftSprites => _idleLeftSprites;
    public Sprite[] IdleRightSprites => _idleRightSprites;
    public Sprite[] WalkUpSprites => _walkUpSprites;
    public Sprite[] WalkDownSprites => _walkDownSprites;
    public Sprite[] WalkLeftSprites => _walkLeftSprites;
    public Sprite[] WalkRightSprites => _walkRightSprites;

    [Header("Wearable Data")]
    [SerializeField] private WearableSlot _slot;
    [SerializeField] private WearableLayer _layer;
    [SerializeField] private GameObject _modelPrefab;

    [Header("Idle Animation Sprites")]
    [SerializeField] private Sprite[] _idleUpSprites = new Sprite[3];
    [SerializeField] private Sprite[] _idleDownSprites = new Sprite[3];
    [SerializeField] private Sprite[] _idleLeftSprites = new Sprite[3];
    [SerializeField] private Sprite[] _idleRightSprites = new Sprite[3];

    [Header("Walk Animation Sprites")]
    [SerializeField] private Sprite[] _walkUpSprites = new Sprite[5];
    [SerializeField] private Sprite[] _walkDownSprites = new Sprite[5];
    [SerializeField] private Sprite[] _walkLeftSprites = new Sprite[5];
    [SerializeField] private Sprite[] _walkRightSprites = new Sprite[5];
}
