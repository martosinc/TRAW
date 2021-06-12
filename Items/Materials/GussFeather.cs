using Terraria.ModLoader;
using Terraria.ID;

namespace TRAW.Items.Materials
{
    public class GussFeather : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Перо Гусса");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 30;
            item.value = 51;
            item.rare = 2;
            item.maxStack = 999;
        }
    }
}
