using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAW.Items.Weapons
{
    public class AstralHammer : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Клинок Мотрона");
        }
        public override void SetDefaults() {
			item.damage = 20; 
			item.width = 40; 
			item.height = 30; 
			item.useTime = 30; 
			item.useAnimation = 40; 
			item.knockBack = 6; 
			item.value = Item.buyPrice(gold: 1); 
			item.rare = 3; 
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.crit = 6;
			item.useStyle = 1;
            item.melee = true;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox) {
            if ((float)player.itemRotation * player.direction > 1.3f) {
                var quakePos = player.Center;
                quakePos.Y += (int)(player.height/2);
                Projectile.NewProjectile(quakePos.X, quakePos.Y,0f,0f,ModContent.ProjectileType<AstralProjectile>(),20,5f);
            }
        }
    }
    public class AstralProjectile : ModProjectile {
        public override void SetDefaults() {
            projectile.timeLeft = 10;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.melee = true;
            projectile.width = 32;
            projectile.height = 32;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.scale = 4f;
            projectile.hide = true; 
        }
        // public override void AI() {
        // }
    }
}