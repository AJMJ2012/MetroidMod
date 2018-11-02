using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

using MetroidMod.Items;
using MetroidMod.Projectiles;
using MetroidMod.Projectiles.chargelead;

namespace MetroidMod.Items.weapons
{
	public class MissileLauncher : ModItem
	{
		// Failsaves.
		private Item[] _missileMods;
		public Item[] missileMods
		{
			get
			{
				if (_missileMods == null)
				{
					_missileMods = new Item[MetroidMod.missileSlotAmount];
					for (int i = 0; i < _missileMods.Length; ++i)
					{
						_missileMods[i] = new Item();
						_missileMods[i].TurnToAir();
					}
				}

				return _missileMods;
			}
			set { _missileMods = value; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Missile Launcher");
			Tooltip.SetDefault("Select this item in your hotbar and open your inventory to open the Missile Addon UI");
		}
		public override void SetDefaults()
		{
			item.damage = 30;
			item.ranged = true;
			item.width = 24;
			item.height = 16;
			item.scale = 0.8f;
			item.useTime = 9;
			item.useAnimation = 9;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 5.5f;
			item.value = 20000;
			item.rare = 2;
			item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/MissileSound");
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("MissileShot");
			item.shootSpeed = 8f;
			item.crit = 3;
			
			MGlobalItem mi = item.GetGlobalItem<MGlobalItem>(mod);
			mi.statMissiles = 5;
			mi.maxMissiles = 5;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBar", 10);
			recipe.AddIngredient(null, "EnergyTank", 1);
			recipe.AddIngredient(ItemID.Musket, 1);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBar", 10);
			recipe.AddIngredient(null, "EnergyTank", 1);
			recipe.AddIngredient(ItemID.TheUndertaker, 1);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
		public override void UseStyle(Player P)
		{
			P.itemLocation.X = P.MountedCenter.X - (float)Main.itemTexture[item.type].Width * 0.5f;
			P.itemLocation.Y = P.MountedCenter.Y - (float)Main.itemTexture[item.type].Height * 0.5f;
		}
		
		public override bool CanUseItem(Player player)
		{
			MGlobalItem mi = item.GetGlobalItem<MGlobalItem>(mod);
			if(player.whoAmI == Main.myPlayer && item.type == Main.mouseItem.type)
			{
				return false;
			}
			return (mi.statMissiles > 0);
		}
		
		int finalDmg = 0;
		
		int useTime = 9;
		
		string shot = "MissileShot";
		string chargeShot = "DiffusionMissileShot";
		string shotSound = "MissileSound";
		string chargeShotSound = "SuperMissileSound";
		string chargeUpSound = "ChargeStartup_Power";
		string chargeTex = "ChargeLead_PlasmaRed";
		int dustType = 6;
		Color dustColor = default(Color);
		Color lightColor = MetroidMod.plaRedColor;

		bool isCharge = false;
		bool isSeeker = false;

		public override void UpdateInventory(Player P)
		{
			MGlobalItem mi = item.GetGlobalItem<MGlobalItem>(mod);
			MPlayer mp = P.GetModPlayer<MPlayer>(mod);

			int ic = mod.ItemType("IceMissileAddon");
			int sm = mod.ItemType("SuperMissileAddon");
			int icSm = mod.ItemType("IceSuperMissileAddon");
			int di = mod.ItemType("DiffusionMissileAddon");
			int se = mod.ItemType("SeekerMissileAddon");
			
			Item slot1 = missileMods[0];
			Item slot2 = missileMods[1];
			Item exp = missileMods[2];
			
			int damage = 30;
			useTime = 9;
			shot = "MissileShot";
			chargeShot = "DiffusionMissileShot";
			shotSound = "MissileSound";
			chargeShotSound = "SuperMissileSound";
			chargeUpSound = "ChargeStartup_Power";
			chargeTex = "ChargeLead_PlasmaRed";
			dustType = 6;
			dustColor = default(Color);
			lightColor = MetroidMod.plaRedColor;
			
			isSeeker = (slot1.type == se);
			isCharge = (!slot1.IsAir && !isSeeker);
			
			mi.maxMissiles = 5 + (5*exp.stack);
			if(mi.statMissiles > mi.maxMissiles)
			{
				mi.statMissiles = mi.maxMissiles;
			}

			// Default Combos
			
			if(slot2.type == sm)
			{
				damage = 90;
				useTime = 18;
				shot = "SuperMissileShot";
			}
			else if(slot2.type == ic)
			{
				damage = 45;
				shot = "IceMissileShot";
				chargeShot = "IceDiffusionMissileShot";
				chargeUpSound = "ChargeStartup_Ice";
				chargeTex = "ChargeLead_Ice";
				dustType = 135;
				lightColor = MetroidMod.iceColor;
			}
			else if(slot2.type == icSm)
			{
				damage = 105;
				useTime = 18;
				shot = "IceSuperMissileShot";
				chargeShot = "IceDiffusionMissileShot";
				chargeUpSound = "ChargeStartup_Ice";
				chargeTex = "ChargeLead_Ice";
				dustType = 135;
				lightColor = MetroidMod.iceColor;
			}
			
			
			finalDmg = damage;
			
			item.damage = finalDmg;
			item.useTime = useTime;
			item.useAnimation = useTime;
			item.shoot = mod.ProjectileType(shot);
			item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/"+shotSound);
			if(isCharge || isSeeker)
			{
				item.UseSound = null;
			}
			
			item.autoReuse = (isCharge || isSeeker);

			item.shootSpeed = 8f;
			item.reuseDelay = 0;
			item.mana = 0;
			item.knockBack = 5.5f;
			item.scale = 0.8f;
			item.crit = 3;
			item.value = 20000;
			
			item.rare = 2;
			
			item.Prefix(item.prefix);
		}
		
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if(item == Main.HoverItem)
			{
				item.modItem.UpdateInventory(Main.player[Main.myPlayer]);
			}

			for (int k = 0; k < tooltips.Count; k++)
			{
				if(tooltips[k].Name == "PrefixDamage")
				{
					double num19 = (double)((float)item.damage - (float)finalDmg);
					num19 = num19 / (double)((float)finalDmg) * 100.0;
					num19 = Math.Round(num19);
					if (num19 > 0.0)
					{
						tooltips[k].text = "+" + num19 + Lang.tip[39].Value;
					}
					else
					{
						tooltips[k].text = num19 + Lang.tip[39].Value;
					}
				}
				if(tooltips[k].Name == "PrefixSpeed")
				{
					double num20 = (double)((float)item.useAnimation - (float)useTime);
					num20 = num20 / (double)((float)useTime) * 100.0;
					num20 = Math.Round(num20);
					num20 *= -1.0;
					if (num20 > 0.0)
					{
						tooltips[k].text = "+" + num20 + Lang.tip[40].Value;
					}
					else
					{
						tooltips[k].text = num20 + Lang.tip[40].Value;
					}
				}
			}
		}
		
		public override ModItem Clone(Item item)
		{
			ModItem clone = this.NewInstance(item);
			MissileLauncher missileClone = (MissileLauncher)clone;
			missileClone.missileMods = this.missileMods;

			return clone;
		}
		
		int chargeLead = -1;
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			MGlobalItem mi = item.GetGlobalItem<MGlobalItem>(mod);
			if(isCharge && mi.statMissiles >= 5)
			{
				int ch = Projectile.NewProjectile(position.X,position.Y,speedX,speedY,mod.ProjectileType("ChargeLead"),damage,knockBack,player.whoAmI);
				ChargeLead cl = (ChargeLead)Main.projectile[ch].modProjectile;
				cl.ChargeUpSound = chargeUpSound;
				cl.ChargeTex = chargeTex;
				cl.DustType = dustType;
				cl.DustColor = dustColor;
				cl.LightColor = lightColor;
				cl.ShotSound = shotSound;
				cl.ChargeShotSound = chargeShotSound;
				cl.projectile.netUpdate = true;

				chargeLead = ch;
				return false;
			}
			else if(isSeeker)
			{
				int ch = Projectile.NewProjectile(position.X,position.Y,speedX,speedY,mod.ProjectileType("SeekerMissileLead"),damage,knockBack,player.whoAmI);
				chargeLead = ch;
				return false;
			}
			else
			{
				mi.statMissiles -= 1;
			}
			return true;
		}
		
		int targetingDelay = 0;
		int prevTarget = -2;
		int targetNum = 0;
		public override void HoldItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				MPlayer mp = player.GetModPlayer<MPlayer>(mod);
				MGlobalItem mi = item.GetGlobalItem<MGlobalItem>(mod);

				if (isCharge)
				{
					if (!mp.ballstate && !mp.shineActive && !player.dead && !player.noItems)
					{
						if (player.controlUseItem && chargeLead != -1 && Main.projectile[chargeLead].active && Main.projectile[chargeLead].owner == player.whoAmI && Main.projectile[chargeLead].type == mod.ProjectileType("ChargeLead"))
						{
							if (mp.statCharge < MPlayer.maxCharge)
								mp.statCharge = Math.Min(mp.statCharge + 1, MPlayer.maxCharge);
						}
						else
						{
							Vector2 oPos = player.RotatedRelativePoint(player.MountedCenter, true);

							float MY = Main.mouseY + Main.screenPosition.Y;
							float MX = Main.mouseX + Main.screenPosition.X;
							if (player.gravDir == -1f)
								MY = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;

							float targetrotation = (float)Math.Atan2((MY - oPos.Y), (MX - oPos.X));

							Vector2 velocity = targetrotation.ToRotationVector2() * item.shootSpeed;

							float dmgMult = 1f;//(1f+((float)mp.statCharge*0.02f));
							int damage = (int)((float)item.damage * player.rangedDamage);

							if (mp.statCharge >= MPlayer.maxCharge && mi.statMissiles >= 5)
							{
								int chargeProj = Projectile.NewProjectile(oPos.X, oPos.Y, velocity.X, velocity.Y, mod.ProjectileType(chargeShot), (int)((float)damage * dmgMult), item.knockBack, player.whoAmI);
								mi.statMissiles -= 5;
							}
							else if (mp.statCharge > 0)
							{
								int shotProj = Projectile.NewProjectile(oPos.X, oPos.Y, velocity.X, velocity.Y, mod.ProjectileType(shot), damage, item.knockBack, player.whoAmI);
								mi.statMissiles -= 1;
							}

							if (chargeLead == -1 || !Main.projectile[chargeLead].active || Main.projectile[chargeLead].owner != player.whoAmI || Main.projectile[chargeLead].type != mod.ProjectileType("ChargeLead"))
							{
								mp.statCharge = 0;
							}
						}
					}
					else if (!mp.ballstate)
					{
						mp.statCharge = 0;
					}
				}
				else
				{
					mp.statCharge = 0;
				}

				if (targetingDelay > 0)
				{
					targetingDelay--;
				}

				if (isSeeker && !mp.ballstate && !mp.shineActive && !player.dead && !player.noItems)
				{
					Vector2 oPos = player.RotatedRelativePoint(player.MountedCenter, true);
					float MY = Main.mouseY + Main.screenPosition.Y;
					float MX = Main.mouseX + Main.screenPosition.X;
					Rectangle mouse = new Rectangle((int)MX - 1, (int)MY - 1, 2, 2);
					if (player.gravDir == -1f)
					{
						MY = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
					}
					float targetrotation = (float)Math.Atan2((MY - oPos.Y), (MX - oPos.X));
					Vector2 velocity = targetrotation.ToRotationVector2() * item.shootSpeed;
					int damage = (int)((float)item.damage * player.rangedDamage);
					if (player.controlUseItem && chargeLead != -1 && Main.projectile[chargeLead].active && Main.projectile[chargeLead].owner == player.whoAmI && Main.projectile[chargeLead].type == mod.ProjectileType("SeekerMissileLead"))
					{
						if (mi.seekerCharge < MGlobalItem.seekerMaxCharge)
						{
							mi.seekerCharge = Math.Min(mi.seekerCharge + 1, MGlobalItem.seekerMaxCharge);
						}
						else
						{
							for (int i = 0; i < Main.maxNPCs; i++)
							{
								NPC npc = Main.npc[i];
								bool? flag3 = NPCLoader.CanBeHitByProjectile(npc, Main.projectile[chargeLead]);
								if (npc.active && npc.chaseable && !npc.dontTakeDamage && !npc.friendly && (!flag3.HasValue || flag3.Value))// && (Collision.CanHit(player.Center, 1, 1, npc.position, npc.width, npc.height) || npc.noTileCollide))
								{
									Rectangle npcRect = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
									if (mouse.Intersects(npcRect) && mi.seekerTarget[targetNum] <= -1 && (targetingDelay <= 0 || prevTarget != npc.whoAmI) && mi.statMissiles > mi.numSeekerTargets)
									{
										mi.seekerTarget[targetNum] = npc.whoAmI;
										prevTarget = mi.seekerTarget[targetNum];
										targetNum++;
										if (targetNum > 4)
										{
											targetNum = 0;
										}
										targetingDelay = 40;
										Main.PlaySound(SoundLoader.customSoundType, (int)oPos.X, (int)oPos.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/SeekerLockSound"));
									}
								}
							}

							int num = 10;
							while (mi.seekerTarget[targetNum] > -1 && num > 0)
							{
								targetNum++;
								if (targetNum > 4)
								{
									targetNum = 0;
								}
								num--;
							}

							mi.numSeekerTargets = 0;
							for (int i = 0; i < mi.seekerTarget.Length; i++)
							{
								if (mi.seekerTarget[i] > -1)
								{
									mi.numSeekerTargets++;

									if (!Main.npc[mi.seekerTarget[i]].active)
									{
										mi.seekerTarget[i] = -1;
									}
								}
							}
						}
					}
					else
					{
						if (mi.seekerCharge >= MGlobalItem.seekerMaxCharge && mi.numSeekerTargets > 0)
						{
							for (int i = 0; i < mi.seekerTarget.Length; i++)
							{
								if (mi.seekerTarget[i] > -1)
								{
									int shotProj = Projectile.NewProjectile(oPos.X, oPos.Y, velocity.X, velocity.Y, mod.ProjectileType(shot), damage, item.knockBack, player.whoAmI);
									MProjectile mProj = (MProjectile)Main.projectile[shotProj].modProjectile;
									mProj.seekTarget = mi.seekerTarget[i];
									mProj.seeking = true;

									mi.statMissiles = Math.Max(mi.statMissiles - 1, 0);
								}
							}

							Main.PlaySound(SoundLoader.customSoundType, (int)oPos.X, (int)oPos.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/SeekerMissileSound"));
						}
						else if (mi.seekerCharge > 0)
						{
							int shotProj = Projectile.NewProjectile(oPos.X, oPos.Y, velocity.X, velocity.Y, mod.ProjectileType(shot), damage, item.knockBack, player.whoAmI);
							Main.PlaySound(SoundLoader.customSoundType, (int)oPos.X, (int)oPos.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/" + shotSound));

							mi.statMissiles -= 1;
						}
						if (chargeLead == -1 || !Main.projectile[chargeLead].active || Main.projectile[chargeLead].owner != player.whoAmI || Main.projectile[chargeLead].type != mod.ProjectileType("SeekerMissileLead"))
						{
							mi.seekerCharge = 0;
						}
						mi.numSeekerTargets = 0;
						for (int k = 0; k < mi.seekerTarget.Length; k++)
						{
							mi.seekerTarget[k] = -1;
						}
						targetNum = 0;
						prevTarget = -2;
						targetingDelay = 0;
					}
				}
				else
				{
					mi.seekerCharge = 0;
					mi.numSeekerTargets = 0;
					for (int k = 0; k < mi.seekerTarget.Length; k++)
					{
						mi.seekerTarget[k] = -1;
					}
					targetNum = 0;
					prevTarget = -2;
					targetingDelay = 0;
				}
			}
		}
				
		public override TagCompound Save()
		{
			TagCompound tag = new TagCompound();
			for (int i = 0; i < missileMods.Length; ++i)
				tag.Add("missileItem" + i, ItemIO.Save(missileMods[i]));

			MGlobalItem mi = item.GetGlobalItem<MGlobalItem>(mod);
			tag.Add("statMissiles", mi.statMissiles);
			tag.Add("maxMissiles", mi.maxMissiles);

			return tag;
		}
		public override void Load(TagCompound tag)
		{
			try
			{
				missileMods = new Item[MetroidMod.missileSlotAmount];
				for (int i = 0; i < missileMods.Length; i++)
				{
					Item item = tag.Get<Item>("missileItem" + i);
					missileMods[i] = item;
				}
				
				MGlobalItem mi = this.item.GetGlobalItem<MGlobalItem>(mod);
				mi.statMissiles = tag.GetInt("statMissiles");
				mi.maxMissiles = tag.GetInt("maxMissiles");
			}
			catch{}
		}

		public override void NetSend(BinaryWriter writer)
		{
			for (int i = 0; i < missileMods.Length; ++i)
			{
				writer.WriteItem(missileMods[i]);
			}
			writer.Write(chargeLead);
		}
		public override void NetRecieve(BinaryReader reader)
		{
			for (int i = 0; i < missileMods.Length; ++i)
			{
				missileMods[i] = reader.ReadItem();
			}
			chargeLead = reader.ReadInt32();
		}
	}
}
