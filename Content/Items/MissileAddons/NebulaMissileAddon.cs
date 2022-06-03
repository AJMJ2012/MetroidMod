using Terraria.ID;
using Terraria.ModLoader;
using MetroidModPorted.Common.GlobalItems;

namespace MetroidModPorted.Content.Items.MissileAddons
{
	public class NebulaMissileAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nebula Missile");
			Tooltip.SetDefault(string.Format("[c/9696FF:Missile Launcher Addon]\n") +
			"Slot Type: Primary\n" +
			"Shots are more powerful and create a larger explosion\n" + 
			"Shots create lingering masses of Nebula energy on impact that continually damage foes\n" + 
			string.Format("[c/78BE78:+400% damage]\n") +
			string.Format("[c/BE7878:-50% speed]"));
		}
		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 14;
			Item.maxStack = 1;
			Item.value = 2500;
			Item.rare = 4;
			/*Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.consumable = true;
			Item.createTile = mod.TileType("NebulaMissileTile");*/
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.missileSlotType = 1;
			mItem.addonDmg = 4f;
			mItem.addonSpeed = -0.5f;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.FragmentNebula, 18)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}
