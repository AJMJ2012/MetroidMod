﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using MetroidModPorted.Common.Players;

namespace MetroidModPorted
{
	public abstract class ModPrimaryABeam : ModBeam
	{
		public override sealed float AddonChargeDamage { get; set; } = 0f;
		public override sealed float AddonChargeHeat { get; set; } = 0f;

		public virtual int ShotAmount { get; set; } = 3;

		public override sealed void Load()
		{
			base.Load();
			AddonSlot = BeamAddonSlotID.PrimaryA;
		}

		public sealed override bool OnShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			return base.OnShoot(player, source, position, velocity, type, damage, knockback);
		}
	}
}
