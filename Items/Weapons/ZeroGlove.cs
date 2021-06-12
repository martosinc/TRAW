using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRAW.Items.Materials;

namespace TRAW.Items.Weapons
{
    public class ZeroGlove : ModItem
    {
        bool spawned = false;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Зеро-перчатка");
            Tooltip.SetDefault("Звоим взглядом высасывает душу из всех противников.");

        }
        public override void SetDefaults() {
			item.damage = 17; 
			item.width = 26; 
			item.height = 26; 
			item.knockBack = 0; 
			item.value = Item.buyPrice(gold: 1, silver:15); 
			item.rare = 3; 
			item.crit = 6;
            item.useTime = 20;
            item.useAnimation = 20;
			item.useStyle = 3;
            item.channel = true;
            item.noUseGraphic = true;
            item.magic = true;
            item.noMelee = true;

            item.shoot = ModContent.ProjectileType<ZeroGloveEye>();
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            position = Main.MouseWorld;
            speedX = 0f;
            speedY = 0f;
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[item.shoot] < 1;
        }
        public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<ZeroEssence>(), 18);
			recipe.AddIngredient(ItemID.Silk, 8);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this); 
			recipe.AddRecipe(); 
		}
    }
    public class ZeroGloveEye : ModProjectile {
        bool scaled = false;
        int pulse = 1;
        int TIMER = 0;
        public override void SetDefaults() {
            projectile.timeLeft = 200;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.melee = false;
            projectile.width = 45;
            projectile.height = 45;
            projectile.alpha = 250;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            TIMER = 10;
        }
        private int ALPHA_CHANGE = 25;
        public override void AI() {
            if (TIMER > 0) {TIMER--;projectile.friendly = false;} else {projectile.friendly = true;}
            if (projectile.alpha > 50) {projectile.alpha -= ALPHA_CHANGE;
            // projectile.scale+=0.075f*2;
            } else {scaled=true;}
            if (scaled) {   
                projectile.alpha += (int)ALPHA_CHANGE/2 * pulse;
                projectile.scale += 0.025f * pulse;
                if (projectile.scale > 1.1f) {
                    pulse = -1;
                } if (projectile.scale < 0.9f) {
                    pulse = 1;
                }
            }
            Player player = Main.player[projectile.owner];

            bool stillInUse = player.channel && !player.noItems && !player.CCed;
            projectile.position = Main.MouseWorld - new Vector2(projectile.width/2,projectile.height/2);

            if (!stillInUse) {
                projectile.active = false;
            }
            projectile.timeLeft = 2;
        }
    }
}