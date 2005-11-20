// $Id$

using System;

namespace PlayOnline.FFXI {

  public enum ItemDataType {
    Armor,
    Object,
    Weapon
  }

  public enum ItemDataLanguage {
    English,
    Japanese
  }

  public enum ItemField {
    Any,
    All,
    // Common
    ID,
    Flags,
    StackSize,
    Type,
    MysteryField, // Need a good name for this...
    ValidTargets,
    EnglishName,
    JapaneseName,
    LogNameSingular,
    LogNamePlural,
    Description,
    // Furniture
    Element,
    Storage,
    // Armor & Weapons
    Level,
    Slots,
    Races,
    Jobs,
    MaxCharges,
    CastTime,
    EquipDelay,
    ReuseTimer,
    // Armor Only
    ShieldSize,
    // Weapon Only
    Damage,
    DPS,
    Delay,
    Skill,
    JugSize, // BST Jugs only; Not sure if this is the correct interpretation...
  }

  public enum Element : byte {
    Fire,
    Ice,
    Wind,
    Earth,
    Thunder,
    Water,
    Light,
    Dark,
    Special   = 15, // this is the element set on the Meteor spell
    Undecided = 255 // this is the element set on inactive furnishing items in the item data
  }

  [Flags]
  public enum EquipmentSlot : ushort {
    // Slot Groups
    None   = 0x0000,
    Ears   = 0x3000,
    Rings  = 0x6000,
    All    = 0xFFFF,
    // Specific Slots
    Main   = 0x0001,
    Sub    = 0x0002,
    Range  = 0x0004,
    Ammo   = 0x0008,
    Head   = 0x0010,
    Body   = 0x0020,
    Hands  = 0x0040,
    Legs   = 0x0080,
    Feet   = 0x0100,
    Neck   = 0x0200,
    Waist  = 0x0400,
    LEar   = 0x0800,
    REar   = 0x1000,
    LRing  = 0x2000,
    RRing  = 0x4000,
    Back   = 0x8000,
  }

  [Flags]
  public enum ItemFlags : ushort {
    None       = 0x0000,
    // Simple Flags - mostly assumed meanings
    Flag00      = 0x0001,
    Flag01      = 0x0002,
    Flag02      = 0x0004,
    Flag03      = 0x0008,
    Flag04      = 0x0010,
    Inscribable = 0x0020,
    NoAuction   = 0x0040,
    Scroll      = 0x0080,
    Linkshell   = 0x0100,
    CanUse      = 0x0200,
    CanTradeNPC = 0x0400,
    CanEquip    = 0x0800,
    NoSale      = 0x1000,
    NoDelivery  = 0x2000,
    NoTradePC   = 0x4000,
    Rare        = 0x8000,
    // Combined Flags
    Ex          = 0x6040, // NoAuction + NoDelivery + NoTrade
  }

  public enum ItemType : ushort {
    Nothing    = 0x0000,
    Item       = 0x0001,
    QuestItem  = 0x0002,
    Fish       = 0x0003,
    Weapon     = 0x0004,
    Armor      = 0x0005,
    Linkshell  = 0x0006,
    UsableItem = 0x0007,
    Crystal    = 0x0008,
    Unknown    = 0x0009,
    Furnishing = 0x000A,
    Plant      = 0x000B,
    Flowerpot  = 0x000C,
    Material   = 0x000D,
    Mannequin  = 0x000E,
    Book       = 0x000F
  }

  [Flags]
  public enum Job : ushort {
    None = 0x0000,
    All  = 0xFFFE,
    // Specific
    WAR  = 0x0002,
    MNK  = 0x0004,
    WHM  = 0x0008,
    BLM  = 0x0010,
    RDM  = 0x0020,
    THF  = 0x0040,
    PLD  = 0x0080,
    DRK  = 0x0100,
    BST  = 0x0200,
    BRD  = 0x0400,
    RNG  = 0x0800,
    SAM  = 0x1000,
    NIN  = 0x2000,
    DRG  = 0x4000,
    SMN  = 0x8000,
  }

  public enum MagicType : byte {
    None,
    WhiteMagic,
    BlackMagic,
    SummonerPact,
    Ninjutsu,
    BardSong
  }

  [Flags]
  public enum Race : ushort {
    None           = 0x0000,
    All            = 0x01FE,
    // Specific
    HumeMale       = 0x0002,
    HumeFemale     = 0x0004,
    ElvaanMale     = 0x0008,
    ElvaanFemale   = 0x0010,
    TarutaruMale   = 0x0020,
    TarutaruFemale = 0x0040,
    Mithra         = 0x0080,
    Galka          = 0x0100,
    // Race Groups
    Hume           = 0x0006,
    Elvaan         = 0x0018,
    Tarutaru       = 0x0060,
    // Gender Groups (with Mithra = female, and Galka = male)
    Male           = 0x012A,
    Female         = 0x00D4,
  }

  public enum Skill : byte {
    None             = 0x00,
    HandToHand       = 0x01,
    Dagger           = 0x02,
    Sword            = 0x03,
    GreatSword       = 0x04,
    Axe              = 0x05,
    GreatAxe         = 0x06,
    Scythe           = 0x07,
    PoleArm          = 0x08,
    Katana           = 0x09,
    GreatKatana      = 0x0a,
    Club             = 0x0b,
    Staff            = 0x0c,
    Ranged           = 0x19,
    Marksmanship     = 0x1a,
    Thrown           = 0x1b,
    DivineMagic      = 0x20,
    HealingMagic     = 0x21,
    EnhancingMagic   = 0x22,
    EnfeeblingMagic  = 0x23,
    ElementalMagic   = 0x24,
    DarkMagic        = 0x25,
    SummoningMagic   = 0x26,
    Ninjutsu         = 0x27,
    Singing          = 0x28,
    StringInstrument = 0x29,
    WindInstrument   = 0x2a,
    Fishing          = 0x30,
  }

  [Flags]
  public enum ValidTarget : ushort {
    None        = 0x00,
    Self        = 0x01,
    Player      = 0x02,
    PartyMember = 0x04,
    Ally        = 0x08,
    NPC         = 0x10,
    Enemy       = 0x20,
    Flag40      = 0x40,
    Flag80      = 0x80,
    Corpse      = 0x9D
  }

}
