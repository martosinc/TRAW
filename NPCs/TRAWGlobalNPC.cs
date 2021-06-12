using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAW.NPCs
{
    public class TRAWGlobalNPC : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            int[] dungeon_mobs = {NPCID.AngryBones,NPCID.DarkCaster,NPCID.CursedSkull,NPCID.AngryBonesBig,NPCID.AngryBonesBigMuscle,NPCID.AngryBonesBigHelmet};
            if (dungeon_mobs.Contains(npc.type))
            {
                if (Main.rand.Next(15) == 0)   //item rarity
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ZeroEssence"),Main.rand.Next(4)+1);
                }
            }
            if (npc.type == NPCID.SkeletronHead) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ZeroEssence"),Main.rand.Next(10,20));
            }
        }
    }
}