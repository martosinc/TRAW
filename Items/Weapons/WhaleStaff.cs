using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System;
using Terraria.ModLoader;

namespace TRAW.Items.Weapons
{
    public class WhaleBuff : ModBuff
    {

		public override void SetDefaults() {
			DisplayName.SetDefault("Кашалот");
			Description.SetDefault("Кашалот сражается за вас");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex) {
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Whale>()] > 0) {
				player.buffTime[buffIndex] = 18000;
			}
			else {
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}

    }
    public class WhaleStaff : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Посох Кашалота");
            Tooltip.SetDefault("Не шизанись");
        }
        public override void SetDefaults() {
			item.damage = 22;
			item.knockBack = 3f;
			item.mana = 6;
			item.width = 32;
			item.height = 32;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = Item.buyPrice(0, 30, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.UseSound = SoundID.Item44;

            item.noMelee = true;
            item.summon = true;
            item.buffType = ModContent.BuffType<WhaleBuff>();
            item.shoot = ModContent.ProjectileType<Whale>();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            player.AddBuff(item.buffType, 2);
            position = Main.MouseWorld;
            return true;
        }
    }
    public class Whale : ModProjectile
    {
        public override void SetStaticDefaults() {
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            Main.projFrames[projectile.type] = 4;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }
        public override void SetDefaults() {
            projectile.height = 102;
            projectile.width = 102;

            projectile.tileCollide = false;
            projectile.ignoreWater = true;

            projectile.friendly = true;
            projectile.minion = true;
            projectile.minionSlots = 1f;
            projectile.penetrate = -1;
        }
        public override bool? CanCutTiles() {
            return false;
        }

        public override bool MinionContactDamage() {
            return true;
        }
        private int STEP = 0;
        private int TIMER = 0;
        private int DASH = 0;
        private float rotated = 0f;
        private bool DASHING = false;
        private Vector2 oldVelocity;
        private Vector2 center;
        public override void AI() {
            int speed = 10;
            float inertia = 11f;
            bool foundTarget = false;

            Player player = Main.player[projectile.owner];
            if (player.dead || !player.active) {
                player.ClearBuff(ModContent.BuffType<WhaleBuff>());
            }
            if (player.HasBuff(ModContent.BuffType<WhaleBuff>())) {
                projectile.timeLeft = 2;
            }  
            projectile.rotation =
                projectile.velocity.ToRotation() +
                MathHelper.ToRadians(90f); 
            
            Vector2 targetCenter = GetTarget(ref foundTarget);

            Vector2 move = new Vector2(0,0);

            Vector2 idlePosition = player.Center;

            Vector2 vectorToIdlePosition = idlePosition - projectile.Center;
			float distanceToIdlePosition = vectorToIdlePosition.Length();
            if (distanceToIdlePosition > 1200f) {
				projectile.position = idlePosition+new Vector2(0,-300f);
			}

            if (!foundTarget && STEP!=2) {
                STEP=0;
            }

            switch (STEP) {
                case 0:
                    if (foundTarget) {
                        STEP++;
                    } else {
                        if (Vector2.Distance(projectile.position,player.Center)<200f) {
                            var b = projectile.position;
                            var a = player.Center;
                            float angle = -(float)Math.Atan2(b.Y-a.Y,b.X-a.X)+MathHelper.ToRadians(90);
                            var direction = GetVector(angle);
                            direction = RotateRadians(direction,90*3.14f/180/50);
                            projectile.position = player.Center + direction*200f;
                            // projectile.position.X += projectile.width/2;
                            // projectile.position.Y -= projectile.height/2;
                            projectile.rotation =
                                direction.ToRotation() +
                                MathHelper.ToRadians(90f);
                        } else {
                            move = player.Center - projectile.Center;
                        }
                    }
                    break;
                case 1:
                    if (!DASHING) {
                        move = targetCenter - projectile.Center;
                    } else {
                        move = oldVelocity;
                    }
                    move *= 60;
                    inertia = 5f;
                    if (Vector2.Distance(targetCenter, projectile.Center) < 75f) {
                        if (!DASHING) {
                            DASH++;
                            oldVelocity = move;
                        }
                        DASHING = true;

                        if (DASH == 5) {
                            center = projectile.position;
                            center.Y -= 150f;
                            rotated = 0f;
                            DASH = 0;
                            STEP++;
                        }
                    } else if (DASHING) {
                        DASHING = false;
                    }
                    break;
                case 2:
                    if (rotated < 3.14f*2.25f) {
                        var c = projectile.position;
                        var d = center;
                        float angle = -(float)Math.Atan2(c.Y-d.Y,c.X-d.X)+MathHelper.ToRadians(90);
                        // Main.NewText(angle.ToString(), 175, 75, 255);
                        var dir = GetVector(angle);
                        dir = RotateRadians(dir,90*3.14f/180/12.5f);
                        projectile.position = center + dir*150f;
                        projectile.rotation =
                                dir.ToRotation() +
                                MathHelper.ToRadians(90f);
                        // projectile.position.X += 2f;
                        rotated += 90*3.14f/180/12.5f;
                        move = dir;
                    } else {
                        STEP=0;
                    }
                    break;
            }

            AdjustMagnitude(ref move);
            projectile.velocity = (projectile.velocity * (inertia - 1) + move*1.75f) / inertia;
        }
        public Vector2 RotateRadians(Vector2 v, double radians)
        {
            var ca = (float)Math.Cos(radians);
            var sa = (float)Math.Sin(radians);
            return new Vector2(ca*v.X - sa*v.Y, sa*v.X + ca*v.Y);
        }
        public Vector2 GetVector(float angle) {
            return new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle));
        }
		private void AdjustMagnitude(ref Vector2 vector) {
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 5f) {
				vector *= 10f / magnitude;
			}
		}
        private Vector2 GetTarget(ref bool foundTarget) {
            NPC npc;
            float distanceFromTarget = 700f;
            Vector2 targetCenter = projectile.position;

            for (int i = 0; i < Main.maxNPCs; i++) {
                npc = Main.npc[i];
                if (npc.CanBeChasedBy()) {
                    float between = Vector2.Distance(npc.Center, projectile.Center);
                    bool closest = Vector2.Distance(projectile.Center, targetCenter) > between;
                    bool inRange = between < distanceFromTarget;
                    bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);
                    if (((closest && inRange) || !foundTarget) && (lineOfSight)) {
                        distanceFromTarget = between;
                        targetCenter = npc.Center;
                        foundTarget = true;
                    }
                }
            }

            return targetCenter;
        }
    }
}

            // var b = projectile.position;
            // var a = center;
            // float angle = -(float)Math.Atan2(b.Y-a.Y,b.X-a.X)+MathHelper.ToRadians(90);
            // var direction = GetVector(angle);
            // direction = RotateRadians(direction,90*3.14f/180/50);
            // projectile.position = center + direction*75f;

            // center += direction;