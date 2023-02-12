using Microsoft.Xna.Framework;

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
	public class HeldProjWithActions : ModProjBehaviorComponent<ModProjectile>
	{
		

		public override bool NetUpdateThis =>true;

		public override string BehaviorName => "Action8heldItem";

		public PlayerControlHelper ControlHelper;

		public int[] ControlBehaviorsID = new int[256];

		public Player Player => Main.player[Projectile.owner];

		public HeldProjWithActions(ModProjectile modProjectile,int itemType,int ControlConsidered,int ControlConsideredMain):base(modProjectile)
		{
			Add(new CheckHeldProj(modProjectile, itemType));
			ControlHelper = new(Player,ControlConsidered,ControlConsideredMain);
		}
		public void AddControl(int id, int ControlFlag) {
			ControlHelper.ActionsStart[ControlFlag] += (p,i) =>
				ActivateBehavior(id);
			ControlHelper.ActionsEnd[ControlFlag] += (p, i) =>
				!PauseBehavior(id);
		}

		public int AddControl(IModProjBehavior behavior, int ControlFlag) {
			int id = Add(behavior);
			AddControl(id, ControlFlag);
			return id;
		}

		public override void AI()
		{
			ControlHelper.TryReset(Player);
			ControlHelper.Update();
			base.AI();
		}

	}
}
