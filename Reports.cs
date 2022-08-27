﻿using System;
using System.Collections.Generic;
using System.Linq;

using PhantomBrigade.Data;
using UnityEngine;

namespace fragmentMod
{
	public partial class Patch
	{
		private static void Report(List<(Func<int, int, object, string>, int, int, object)> debugInfo)
		{
			Debug.Log(string.Join("\n", debugInfo.Select(x => x.Item1(x.Item2, x.Item3, x.Item4))));
		}

		private static string ReportCombatContext(int i, int n, object o) => string.Format("[1] - Context Shared Instance (combat): {0}", o);
		private static string ReportPart(int i, int n, object o) => string.Format("[2] - projectile {0} - parent part: {1}", i, ((EquipmentEntity)o).dataKeyPartPreset.s);
		private static string ReportSubsystem(int i, int n, object o) => string.Format("[3] - projectile {0} - parent subsystem: {1}", i, ((EquipmentEntity)o).dataKeySubsystem.s);
		private static string ReportBlueprint(int i, int n, object o) => string.Format("[4] - projectile {0} - Blueprint: {1}", i, o);
		private static string ReportFragmentDelay(int i, int n, object o) => string.Format("[5] - projectile {0} - Fragment Delay: {1}", i, ((ValueTuple<bool, float>)o).Item1 ? (object)(((ValueTuple<bool, float>)o).Item2) : (object)"<null>");
		private static string ReportFragmentKey(int i, int n, object o) => string.Format("[6] - projectile {0} - Fragment Key: {1}", i, ((ValueTuple<bool, string>)o).Item1 ? ((ValueTuple<bool, string>)o).Item2 : "<null>");
		private static string ReportFragmentHardpoint(int i, int n, object o) => string.Format("[7] - projectile {0} - Fragment Hardpoint: {1}", i, ((ValueTuple<bool, string>)o).Item1 ? ((ValueTuple<bool, string>)o).Item2 : "<null>");
		private static string ReportBodyAssetScale(int i, int n, object o) => string.Format("[8] - projectile {0} - BodyAssetScale(x,y,z): {1}", i, o);
		private static string ReportPresetMirv(int i, int n, object o) => string.Format("[9] - projectile {0} - presetMirv | {1}", i, o);
		private static string ReportPartMirv(int i, int n, object o) => string.Format("[10] - projectile {0} - partMirv | {1}", i, o);
		private static string ReportSubsystemMirv(int i, int n, object o) => string.Format("[11] - projectile {0} - subsystemMirv | {1}", i, o);
		private static string ReportFragmentBlueprint(int i, int n, object o) => string.Format("[12] - projectile {0} - fragmentBlueprint | {1}", i, o);
		private static string ReportProjectileData(int i, int n, object o) => string.Format("[13] - projectile {0} - projectileData | {1}", i, o);

		private static string ReportCloneProjectileData(int i, int n, object o)
		{
			var v = (ValueTuple<bool, DataBlockSubsystemProjectile_V2>)o;
			if (v.Item1)
			{
				return string.Format("[15] - projectile {0} - fragment {1} - Projectile Data Link (root) added! - result | ProjectileData: {2}", i, n, v.Item2);
			}
			return "[15] - Projectile Data Link (root) not added";
		}

		private static string ReportCloneParentPart(int i, int n, object o)
		{
			var v = (ValueTuple<bool, CombatEntity, EquipmentEntity>)o;
			if (v.Item1)
			{
				return string.Format("[16] - projectile {0} - fragment {1} - parent part (root) added | value: {2} ({3})", i, n, v.Item2.parentPart.equipmentID, v.Item3.id.id);
			}
			return "[16] - parent part (root) not added";
		}

		private static string ReportCloneParentSubsystem(int i, int n, object o)
		{
			var v = (ValueTuple<bool, CombatEntity, EquipmentEntity>)o;
			if (v.Item1)
			{
				return string.Format("[17] - projectile {0} - fragment {1} - parent subsystem (root) added | value: {2} ({3})", i, n, v.Item2.parentSubsystem.equipmentID, v.Item3.id.id);
			}
			return "[17] - parent subsystem (root) not added";
		}

		private static string ReportCloneLevel(int i, int n, object o)
		{
			var v = (ValueTuple<bool, CombatEntity>)o;
			if (v.Item1)
			{
				return string.Format("[19] - projectile {0} - fragment {1} - Projectile level added | value: {2}", i, n, v.Item2.level.i);
			}
			return "[19] - Projectile level not added";
		}

		private static string ReportCloneProjectileCollision(int i, int n, object o)
		{
			var v = (ValueTuple<bool, int>)o;
			if (v.Item1)
			{
				return string.Format("[24] - projectile {0} - fragment {1} - Projectile collision added | value: {2}", i, n, v.Item2);
			}
			return "[24] - Projectile collision not added";
		}

		private static string ReportCloneInflictedDamage(int i, int n, object o)
		{
			var v = (ValueTuple<bool, float>)o;
			if (v.Item1)
			{
				return string.Format("[28] - projectile {0} - fragment {1} - InflictedDamage added | value: {2}", i, n, v.Item2);
			}
			return "[28] - InflictedDamage not added";
		}

		private static string ReportCloneProjectileTargetPosition(int i, int n, object o)
		{
			var v = (ValueTuple<bool, CombatEntity>)o;
			if (v.Item1)
			{
				return string.Format("[27] - projectile {0} - fragment {1} - ProjectileTargetPosition added | value: {2}", i, n, v.Item2.projectileGuidanceTargetPosition.v.normalized);
			}
			return "[27] - ProjectileTargetPosition not added";
		}

		private static string ReportCloneMovementSpeedCurrent(int i, int n, object o)
		{
			var v = (ValueTuple<bool, CombatEntity>)o;
			if (v.Item1)
			{
				return string.Format("[29] - projectile {0} - fragment {1} - MovementSpeedCurrent added | value: {2}", i, n, v.Item2.movementSpeedCurrent.f);
			}
			return "[29] - MovementSpeedCurrent not added";
		}

		private static string ReportCloneRicochetChance(int i, int n, object o)
		{
			var v = (ValueTuple<bool, CombatEntity>)o;
			if (v.Item1)
			{
				return string.Format("[30] - projectile {0} - fragment {1} - RicochetChance added | value: {2}", i, n, v.Item2.ricochetChance.f);
			}
			return "[30] - RicochetChance not added";
		}

		private static string ReportCloneFlightInfo(int i, int n, object o)
		{
			var v = (ValueTuple<bool, CombatEntity>)o;
			if (v.Item1)
			{
				return string.Format("[13] - projectile {0} - fragment {1} - FlightInfo added | value: {2}, {3}", i, n, v.Item2.flightInfo.origin, v.Item2.flightInfo.positionLast);
			}
			return "[13] - FlightInfo not added";
		}

		private static string ReportCloneSimpleMovement(int i, int n, object o) => string.Format("[31] - projectile {0} - fragment {1} - SimpleMovement: {2}", i, n, o);
		private static string ReportCloneSimpleFaceMotion(int i, int n, object o) => string.Format("[32] - projectile {0} - fragment {1} - SimpleFaceMotion: {2}", i, n, o);

		private static string ReportClonePosition(int i, int n, object o)
		{
			var v = (ValueTuple<bool, CombatEntity>)o;
			if (v.Item1)
			{
				return string.Format("[20] - projectile {0} - fragment {1} - ProjectilePosition added | value: {2}", i, n, v.Item2.position.v.normalized);
			}
			return "[20] - ProjectilePosition not added";
		}

		private static string ReportCloneRotation(int i, int n, object o)
		{
			var v = (ValueTuple<bool, CombatEntity>)o;
			if (v.Item1)
			{
				return string.Format("[21] - projectile {0} - fragment {1} - ProjectileRotation added | value: {2}", i, n, v.Item2.rotation.q);
			}
			return "[21] - ProjectileRotation not added";
		}

		private static string ReportCloneFacing(int i, int n, object o)
		{
			var v = (ValueTuple<bool, CombatEntity>)o;
			if (v.Item1)
			{
				return string.Format("[22] - projectile {0} - fragment {1} - Facing added | value: {2}", i, n, v.Item2.facing.v);
			}
			return "[22] - Facing not added";
		}

		private static string ReportCloneScale(int i, int n, object o)
		{
			var v = (ValueTuple<bool, CombatEntity>)o;
			if (v.Item1)
			{
				return string.Format("[18] - projectile {0} - fragment {1} - Scale added | value: {2}", i, n, v.Item2.scale.v);
			}
			return "[18] - Scale not added";
		}

		private static string ReportCloneSourceEntity(int i, int n, object o)
		{
			var v = (ValueTuple<bool, CombatEntity>)o;
			if (v.Item1)
			{
				return string.Format("[25] - projectile {0} - fragment {1} - SourceEntity (parentAction) Added | Parent Action: {2}", i, n, v.Item2.sourceEntity.combatID);
			}
			return "[25] - SourceEntity (parentAction) not Added";
		}

		private static string ReportCloneTimeToLive(int i, int n, object o)
		{
			var v = (ValueTuple<bool, CombatEntity>)o;
			if (v.Item1)
			{
				return string.Format("[23] - projectile {0} - fragment {1} - HasTimeToLive Added | value: {2}", i, n, v.Item2.timeToLive.f);
			}
			return "[23] - HasTimeToLive Not Added";
		}

		private static string ReportCloneProjectileTargetEntity(int i, int n, object o)
		{
			var v = (ValueTuple<bool, CombatEntity>)o;
			if (v.Item1)
			{
				return string.Format("[26] - projectile {0} - fragment {1} - ProjectileTargetEntity Added | value: {2}", i, n, v.Item2.projectileTargetEntity.combatID);
			}
			return "[26] - ProjectileTargetEntity Not Added";
		}

		private static string ReportCloneProjectileIndex(int i, int n, object o)
		{
			var v = (ValueTuple<bool, CombatEntity>)o;
			if (v.Item1)
			{
				return string.Format("[28] - projectile {0} - fragment {1} - ProjectileIndex Added | value: {2}", i, n, v.Item2.projectileIndex.i);
			}
			return "[28] - ProjectileIndex Not Added";
		}
	}
}
