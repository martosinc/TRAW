using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;          
using Terraria.ModLoader;
using System;
using static Terraria.ModLoader.ModContent;

namespace TRAW.NPCs
{
    public class GussSlime : ModNPC  {
        public override void SetStaticDefaults() 
        {
            DisplayName.SetDefault("Гусс-слайм");
        }

        public override void SetDefaults()   
        {
            npc.width = 38;               
            npc.height = 37;              
            npc.damage = 15;             
            npc.defense = 3;             
            npc.lifeMax = 30;            
            npc.HitSound = SoundID.NPCHit1 ;            
            npc.DeathSound = SoundID.NPCDeath1;          
            npc.value = 10f;             
            npc.knockBackResist = 0f;      
            Main.npcFrameCount[npc.type] = 2; 
            npc.aiStyle = 1;
            aiType = NPCID.BlueSlime;
            animationType = NPCID.BlueSlime;
			banner = Item.NPCtoBanner(NPCID.BlueSlime);
			bannerItem = Item.BannerToItem(banner);
        }
        public override void AI() {
            if (npc.velocity.X == 0) {
            } else if (npc.velocity.X >= 0) {
                npc.spriteDirection = -1;
            } else {
                npc.spriteDirection = 1;
            }
        }
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			return SpawnCondition.OverworldDay.Chance * 0.5f;
		}
		public override void NPCLoot()
		{
			int loots = Main.rand.Next(2);
			switch (loots)
			{
				case 1:
					Item.NewItem(npc.getRect(), mod.ItemType("GussFeather"), Main.rand.Next(1, 3)); break;
			}
		}
    }
}