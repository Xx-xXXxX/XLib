using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

using XLib.Behaviors;

namespace XLib.Projectiles.Behaviors
{
	public abstract class ModProjWithBehavior : ModProjectile
	{
		public IModProjBehavior behavior;

		public override void SetDefaults()
		{
			base.SetDefaults();
			behavior.TrySetActive(true);
		}

		public override void AI()
		{
			behavior.AI();
		}

		public override bool? CanCutTiles()
		{
			return behavior.CanCutTiles();
		}

		public override bool? CanDamage()
		{
			return behavior.CanDamage();
		}

		public override bool? CanHitNPC(NPC target)
		{
			return behavior.CanHitNPC(target);
		}

		public override bool CanHitPlayer(Player target)
		{
			return behavior.CanHitPlayer(target);
		}

		public override bool CanHitPvp(Player target)
		{
			return behavior.CanHitPvp(target);
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			return behavior.Colliding(projHitbox, targetHitbox);
		}

		public override void CutTiles()
		{
			behavior.CutTiles();
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			behavior.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return behavior.GetAlpha(lightColor);
		}

		public override void GrapplePullSpeed(Player player, ref float speed)
		{
			behavior.GrapplePullSpeed(player, ref speed);
		}

		public override float GrappleRange()
		{
			return behavior.GrappleRange();
		}

		public override void GrappleRetreatSpeed(Player player, ref float speed)
		{
			behavior.GrappleRetreatSpeed(player, ref speed);
		}

		public override void GrappleTargetPoint(Player player, ref float grappleX, ref float grappleY)
		{
			behavior.GrappleTargetPoint(player, ref grappleX, ref grappleY);
		}

		public override void Kill(int timeLeft)
		{
			behavior.Kill(timeLeft);
			behavior.Dispose();
		}

		public override bool MinionContactDamage()
		{
			return behavior.MinionContactDamage();
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			behavior.ModifyDamageHitbox(ref hitbox);
		}

		public override void ModifyDamageScaling(ref float damageScale)
		{
			behavior.ModifyDamageScaling(ref damageScale);
		}

		public override void ModifyFishingLine(ref Vector2 lineOriginOffset, ref Color lineColor)
		{
			behavior.ModifyFishingLine(ref lineOriginOffset, ref lineColor);
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			behavior.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
		}

		public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
		{
			behavior.ModifyHitPlayer(target, ref damage, ref crit);
		}

		public override void ModifyHitPvp(Player target, ref int damage, ref bool crit)
		{
			behavior.ModifyHitPvp(target, ref damage, ref crit);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			behavior.OnHitNPC(target, damage, knockback, crit);
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			behavior.OnHitPlayer(target, damage, crit);
		}

		public override void OnHitPvp(Player target, int damage, bool crit)
		{
			behavior.OnHitPvp(target, damage, crit);
		}

		public override void OnSpawn(IEntitySource source)
		{
			behavior.OnSpawn(source);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return behavior.OnTileCollide(oldVelocity);
		}

		public override void PostAI()
		{
			behavior.PostAI();
		}

		public override void PostDraw(Color lightColor)
		{
			behavior.PostDraw(lightColor);
		}

		public override bool PreAI()
		{
			return behavior.PreAI();
		}

		public override bool PreDraw(ref Color lightColor)
		{
			return behavior.PreDraw(ref lightColor);
		}

		public override bool PreDrawExtras()
		{
			return behavior.PreDrawExtras();
		}

		public override bool PreKill(int timeLeft)
		{
			return behavior.PreKill(timeLeft);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			behavior.ReceiveExtraAI(reader);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			behavior.SendExtraAI(writer);
		}

		public override bool ShouldUpdatePosition()
		{
			return behavior.ShouldUpdatePosition();
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			return behavior.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}
	}
}
