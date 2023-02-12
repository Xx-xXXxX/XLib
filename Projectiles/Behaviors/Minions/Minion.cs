using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace XLib.Projectiles.Behaviors.Minions
{
	public class Minion:ModProjBehaviorComponent<ModProjectile>
	{
		public Minion(ModProjectile modProj, int minionBuffType) : base(modProj)
		{
			MinionBuffType = minionBuffType;
		}

		public override string BehaviorName =>"Minion";
		public override bool NetUpdateThis =>false;

		public Player Owner => Main.player[Projectile.owner];
		public readonly int MinionBuffType;
		/// <summary>
		/// 与玩家距离太远传送的距离，<![CDATA[<]]>0不传送
		/// </summary>
		public int TeleportDistance;

		public override void AI()
		{
			if (!Owner.HasBuff(MinionBuffType)) { Projectile.Kill();return; }
			if ((Projectile.Center - Owner.Center).Length() > TeleportDistance) { Projectile.Center = Owner.Center; }
			base.AI();
		}
	}
}
