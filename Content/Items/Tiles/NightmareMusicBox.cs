using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Items.Tiles
{
	public class NightmareMusicBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Music Box (Vs. Nightmare)");
			Tooltip.SetDefault("Original composition by Minako Hamano and Akira Fujiwara");
		}
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.NightmareMusicBox>();
			Item.width = 24;
			Item.height = 32;
			Item.rare = ItemRarityID.Yellow;
			Item.value = 1000;
			Item.accessory = true;
		}
	}
}
