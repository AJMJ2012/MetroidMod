using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ReLogic.Graphics;
using ReLogic;
using MetroidMod.Items;

namespace MetroidMod {
	public class MetroidMod : Mod
	{
		public static Color powColor = new Color(248, 248, 110);
		public static Color iceColor = new Color(0, 255, 255);
		public static Color waveColor = new Color(215, 0, 215);
		public static Color waveColor2 = new Color(239, 153, 239);
		public static Color plaRedColor = new Color(253, 221, 3);
		public static Color plaGreenColor = new Color(0, 248, 112);
		public static Color plaGreenColor2 = new Color(61, 248, 154);
		public static Color novColor = new Color(50, 255, 1);
		public static Color wideColor = new Color(255, 210, 255);


		internal static ModHotKey MorphBallKey;
		internal static ModHotKey SpiderBallKey;
		internal static ModHotKey BoostBallKey;
		internal static ModHotKey PowerBombKey;
		internal static ModHotKey SenseMoveKey;
		public const string SerrisHead = "MetroidMod/NPCs/Serris/Serris_Head_Head_Boss_";
		public static Mod Instance;
		public MetroidMod()
		{

			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadSounds = true,
				AutoloadGores = true

			};
			
		}
		public override void Load()
		{
			Instance = this;
			MorphBallKey = RegisterHotKey("Morph Ball", "Z");
			SpiderBallKey = RegisterHotKey("Spider Ball", "X");
			BoostBallKey = RegisterHotKey("Boost Ball", "F");
			PowerBombKey = RegisterHotKey("Power Bomb", "R");
			SenseMoveKey = RegisterHotKey("Use Sense Move", "F");
			if (!Main.dedServ)
			{
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Serris"), ItemType("SerrisMusicBox"), TileType("SerrisMusicBox"));
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Kraid"), ItemType("KraidPhantoonMusicBox"), TileType("KraidPhantoonMusicBox"));
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Ridley"), ItemType("RidleyMusicBox"), TileType("RidleyMusicBox"));
			}
			for (int k = 1; k <= 7; k++)
			{
				AddBossHeadTexture(SerrisHead + k);
			}
		}
		static int z = 0;
		float tRot = 0f;
		public override void PostDrawInterface(SpriteBatch sb)
		{
			Mod mod = ModLoader.GetMod(UIParameters.MODNAME);
			Player P = Main.player[Main.myPlayer];
			MPlayer mp = P.GetModPlayer<MPlayer>(mod);
			Item item = P.inventory[P.selectedItem];
			
			if(!Main.playerInventory && Main.npcChatText == "" && P.sign < 0 && !Main.ingameOptionsWindow)
			{
				DrawChargeBar(sb);
				DrawSpaceJumpBar(sb);
			}

			if (P.buffType[0] > 0)
			{
				if(P.buffType[11] > 0)
				{
					z = 100;
				}
				else
				{
					z = 50;
				}
			}
			else
			{
				z = 0;
			}
			
			if(item.type == mod.ItemType("MissileLauncher"))
			{
				MGlobalItem mi = item.GetGlobalItem<MGlobalItem>(mod);
				if(mi.numSeekerTargets > 0)
				{
					tRot += 0.05f;
					for(int i = 0; i < mi.seekerTarget.Length; i++)
					{
						if(mi.seekerTarget[i] > -1)
						{
							NPC npc = Main.npc[mi.seekerTarget[i]];
							Texture2D tTex = mod.GetTexture("Gore/Targeting_retical");
							Color color = new Color(255, 255, 255, 10);
							sb.Draw(tTex, npc.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, tTex.Width, tTex.Height)), color, tRot, new Vector2((float)tTex.Width/2f, (float)tTex.Height/2f), npc.scale*1.5f, SpriteEffects.None, 0f);
						}
					}
				}
			}
		}
		public static int chStyle;
		public static int chR = 255;
		public static int chG = 0;
		public static int chB = 0;
		public void DrawChargeBar(SpriteBatch sb)
		{
			Mod mod = ModLoader.GetMod(UIParameters.MODNAME);
			Player P = Main.player[Main.myPlayer];
			MPlayer mp = P.GetModPlayer<MPlayer>(mod);
			Item item = P.inventory[P.selectedItem];
			if (P.whoAmI == Main.myPlayer && P.active && !P.dead && !P.ghost)
			{
				Texture2D texBar = mod.GetTexture("Gore/ChargeBar"),
					texBarBorder = mod.GetTexture("Gore/ChargeBarBorder"),
					texBarBorder2 = mod.GetTexture("Gore/ChargeBarBorder2");
				if(item.type == mod.ItemType("PowerBeam") || item.type == mod.ItemType("MissileLauncher") || mp.ballstate)
				{
					int ch = (int)mp.statCharge, chMax = (int)MPlayer.maxCharge;
					int pb = (int)mp.statPBCh, pbMax = (int)MPlayer.maxPBCh;
					float x = 22, y = 78+z;
					int times = (int)Math.Ceiling(texBar.Height/2f);
					float chpercent = chMax == 0 ? 0f : 1f*ch/chMax;
					float pbpercent = pbMax == 0 ? 0f : 1f*pb/pbMax;
					int w = (int)(Math.Floor(texBar.Width/2f*chpercent)*2);
					int w2 = (int)(Math.Floor(texBar.Width/2f*pbpercent)*2);
					Color c = chpercent < 1f ? new Color(chR,chG,chB) : Color.Gold;
					Color p = pbpercent < 1f ? Color.Crimson : Color.Gray;
					chStyle = chpercent <= 0f ? 0 : (chpercent <= .5f ? 1 : (chpercent <= .75f ? 2 : (chpercent <= .99f ? 3 : 0)));
					float offsetX = 2, offsetY = 2;
					sb.Draw(texBarBorder2,new Vector2(x,y),new Rectangle(0,0,texBarBorder2.Width,texBarBorder2.Height),Color.White);
					if(pb > 0)
					{
						for (int i = 0; i < times; i++)
						{
							int ww = w2-(i*2);
							if (ww > 0)
							{
								sb.Draw(texBar,new Vector2(x+offsetX,y+offsetY+i*2),new Rectangle(0,i*2,ww,2),p);
							}
						}
					}
					if(ch > 9)
					{
						for (int i = 0; i < times; i++)
						{
							int ww = w-(i*2);
							if (ww > 0)
							{
								sb.Draw(texBar,new Vector2(x+offsetX,y+offsetY+i*2),new Rectangle(0,i*2,ww,2),c);
							}
						}
					}
					if(mp.hyperColors > 0)
					{
						sb.Draw(texBar,new Vector2(x+offsetX,y+offsetY),new Rectangle(0,0,texBar.Width,texBar.Height),new Color(mp.r,mp.g,mp.b));
					}
					sb.Draw(texBarBorder,new Vector2(x,y),new Rectangle(0,0,texBarBorder.Width,texBarBorder.Height),Color.White);

					if(item.type == mod.ItemType("MissileLauncher"))
					{
						MGlobalItem mi = item.GetGlobalItem<MGlobalItem>(mod);
						int num = Math.Min(mi.statMissiles,mi.maxMissiles);
						string text = num.ToString("000");
						Vector2 vect = Main.fontMouseText.MeasureString(text);
						Color color = new Color((int)((byte)((float)Main.mouseTextColor)), (int)((byte)((float)Main.mouseTextColor)), (int)((byte)((float)Main.mouseTextColor)), (int)((byte)((float)Main.mouseTextColor)));
						sb.DrawString(Main.fontMouseText, text, new Vector2(x+38-(vect.X/2), y), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
					}
				}
				if(item.type == mod.ItemType("PowerBeam") || mp.shineDirection != 0 || mp.shineActive)
				{
					Texture2D overheatBar = mod.GetTexture("Gore/OverheatBar"),
					overheatBorder = mod.GetTexture("Gore/OverheatBorder");
					int ovh = (int)mp.statOverheat, ovhMax = (int)mp.maxOverheat;
					float x2 = 22, y2 = 120+z;
					int times2 = (int)Math.Ceiling(overheatBar.Height/2f);
					float ovhpercent = ovhMax == 0 ? 0f : 1f*ovh/ovhMax;
					int wo = (int)(Math.Floor(overheatBar.Width*ovhpercent));
					Color colorheat = new Color((int)((byte)((float)Main.mouseTextColor)),(int)((byte)((float)Main.mouseTextColor*0.25f)),(int)((byte)((float)Main.mouseTextColor*0.1f)),(int)((byte)((float)Main.mouseTextColor)));
					Color o = ovhpercent < 1f ? Color.Gold : colorheat;
					sb.Draw(overheatBorder,new Vector2(x2,y2),new Rectangle(0,0,overheatBorder.Width,overheatBorder.Height),Color.White);
					if(ovh > 0)
					{
						for (int i = 0; i < times2; i++)
						{
							int ww = wo-(i*2);
							if (ww > 0 && ovh <= ovhMax)
							{
								sb.Draw(overheatBar,new Vector2(x2+6,y2+2+i*2),new Rectangle(0,i*2,ww,2),o);
							}
						}
					}
					string text = ovh+"/"+ovhMax;
					Vector2 vect = Main.fontMouseText.MeasureString(text);
					Color color = new Color((int)((byte)((float)Main.mouseTextColor)), (int)((byte)((float)Main.mouseTextColor)), (int)((byte)((float)Main.mouseTextColor)), (int)((byte)((float)Main.mouseTextColor)));
					sb.DrawString(Main.fontMouseText, text, new Vector2(x2+2, y2+overheatBorder.Height+2), color, 0f, default(Vector2), 0.75f, SpriteEffects.None, 0f);
				}
				int num4 = (int)((float)30 % 255);
				if (chStyle == 1)
				{
					chG += num4;
					if (chG >= 255)
					{
						chG = 255;
						chStyle++;
					}
					chR -= num4;
					if (chR <= 0)
					{
						chR = 0;
					}
				}
				else if (chStyle == 2)
				{
					chB += num4;
					if (chB >= 255)
					{
						chB = 255;
						chStyle++;
					}
					chG -= num4;
					if (chG <= 196)
					{
						chG = 196;
					}
				}
				else if (chStyle == 3)
				{
					chR += num4;
					if (chR >= 255)
					{
						chR = 255;
						chStyle = 0;
					}
					chB -= num4;
					if (chB <= 0)
					{
						chB = 0;
					}
					if(chB <= 196)
					{
						chG -= num4;
						if (chG <= 0)
						{
							chG = 0;
						}
					}
				}
				else if (chStyle == 0 || mp.statCharge <= 0)
				{
					chR = 255;
					chG = 0;
					chB = 0;
				}
			}
		}
		public void DrawSpaceJumpBar(SpriteBatch sb)
		{
			Mod mod = ModLoader.GetMod(UIParameters.MODNAME);
			Player P = Main.player[Main.myPlayer];
			MPlayer mp = P.GetModPlayer<MPlayer>(mod);
			if(mp.shineDirection == 0 && mp.spaceJump && mp.spaceJumped && P.velocity.Y != 0 && !mp.ballstate)
			{
				Texture2D texBar = mod.GetTexture("Gore/SpaceJumpBar"), texBarBorder = mod.GetTexture("Gore/SpaceJumpBarBorder");
				if (P.whoAmI == Main.myPlayer && P.active && !P.dead && !P.ghost)
				{
					int sj = (int)mp.statSpaceJumps, sjMax = (int)MPlayer.maxSpaceJumps;
					float x = 160, y = 98+z;
					int times = (int)Math.Ceiling(texBar.Height/2f);
					float sjpercent = sjMax == 0 ? 0f : 1f*sj/sjMax;
					int w = (int)(Math.Floor(texBar.Width/2f*sjpercent)*2);
					Color s = sjpercent < 1f ? Color.Cyan : Color.SkyBlue;
					sb.Draw(texBarBorder,new Vector2(x,y),new Rectangle(0,0,texBarBorder.Width,texBarBorder.Height),Color.White);
					sb.Draw(texBar,new Vector2(x+2,y+2),new Rectangle(0,0,w,texBar.Height),s);
				}
			}
		}
	}
}
