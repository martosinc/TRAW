using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAW.Items.Materials
{
    public class ZeroEssence : ModItem
    {
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Эссенция Зеро");
			Tooltip.SetDefault("Переполненна злобой души Зеро");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 5));
			// ItemID.Sets.AnimatesAsSoul[item.type] = true;
			ItemID.Sets.ItemIconPulse[item.type] = true;
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}
        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 26;
            item.value = 30;
            item.rare = 0;
            item.maxStack = 99;
        }
		public override void GrabRange(Player player, ref int grabRange) {
			grabRange *= 2;
		}

		public override bool GrabStyle(Player player) {
			Vector2 vectorItemToPlayer = player.Center - item.Center;
			Vector2 movement = -vectorItemToPlayer.SafeNormalize(default(Vector2)) * 0.1f;
			item.velocity = item.velocity + movement;
			item.velocity = Collision.TileCollision(item.position, item.velocity, item.width, item.height);
			return true;
		}
    }
}
