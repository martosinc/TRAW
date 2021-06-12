using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System;
using Terraria.ModLoader;

namespace TRAW.Items.Weapons
{
    public class MartosPearl : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Жемчужина Мартоса");
            Tooltip.SetDefault("Unfinished");
        }
        public override void SetDefaults() {
			item.damage = 33;
			item.knockBack = 3f;
			item.mana = 8;
			item.width = 32;
			item.height = 32;
			item.useTime = 45;
			item.useAnimation = 45;
            item.autoReuse = true;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = Item.buyPrice(0, 30, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.UseSound = SoundID.Item44;

            item.magic = true;
            item.noMelee = true;
            item.shoot = ModContent.ProjectileType<MartosPearlLeftProjectile>();
            item.shootSpeed=10f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            var center = player.Center;

            var a = center;
            var b = Main.MouseWorld;
            var angle = -(float)Math.Atan2(b.Y-a.Y,b.X-a.X)+MathHelper.ToRadians(90);

            var direction = GetVector(angle);
            direction.Normalize();

            speedX = direction.X;
            speedY = direction.Y;

            Projectile.NewProjectile(center.X,center.Y-20f,speedX,speedY, ModContent.ProjectileType<MartosPearlLeftProjectile>(), item.damage, item.knockBack, player.whoAmI);
            Projectile.NewProjectile(center.X,center.Y+20f,-1f*speedX,-1f*speedY, ModContent.ProjectileType<MartosPearlRightProjectile>(),item.damage,item.knockBack, player.whoAmI);

            return false;
        }
        public Vector2 GetVector(float angle) {
            return new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle));
        }
    }
    public class MartosPearlLeftProjectile : ModProjectile {
        private Vector2 center;
        Vector2 dir;
        bool spawned = false;
        public override void SetDefaults() {
            projectile.friendly = true;
            projectile.hostile = false;
            // projectile.melee = true;
            // projectile.ranged = true;
            projectile.magic = true;
            projectile.width = 22;
            projectile.height = 22;
            projectile.tileCollide = true;
            projectile.penetrate = 1;
        }
        public override void AI() {
            if (!spawned) {
                center = projectile.position + new Vector2(0,20f);
                dir = projectile.velocity;
                // projectile.velocity = Vector2.Zero;

                spawned = true;
            }
            var b = projectile.position;
            var a = center;
            float angle = -(float)Math.Atan2(b.Y-a.Y,b.X-a.X)+MathHelper.ToRadians(90);
            var direction = GetVector(angle);
            direction = RotateRadians(direction,90*3.14f/180/10);
            projectile.position = center + direction*20f;
            // projectile.velocity = projectile.position - (center + direction*20f);

            projectile.position -= projectile.velocity;
            center += dir;
            // var b = projectile.position;
            // var a = center;
            // float angle = -(float)Math.Atan2(b.Y-a.Y,b.X-a.X)+MathHelper.ToRadians(90);
            // var direction = GetVector(angle);
            // direction = RotateRadians(direction,90*3.14f/180/50);
            // projectile.position = center + direction*75f;

            // center += direction;
        }
        public Vector2 GetVector(float angle) {
            return new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle));
        }
        public Vector2 RotateRadians(Vector2 v, double radians)
        {
            var ca = (float)Math.Cos(radians);
            var sa = (float)Math.Sin(radians);
            return new Vector2(ca*v.X - sa*v.Y, sa*v.X + ca*v.Y);
        }
    }
    public class MartosPearlRightProjectile : ModProjectile {
        private Vector2 center;
        Vector2 dir;
        bool spawned = false;
        public override void SetDefaults() {
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.melee = true;
            projectile.magic = true;
            projectile.width = 22;
            projectile.height = 22;
            projectile.tileCollide = true;
        }
        public override void AI() {
            if (!spawned) {
                center = projectile.position + new Vector2(0,-20f);
                dir = projectile.velocity;
                // projectile.velocity = Vector2.Zero;

                spawned = true;
            }
            var b = projectile.position;
            var a = center;
            float angle = -(float)Math.Atan2(b.Y-a.Y,b.X-a.X)+MathHelper.ToRadians(90);
            var direction = GetVector(angle);
            direction = RotateRadians(direction,90*3.14f/180/10);
            projectile.position = center + direction*20f;
            // projectile.velocity = projectile.position - (center + direction*20f);

            projectile.position -= projectile.velocity;
            center -= dir;
            // var b = projectile.position;
            // var a = center;
            // float angle = -(float)Math.Atan2(b.Y-a.Y,b.X-a.X)+MathHelper.ToRadians(90);
            // var direction = GetVector(angle);
            // direction = RotateRadians(direction,90*3.14f/180/50);
            // projectile.position = center + direction*75f;

            // center += direction;
        }
        public Vector2 GetVector(float angle) {
            return new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle));
        }
        public Vector2 RotateRadians(Vector2 v, double radians)
        {
            var ca = (float)Math.Cos(radians);
            var sa = (float)Math.Sin(radians);
            return new Vector2(ca*v.X - sa*v.Y, sa*v.X + ca*v.Y);
        }
    }
}