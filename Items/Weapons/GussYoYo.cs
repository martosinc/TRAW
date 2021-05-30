using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAW.Items.Weapons
{
    public class GussYoyo : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Гусс-Йойо");
        }
		public override void SetDefaults() {
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.width = 27;
			item.height = 27;
			item.useAnimation = 25;
			item.useTime = 25;
			item.shootSpeed = 16f;
			item.knockBack = 2.5f;
			item.damage = 6;
			item.rare = ItemRarityID.White;

			item.melee = true;
			item.channel = true;
			item.noMelee = true;
			item.noUseGraphic = true;

			item.UseSound = SoundID.Item1;
			item.value = Item.sellPrice(silver: 56);
			item.shoot = ModContent.ProjectileType<GussYoyoProjectile>();
		}
    }
public class GussYoyoProjectile : ModProjectile
	{
		public override void SetStaticDefaults() {
			ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 3.5f;
			ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 300f;
			ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 13f;
		}

		public override void SetDefaults() {
			projectile.extraUpdates = 0;
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = 99;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.melee = true;
			projectile.scale = 1f;
		}
		public override void PostAI() {
			if (Main.rand.NextBool()) {
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 16);
				dust.noGravity = true;
				dust.scale = 1.6f;
			}
		}
	}
}