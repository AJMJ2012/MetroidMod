using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;
using Terraria;
using Terraria.Audio;
using MetroidModPorted.Common.GlobalItems;
using MetroidModPorted.Common.Players;
//using MetroidModPorted.Content.Items;

namespace MetroidModPorted.Content.Buffs
{
    public class MissileRecharge : ModBuff
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Recharging Missiles");
			Description.SetDefault("Using missile station, cant move");
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}
		SoundEffectInstance soundInstance;
		bool soundPlayed = false;
		int num = 0;
		public override void Update(Player player,ref int buffIndex)
        {
            MPlayer mp = player.GetModPlayer<MPlayer>();
            bool flag = false;
            for (int i = 0; i < player.inventory.Length; i++)
            {
                if (player.inventory[i].type == ModContent.ItemType<Items.Weapons.MissileLauncher>())
                {
                    MGlobalItem mi = player.inventory[i].GetGlobalItem<MGlobalItem>();
                    flag = true;
					if(mi.statMissiles < mi.maxMissiles)
					{
						mi.statMissiles++;
						num++;
						int num2 = num;
						while(num2 > 50)
						{
							mi.statMissiles++;
							num2 -= 50;
						}
						break;
					}
                    else //if (mi.statMissiles >= mi.maxMissiles)
                    {
                        //Main.PlaySound(SoundLoader.customSoundType, player.Center, mod.GetSoundSlot(SoundType.Custom, "Sounds/MissilesReplenished"));
                        flag = false;
                    }
                }
            }
            if (!flag || player.controlJump || player.controlUseItem)
            {
				if(soundInstance != null)
				{
					soundInstance.Stop(true);
				}
				soundPlayed = false;
				if(!flag)
				{
					SoundEngine.PlaySound(SoundLoader.CustomSoundType, player.Center, SoundLoader.GetSoundSlot(Mod, "Assets/Sounds/MissilesReplenished"));
				}
				num = 0;
                player.DelBuff(buffIndex);
                buffIndex--;
			}
            else
            {
                player.buffTime[buffIndex] = 2;
                player.controlLeft = false;
                player.controlRight = false;
                player.controlUp = false;
                player.controlDown = false;
                player.controlUseTile = false;
                player.velocity.X *= 0;
                if (player.velocity.Y < 0)
                {
                    player.velocity.Y *= 0;
                }
                player.mount.Dismount(player);
                //Main.PlaySound(10, player.Center);
				if(!soundPlayed)
				{
					soundInstance = SoundEngine.PlaySound(SoundLoader.CustomSoundType, (int)player.Center.X, (int)player.Center.Y, SoundLoader.GetSoundSlot(Mod, "Assets/Sounds/ConcentrationLoop"));
					soundPlayed = true;
				}
            }
        }
    }
}
