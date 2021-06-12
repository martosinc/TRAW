using System;
using System.Collections.Generic;
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
            DisplayName.SetDefault("Молот Астрала");
            Tooltip.SetDefault("Молот благородного палладина с усами и повязкой на глазах");
        }
        public override void SetDefaults() {
			item.damage = 27; 
			item.width = 21;
			item.height = 21; 
			item.useTime = 30; 
			item.useAnimation = 30; 
			item.knockBack = 10; 
			item.value = Item.buyPrice(gold: 1); 
			item.rare = 3; 
            item.channel = true;
			item.UseSound = SoundID.Item1;
			item.crit = 6;
            item.noUseGraphic = true;
            item.autoReuse = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
            item.melee = true;
            item.shoot = ModContent.ProjectileType<AstralHammerProjectile>();
            item.shootSpeed = 5f;
        }		
        public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<MotronBlade>(), 1);
			recipe.AddIngredient(ItemID.SilverBar, 12);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this); 
			recipe.AddRecipe(); 

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<MotronBlade>(), 1);
			recipe.AddIngredient(ItemID.TungstenBar, 12);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this); 
			recipe.AddRecipe(); 
		}

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[item.shoot] < 1;
        }
    }
    public class AstralHammerProjectile : ModProjectile {
        public override void SetDefaults() {
            projectile.Size = new Vector2(78);
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.melee = true;
            projectile.width = 78;
            projectile.height = 78;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
        }
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI) {            
            drawCacheProjsBehindNPCs.Add(index);
        }
        public override void AI()
        {
            float num = 50f;
            float num2 = 2f;
            float quarterPi = -(float)Math.PI / 4f;
            float scaleFactor = 0f;
            Player player = Main.player[projectile.owner];
            Vector2 relativePoint = player.RotatedRelativePoint(player.MountedCenter);
            if (player.dead)
            {
                projectile.Kill();
                return;
            }
            Lighting.AddLight(player.Center, 0.75f, 0.2f, 0.3f);
            
            int sign = Math.Sign(projectile.velocity.X);
            projectile.velocity = new Vector2(sign, 0f);
            if(projectile.ai[0] == 0f)
            {
                projectile.rotation = new Vector2(sign, 0f - player.gravDir).ToRotation() + quarterPi + (float)Math.PI;
                if(projectile.velocity.X < 0f)
                {
                    projectile.rotation -= (float)Math.PI / 2f;
                }
            }

            projectile.ai[0] += 1f;
            projectile.rotation += (float)Math.PI * 2f * num2 / num * (float)sign;
            bool isDone = projectile.ai[0] == (num / 2f);
            if(projectile.ai[0] >= num || (isDone && !player.controlUseItem))
            {
                projectile.Kill();
                player.reuseDelay = 2;
            } else if(isDone) 
            {
                // Get position of cursor
                Vector2 mouseWorld = Main.MouseWorld;
                int dir = (player.DirectionTo(mouseWorld).X > 0f) ? 1 : -1;
                if((float)dir != projectile.velocity.X)
                {
                    player.ChangeDir(dir);
                    projectile.velocity = new Vector2(dir, 0f);
                    projectile.netUpdate = true;
                    projectile.rotation -= (float)Math.PI;
                }
            }

            float rotationValue = projectile.rotation - (float)Math.PI / 4f * (float)sign;
            Vector2 positionVector = (rotationValue + (sign == -1 ? (float)Math.PI : 0f)).ToRotationVector2() * (projectile.ai[0] / num) * scaleFactor;
            Vector2 dustVector1 = projectile.Center + (rotationValue + ((sign == -1) ? ((float)Math.PI) : 0f)).ToRotationVector2() * 30f;
            Vector2 dustPosition = rotationValue.ToRotationVector2();
            Vector2 dustVector2 = dustPosition.RotatedBy((float)Math.PI / 2f * (float)projectile.spriteDirection);
            if(Main.rand.Next(2) == 0)
            {
                Dust dust = Dust.NewDustDirect(dustVector1 - new Vector2(5f), 10, 10, DustID.Fire, player.velocity.X, player.velocity.Y, 150);
                dust.velocity = projectile.DirectionTo(dust.position) * 0.1f + dust.velocity * 0.1f;
            }
            for(int i = 0; i < 4; i++)
            {
                float scaleFactor2 = 1f;
                float scaleFactor3 = 1f;
                switch(i)
                {
                    case 1:
                        scaleFactor3 = -1f;
                        break;
                    case 2:
                        scaleFactor2 = 0.5f;
                        scaleFactor3 = 1.25f;
                        break;
                    case 3:
                        scaleFactor2 = 0.5f;
                        scaleFactor3 = -1.25f;
                        break;
                }
                // Spawns Dust 5/6 times
                if(Main.rand.Next(6) != 0)
                {
                    Dust dust2 = Dust.NewDustDirect(projectile.position, 0, 0, DustID.AmberBolt, 0f, 0f, 100);
                    dust2.position = projectile.Center + dustPosition * (60f + Main.rand.NextFloat() * 20f) * scaleFactor3;
                    dust2.velocity = dustVector2 * (4f + 4f * Main.rand.NextFloat()) * scaleFactor3 * scaleFactor2;
                    dust2.noGravity = true;
                    dust2.noLight = true;
                    dust2.scale = 0.5f;
                    dust2.customData = this;
                    if(Main.rand.Next(4) == 0)
                    {
                        dust2.noGravity = false;
                    }
                }
            }

            projectile.position = relativePoint - projectile.Size / 2f;
            projectile.position += positionVector;
            projectile.spriteDirection = projectile.direction;
            projectile.timeLeft = 2;
            
            player.ChangeDir(projectile.direction);
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = MathHelper.WrapAngle(projectile.rotation);
        }
    }
}