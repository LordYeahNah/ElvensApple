using Godot;
using System;
using System.Diagnostics;

public partial class WeaponController : Node3D
{
	private Node3D mOwner;
	private Weapon mWeaponDetails;
	[Export] private RayCast3D mWeaponCast;						// Reference to the ray cast object
	public bool CanDealDamage = true;							// If the weapon can deal damage

	public string DamageToGroup;

	public void Setup(Weapon weapon, Node3D owner, string group)
	{
		mWeaponDetails = weapon;
		mOwner = owner;
		DamageToGroup = group;
	}

    public override void _Ready()
    {
        base._Ready();
		if(mWeaponCast == null)
			mWeaponCast = GetNodeOrNull<RayCast3D>("RayCast3D");
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
		if(mWeaponCast.IsColliding())
		{
			GodotObject collision = mWeaponCast.GetCollider();
			if(collision != Owner)
			{
				if(collision is BaseCharacter character)
				{
					if(character.IsInGroup(DamageToGroup))
					{
						GD.Print("Hit Enemy");
						character.TakeDamage(mWeaponDetails.GetDamagePoints());
					}
				}
			}
		}
    }
}
