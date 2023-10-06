using System;
using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public partial class ItemDatabase : Node3D
{
    private const string WEAPON_PATH = "res://Database/Weapons.json";
    private const string ARMOR_PATH = "res://Database/Armor.json";
    private List<BaseItem> mWeapons = new List<BaseItem>();
    private List<BaseItem> mArmor = new List<BaseItem>();

    public override void _Ready()
    {
        base._Ready();
        LoadWeapons();
        LoadArmor();
    }

    public BaseItem GetRandomWeapon()
    {
        RandomNumberGenerator rand = new RandomNumberGenerator();
        return mWeapons[rand.RandiRange(0, mWeapons.Count - 1)];
    }

    public BaseItem GetRandomArmor()
    {
        RandomNumberGenerator rand = new RandomNumberGenerator();
        rand.Randomize();
        return mArmor[rand.RandiRange(0, mArmor.Count - 1)];
    }

    private void LoadArmor()
    {
        var file = FileAccess.Open(ARMOR_PATH, FileAccess.ModeFlags.Read);
        if(file != null && file.IsOpen())
        {
            string text = file.GetAsText();
            var objects = JArray.Parse(text);

            foreach(var o in objects)
            {
                JToken token = o;
                ArmorData data = JsonConvert.DeserializeObject<ArmorData>(token.ToString());
                mArmor.Add(CreateArmor(data));
            }
        }
    }

    private void LoadWeapons()
    {
        var file = FileAccess.Open(WEAPON_PATH, FileAccess.ModeFlags.Read);             // Open the file

        if(file != null && file.IsOpen())
        {
            string text = file.GetAsText();                 // get the contents as text
            var objects = JArray.Parse(text);

            foreach(var o in objects)
            {
                JToken token = o;
                WeaponData data = JsonConvert.DeserializeObject<WeaponData>(token.ToString());
                mWeapons.Add(CreateWeapon(data));
                file.Close();
            }
        } 

    }

    private Weapon CreateWeapon(WeaponData data)
    {
        Texture2D icon = GD.Load<Texture2D>(data.PathToIcon);
        PackedScene mesh = GD.Load<PackedScene>(data.PathToItem);
        return new Weapon(data.WeaponName, data.Description, data.Cost, data.DamagePoints, data.CriticalHitChance, icon, mesh);
    }

    private Armor CreateArmor(ArmorData data)
    {
        Texture2D icon = GD.Load<Texture2D>(data.PathToIcon);
        return new Armor(data.ArmorName, false, data.Description, data.Cost, icon, null, data.ReductionPercentage);
    }
}

public class WeaponData
{
    [JsonProperty] public string WeaponName;
    [JsonProperty] public string Description;
    [JsonProperty] public int Cost;
    [JsonProperty] public float DamagePoints;
    [JsonProperty] public float CriticalHitChance;
    [JsonProperty] public string PathToIcon;
    [JsonProperty] public string PathToItem;
}

public class ArmorData
{
    [JsonProperty] public string ArmorName;
    [JsonProperty] public string Description;
    [JsonProperty] public int Cost;
    [JsonProperty] public float ReductionPercentage;
    [JsonProperty] public string PathToIcon;
}