using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using VRageMath;
using VRage.Game;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Ingame;
using Sandbox.Game.EntityComponents;
using VRage.Game.Components;
using VRage.Collections;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Utils;
using Sandbox.Game.Gui;
using System.Runtime.CompilerServices;
using VRage.Library.Utils;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage;
using System.Collections.Specialized;
using SpaceEngineers.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using Sandbox.Game;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Weapons;
using System.ComponentModel;
using System.Net;
using Sandbox.Definitions;
using EmptyKeys.UserInterface.Generated.DataTemplatesContractsDataGrid_Bindings;
using Sandbox.Game.Weapons.Guns;
using static Sandbox.Graphics.GUI.MyGuiControlTable;
//using Sandbox.ModAPI;
//using SpaceEngineers.Game.ModAPI.Ingame;

namespace dolboeb
{
	public class MyClass : MyGridProgram
	{

		// Может еще добавить типа IgnoreTag? Чтобы в оперделенные пушки оно не сувало патроны
		// Этот игнортэг надо в кустом дату сувать

		// Добавить "помощника" который будет по ошибкам говорить че не так

		// непонятно еще как быть с AP патронами

		// Сделать найстройки в CustomData пб, а не в скрипте, иначе неудобно




		//MyGun CROSSBOW = new MyGun();
		//MyGun ARCHER = new MyGun();
		//MyGun LONGBOW = new MyGun();
		//MyGun BALLSLISTA = new MyGun();

		//MyGun ELEPHANT = new MyGun();
		//MyGun RHINO = new MyGun();
		//MyGun MAMMOTH = new MyGun();
		//MyGun MASTODON = new MyGun();

		//MyGun CIWS = new MyGun();
		//MyGun FLAK = new MyGun();
		//MyGun HEAVYMINIGUN = new MyGun();

		//public void YoptaGun()
		//{
		//	// 300-s
		//	CROSSBOW.Name = "Crossbow"; 
		//	CROSSBOW.Id = "Series300SideCannon";
		//	CROSSBOW.RoF = ARCHER.RoF;

		//	ARCHER.Name = "Archer";
		//	ARCHER.Id = "Series300SingleBarrel";
		//	ARCHER.RoF = 13;

		//	LONGBOW.Name = "Longbow";
		//	LONGBOW.Id = "Series300DoubleBarrel";
		//	LONGBOW.RoF = 25;

		//	BALLSLISTA.Name = "Ballista";
		//	BALLSLISTA.Id = "Series300TrippleBarrel";
		//	BALLSLISTA.RoF = 35;


		//	//900-s
		//	ELEPHANT.Name = "Elephant";
		//	ELEPHANT.Id = "Series900SideCannon";
		//	ELEPHANT.RoF = RHINO.RoF;

		//	RHINO.Name = "Rhino";
		//	RHINO.Id = "Series900SingleBarrel";
		//	RHINO.RoF = 9;

		//	MAMMOTH.Name = "Mammoth";
		//	MAMMOTH.Id = "Series900DoubleBarrel";
		//	MAMMOTH.RoF = 17;

		//	MASTODON.Name = "Mastodon";
		//	MASTODON.Id = "Series900TrippleBarrel";
		//	MASTODON.RoF = 25;


		//	// Other weps
		//	CIWS.Name = "CIWS";
		//	CIWS.Id = "CIWS";
		//	CIWS.RoF = 2;

		//	FLAK.Name = "Quad";
		//	FLAK.Id = "Bofors";
		//	FLAK.RoF = 12;

		//	HEAVYMINIGUN.Name = "Heavy Gatling Fun";
		//          HEAVYMINIGUN.Id = "Vulcan";
		//          HEAVYMINIGUN.RoF = 8;

		//internal class MyGun
		//{
		//	public string Name = "", Id = ""; // Name of the gun, SubID of block, Existing list
		//	public int RoF = 0; // Rate of fire
		//}

		//}











		//пд, хевиГатлинг, квад, 900, 300, цивс, артилерия, рокет лаунчер

// ...UNICODE1...

// Скрипт распределяет боеприпасы по всем орудиям из заданного контейнера


// Чтобы скрипт работал как нужно: 
// 1) Поставьте контейнер, подключенный к общей системе конвееров
// 2) Назовите его также, как установлено в переменной "AmmoInventory"
// 3) Установите требуемое время стрельбы в переменной "ShootTime" (формула для распределения - Темп стрельбы * ShootTime)
// 4) Положите в контейнер подходящие боеприпасы (Gatling ammo box, 300mm HE Shells и т.д.)
// 5) Запустите скрипт
// Скрипт продолжит работу в фоновом режиме. Отключите програмный блок чтобы выключить скрипт.



		string AmmoInventory = "[Cargo-Ammo]"; // Название инвентаря, в котором должны быть патроны для распределения
		int ShootTime = 1; // Время стрельбы (в минутах)
		//string TagAP = "AP"; // Тег для определения в какие пушки должны поступать AP снаряды




		// NO EDIT BELOW.
		// OR ELSE I WILL NUKE PAKISTANIAN REBELS.


		List<IMyTerminalBlock> Gatling = new List<IMyTerminalBlock>();
		List<IMyTerminalBlock> RocketLauncher = new List<IMyTerminalBlock>();
		List<IMyTerminalBlock> Artillery = new List<IMyTerminalBlock>();

		List<IMyConveyorSorter> Archer = new List<IMyConveyorSorter>();
		List<IMyConveyorSorter> Crossbow = new List<IMyConveyorSorter>();
		List<IMyConveyorSorter> Longbow = new List<IMyConveyorSorter>();
		List<IMyConveyorSorter> Ballista = new List<IMyConveyorSorter>();
		List<IMyConveyorSorter> Elephant = new List<IMyConveyorSorter>();
		List<IMyConveyorSorter> Rhino = new List<IMyConveyorSorter>();
		List<IMyConveyorSorter> Mammoth = new List<IMyConveyorSorter>();
		List<IMyConveyorSorter> Mastadon = new List<IMyConveyorSorter>();
		List<IMyConveyorSorter> Ciws = new List<IMyConveyorSorter>(); 
		List<IMyConveyorSorter> Flak = new List<IMyConveyorSorter>(); // quad
		List<IMyConveyorSorter> Minihuy = new List<IMyConveyorSorter>();



		public void SetTurrets()
		{
			GridTerminalSystem.GetBlocksOfType(Archer, a => a.BlockDefinition.SubtypeId.Equals("Series300SingleBarrel"));
			GridTerminalSystem.GetBlocksOfType(Longbow, a => a.BlockDefinition.SubtypeId.Equals("Series300DoubleBarrel"));
			GridTerminalSystem.GetBlocksOfType(Ballista, a => a.BlockDefinition.SubtypeId.Equals("Series300TripleBarrel"));
			GridTerminalSystem.GetBlocksOfType(Rhino, a => a.BlockDefinition.SubtypeId.Equals("Series900SingleBarrel"));
			GridTerminalSystem.GetBlocksOfType(Mammoth, a => a.BlockDefinition.SubtypeId.Equals("Series900DoubleBarrel"));
			GridTerminalSystem.GetBlocksOfType(Mastadon, a => a.BlockDefinition.SubtypeId.Equals("Series900TripleBarrel"));
			GridTerminalSystem.GetBlocksOfType(Ciws, a => a.BlockDefinition.SubtypeId.Equals("CIWS"));
			GridTerminalSystem.GetBlocksOfType(Flak, a => a.BlockDefinition.SubtypeId.Equals("Bofors"));
			GridTerminalSystem.GetBlocksOfType(Minihuy, a => a.BlockDefinition.SubtypeId.Equals("Vulcan"));
			GridTerminalSystem.GetBlocksOfType(Crossbow, a => a.BlockDefinition.SubtypeId.Equals("Series300SideCannon"));
			GridTerminalSystem.GetBlocksOfType(Elephant, a => a.BlockDefinition.SubtypeId.Equals("Series900SideCannon"));
			GridTerminalSystem.GetBlocksOfType<IMyLargeGatlingTurret>(Gatling);
			GridTerminalSystem.GetBlocksOfType<IMyLargeMissileTurret>(Artillery);
		}


		public void SetAmmo()
		{
			MoveAmmo(Gatling, 4);
			MoveAmmo(Artillery, 10);

			MoveAmmo(Crossbow, 13);
			MoveAmmo(Archer, 13);
			MoveAmmo(Longbow, 25);
			MoveAmmo(Ballista, 35);

			MoveAmmo(Rhino, 9);
			MoveAmmo(Mammoth, 17);
			MoveAmmo(Mastadon, 25);
			MoveAmmo(Elephant, 9);

			MoveAmmo(Ciws, 2);
			MoveAmmo(Flak, 12);
			MoveAmmo(Minihuy, 4);
		}


		private void MoveAmmo(IList GunList, int RoF)
		{
			MyFixedPoint CumLoad = (RoF * ShootTime);

			foreach (IMyCubeBlock b in GunList)
			{
				var inventory = b.GetInventory();
				var slut = inventory.GetItemAt(0);


				if (!slut.HasValue) 
				{ 
					MoveInventory(inventory, (int)CumLoad, false);

				}
				else if (!slut.Value.Amount.Equals(CumLoad))
				{
					Console($"{b.DisplayNameText} - {(int)CumLoad - (int)slut.Value.Amount}\n");

					MoveInventory(inventory, (int)CumLoad - (int)slut.Value.Amount, false);
					continue;

				}


			}
		}

		private void MoveInventory(IMyInventory dest, int amount, bool unload)
		{
			var Container = (IMyCargoContainer)GridTerminalSystem.GetBlockWithName(AmmoInventory);
			var src = Container.GetInventory();

			for (int i = 0; i < src.ItemCount; i++)
			{
				var item = src.GetItemAt(i);
				if (!item.HasValue) continue;
				if (unload == false)
				{
					src.TransferItemTo(dest, item.Value, amount);
				}
				else
				{
					src.TransferItemFrom(dest, item.Value, amount);
				}
			}
		}

		internal void Console(object message)
		{ Echo(Convert.ToString(message)); }

		internal void SetConfigurator()
		{
			//string AmmoInventory = Me.CustomData = "ABC";
		}

		public void Main(string args)
		{
			Console("LEFT TO LOAD:\n");
			Runtime.UpdateFrequency = UpdateFrequency.Update100;
			SetTurrets();
			SetAmmo();
		}

		public void Save()
		{
			// вот здесь будет инфа для кастом даты
		}

		public Program()
		{
			SetConfigurator();
		}



		// ...UNICODE1...

	}
}