﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;
using static MetroidMod.Common.Configs.MServerConfig;

namespace MetroidMod.Common.Configs
{
	[Label("Client Side")]
	public class MConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;
		
		public static MConfig Instance;

		public MConfig()
		{
		}
		
		[Header("General")]
		
		[Label("Toggle alternate weapon textures")]
		[Tooltip("When enabled, shows Metroid Prime style weapons, as opposed to the default Super Metroid style.\n" +
		"Default value: false")]
		public bool UseAltWeaponTextures;

		[Label("[i:MetroidMod/EnergyTankAddon] Low Energy Alert")]
		[Tooltip("When enabled, a beep will be heard when Suit Energy is low.\n" +
		"Default value: true")]
		[DefaultValue(true)]
		public bool energyLow;

        [Label("[i:MetroidMod/EnergyTankAddon] Low Energy Alert Interval")]
        [Tooltip("The interval between Low Energy beeps.\n[Default: 20]")]
        [Slider]
        [DefaultValue(20)]
        [Range(5, 200)]
        [Increment(5)]
        public int energyLowInterval;

		[Label("[i:MetroidMod/EnergyTankAddon] Low Energy Alert Fade")]
		[Tooltip("When enabled, a fading, non-looping beep will be heard when Suit Energy is low.\n" +
		"Default value: false\n" + "(WORK IN PROGRESS)")]
		[DefaultValue(false)]
		public bool energyLowFade;

		[Header("Draggable UI Panels")]
		[Label("Power Beam")]
		public DragablePanelPage PowerBeam = new();
		[Label("Power Beam Error")]
		public DragablePanelPage PowerBeamError = new();
		[Label("Missile Launcher")]
		public DragablePanelPage MissileLauncher = new();
		[Label("Morph Ball")]
		public DragablePanelPage MorphBall = new();
		[Label("Sense Move")]
		public DragablePanelPage SenseMove = new();
		[Label("Helmet Addons")]
		public DragablePanelPage HelmetAddons = new();
		[Label("Breastplate Addons")]
		public DragablePanelPage BreastplateAddons = new();
		[Label("Greaves Addons")]
		public DragablePanelPage GreavesAddons = new();
		[Label("Reserves Menu")]
		public DragablePanelPage Reserves = new();
		[Label("Charge Somersault (Psuedo Screw Attack)")]
		public DragablePanelPage PsuedoScrewAttack = new();

		[SeparatePage]
		public class DragablePanelPage
		{
			public DragablePanelPage()
			{

			}

			[Label("Enabled")]
			[Tooltip("Allows the UI to be draggable.\n" +
			"Default value: false")]
			[DefaultValue(false)]
			public bool enabled = false;

			[Label("Automatically move the UI")]
			[Tooltip("Automatically moves the UI according to other UI elements.\n" +
			"Default value: true")]
			[DefaultValue(true)]
			public bool auto = true;

			[Header("Location and Placement")]

			[Label("Measure X from the left")]
			[Tooltip("Measure X Displacement from the left side of the screen.\n" +
			"Default value: true")]
			public bool fromLeft = true;
			[Label("X Displacement")]
			public float locationX;
			[Label("Measure Y from the top")]
			[Tooltip("Measure Y Displacement from the top side of the screen.\n" +
			"Default value: true")]
			public bool fromTop = true;
			[Label("Y Displacement")]
			public float locationY;

			public override string ToString()
			{
				return $"{enabled} {auto} {fromLeft} {locationX} {fromTop} {locationY}";
			}

			public override bool Equals(object obj)
			{
				if (obj is DragablePanelPage other)
					return enabled == other.enabled && auto == other.auto && locationX == other.locationX && locationY == other.locationY;
				return base.Equals(obj);
			}

			public override int GetHashCode() => new { enabled, auto, locationX, locationY }.GetHashCode();
		}

		[Header("Map Icons")]
		
		[Label("[i:MetroidMod/GoldenTorizoSummon] Show Torizo Room Location on Map")]
		[Tooltip("When enabled, the map will show an icon where Torizo's boss room is.\n" +
		"Default value: true")]
		[DefaultValue(true)]
		public bool showTorizoRoomIcon;

		public override void OnChanged()
		{
			MetroidMod.UseAltWeaponTextures = UseAltWeaponTextures;
		}
	}

	[Label("Client Side Debug")]
	public class MDebugConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;
		
		public static MDebugConfig Instance;

		public MDebugConfig()
		{
			Instance = this;
		}
		
		[Label("Draw NPC hitboxes")]
		[Tooltip("When enabled, draws NPC hitboxes.\n" +
		"Default value: false")]
		public bool DrawNPCHitboxes;

		[Label("Markers for statue items")]
		[Tooltip("When enabled, draws markers for statue items\n" +
		"Note: Performance will tank on world load\n" +
		"Default value: false")]
		public bool StatueItemMarkers;

		public override void OnChanged()
		{
			MetroidMod.DebugDH = DrawNPCHitboxes;
			MetroidMod.DebugDSI = StatueItemMarkers;
		}
	}
}
