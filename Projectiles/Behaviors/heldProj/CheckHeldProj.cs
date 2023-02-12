using Microsoft.Xna.Framework;

using rail;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace XLib.Projectiles.Behaviors.heldProj
{
	public class CheckHeldProj:ModProjBehavior<ModProjectile>
	{
		public int ItemType;
		public override string BehaviorName => "CheckHeldProj";
		public override bool NetUpdate => false;
		public Action<Projectile, Player> SetPosition= DefSetPosition;
		public static void DefSetPosition(Projectile proj, Player plr)
		{
			//proj.Center = plr.itemLocation;
			//proj.rotation = plr.itemRotation;
			if (Main.myPlayer == plr.whoAmI) {
				Vector2 r = (Main.MouseWorld - plr.Center);
				r.Normalize();
				proj.Center = plr.Center+r*24;
				proj.rotation = r.ToRotation();
			}
		}
		public CheckHeldProj(ModProjectile modProjectile, int itemType):base(modProjectile)
		{
			ItemType = itemType;
		}
		public Player Player => Main.player[Projectile.owner];


		public override void PostAI()
		{
			Player player = Player;
			if (!player.PlayerCanUse() || player.noItems || player.HeldItem.type != ItemType) Projectile.Kill();
			
		}
		public override bool PreAI()
		{
			Player.heldProj = Projectile.whoAmI;
			if (Projectile.timeLeft < 2) Projectile.timeLeft = 2;
			SetPosition?.Invoke(Projectile,Player);
			return base.PreAI();
		}
	}
}
