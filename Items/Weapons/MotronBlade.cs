using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAW.Items.Weapons
{
    public class MotronBlade : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Клинок Мотрона");
            Tooltip.SetDefault("До нерфа был способен станить целые группы и сообщества игроков.");

        }
        public override void SetDefaults() {
			item.damage = 20; 
			item.width = 32; 
			item.height = 36; 
			item.useTime = 40; 
			item.useAnimation = 20; 
			item.knockBack = 6; 
			item.value = Item.buyPrice(gold: 1); 
			item.rare = 3; 
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
			item.crit = 6;
			item.useStyle = 3;
            item.noUseGraphic = true;
            item.melee = true;
            item.noMelee = true;

            item.shoot = ModContent.ProjectileType<MotronBladeProjectile>();
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            position = Main.MouseWorld + GetVector(MathHelper.ToRadians(Main.rand.Next(360)))*100f;
            var a = position;
            var b = Main.MouseWorld;
            var angle = -(float)Math.Atan2(b.Y-a.Y,b.X-a.X)+MathHelper.ToRadians(90);

            var direction = GetVector(angle);
            direction.Normalize();
            direction *= 15;

            speedX = direction.X;
            speedY = direction.Y;
            return true;
        }
        public Vector2 GetVector(float angle) {
            return new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle));
        }
		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<GussYoyo>(), 1);
			recipe.AddIngredient(ItemID.Vertebrae, 5);
			recipe.AddIngredient(ItemID.Stinger, 2);
			recipe.AddIngredient(ItemID.CrimtaneBar, 10);
			recipe.AddIngredient(ItemID.TissueSample, 10);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this); 
			recipe.AddRecipe(); 

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<GussYoyo>(), 1);
			recipe.AddIngredient(ItemID.RottenChunk, 5);
			recipe.AddIngredient(ItemID.Stinger, 2);
			recipe.AddIngredient(ItemID.DemoniteBar, 10);
			recipe.AddIngredient(ItemID.ShadowScale, 10);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this); 
			recipe.AddRecipe(); 
		}
    }
    public class MotronBladeProjectile : ModProjectile {
        Vector2 startVelocity;
        bool spawned = false;
        public override void SetDefaults() {
            projectile.timeLeft = 40;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.melee = true;
            projectile.width = 64;
            projectile.height = 72;
            projectile.tileCollide = false;
            projectile.alpha = 255;
            projectile.penetrate = -1;
        }

        private int TIMER = 0;
        private int TIMER_MAX = 30;
        private int alphaChange = 10;
        private float inertia = 5f;
        public override void AI() {
            TIMER++;

            if (!spawned) {
                projectile.rotation =
                    projectile.velocity.ToRotation() + 
                    MathHelper.ToRadians(45f); 

                startVelocity = projectile.velocity*2f;
                projectile.velocity = Vector2.Zero;

                spawned = true;
            }
            if (TIMER < TIMER_MAX) {
                if (projectile.alpha > 0) {projectile.alpha -= alphaChange;}
            } else if (TIMER == TIMER_MAX) {
                projectile.velocity = startVelocity;
            } else {
                projectile.velocity = (projectile.velocity * (inertia - 1)) / inertia;
                projectile.alpha += alphaChange * 2;
            }

            if (TIMER == 70) {
                projectile.active = false;
            }
        }
    }
}