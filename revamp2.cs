using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
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
using Sandbox.Game.Weapons.Guns;
using System.Reflection.Metadata.Ecma335;
using static VRage.Game.MyObjectBuilder_Toolbar;
using Sandbox.Game.Screens;
//using Sandbox.ModAPI;
//using SpaceEngineers.Game.ModAPI.Ingame;
namespace dolboeb
{
	public class MyClass : MyGridProgram
	{
			// TODO Сделать найстройки в CustomData пб, а не в скрипте, иначе неудобно

			// TODO Сделать один большой универсальный список пушек
			// * Чтобы не ебаться и постоянно новый список под каждый ствол не добавлять
		


		// ...UNICODE1...

		// Скрипт распределяет боеприпасы по всем орудиям из заданного контейнера


		// Чтобы скрипт работал: 
		// 1) Поставьте контейнер, подключенный к общей системе конвееров
		// 2) Назовите его также, как установлено в переменной "AmmoInventory"
		// 3) Установите требуемое время стрельбы в переменной "ShootTime" (формула для распределения - Темп стрельбы * ShootTime)
		// 4) Положите в контейнер подходящие боеприпасы (Gatling ammo box, 300mm HE Shells и т.д.)
		// 5) Запустите скрипт кнопкой Run
		// Скрипт продолжит работу в фоновом режиме. Отключите програмный блок чтобы выключить скрипт.
		// При стыковании нового корабля с базой следует снова запустить скрипт кнопкой Run, чтобы новые орудия добавились в список.
		// В случае, если в инвентаре орудия оказалось больше патронов, чем должно быть, скрипт достанет лишние патроны и положит в ящик.



		string AmmoInventory = "[Cargo-Ammo]"; // Название инвентаря, в котором должны быть патроны для распределения (Станд. - [Cargo-Ammo])
		int ShootTime = 1; // Время стрельбы (в минутах) (Станд. - 1)
		bool Delay = true; // Исскуственная задержка на 3 секунды, нужная для оптимизации. Может быть отключена, установив значение false (Станд. - true)




		// NO EDIT BELOW.
		// OR ELSE I WILL NUKE PAKISTANIAN REBELS.




		List<IMyFunctionalBlock> Gatling = new List<IMyFunctionalBlock>();
		List<IMyFunctionalBlock> RocketLauncher = new List<IMyFunctionalBlock>();
		List<IMyFunctionalBlock> Artillery = new List<IMyFunctionalBlock>();
		List<IMyFunctionalBlock> Autocannon = new List<IMyFunctionalBlock>();

		List<IMyFunctionalBlock> Archer = new List<IMyFunctionalBlock>();
		List<IMyFunctionalBlock> Crossbow = new List<IMyFunctionalBlock>();
		List<IMyFunctionalBlock> Longbow = new List<IMyFunctionalBlock>();
		List<IMyFunctionalBlock> Ballista = new List<IMyFunctionalBlock>();

		List<IMyFunctionalBlock> Elephant = new List<IMyFunctionalBlock>();
		List<IMyFunctionalBlock> Rhino = new List<IMyFunctionalBlock>();
		List<IMyFunctionalBlock> Mammoth = new List<IMyFunctionalBlock>();
		List<IMyFunctionalBlock> Mastadon = new List<IMyFunctionalBlock>();

		List<IMyFunctionalBlock> Ciws = new List<IMyFunctionalBlock>(); 
		List<IMyFunctionalBlock> Flak = new List<IMyFunctionalBlock>(); // quad
		List<IMyFunctionalBlock> Cannonsentry = new List<IMyFunctionalBlock>();
		List<IMyFunctionalBlock> Minihuy = new List<IMyFunctionalBlock>();

		List<MyItemType> ammo = new List<MyItemType>();

		MyInventory src;

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
			GridTerminalSystem.GetBlocksOfType(Cannonsentry, a => a.BlockDefinition.SubtypeId.Equals("SG_RocketSentry"));
			GridTerminalSystem.GetBlocksOfType(Autocannon, a => a.BlockDefinition.SubtypeId.Equals("AutoCannonTurret"));
			GridTerminalSystem.GetBlocksOfType(Gatling, a => a.BlockDefinition.SubtypeId.Equals("LargeGatlingTurret")); 
			GridTerminalSystem.GetBlocksOfType(Artillery, a => a.BlockDefinition.SubtypeId.Equals("LargeCalibreTurret")); 
			GridTerminalSystem.GetBlocksOfType(RocketLauncher, a => a.BlockDefinition.SubtypeId.Equals("LargeMissileLauncher")); 
		}


		private void SetAmmo()
		{
			MoveAmmo(Gatling, 4);
			MoveAmmo(Artillery, 10);
			MoveAmmo(RocketLauncher, 2);
			MoveAmmo(Autocannon, 5);

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
		}

		// ПЕРЕДАТЬ ПАТРОНЫ В БЛОКИ
		private void MoveAmmo(IList GunList, int RoF)
		{
			MyFixedPoint CumLoad = (RoF * ShootTime);

			foreach (IMyFunctionalBlock b in GunList)
			{
				var inventory = b.GetInventory();
				var slut1 = inventory.GetItemAt(0);
				var slut2 = inventory.GetItemAt(1);

				// ЕСЛИ СЛОТ ПУСТОЙ -> ЗАГРУЗИТЬ СКОЛЬКО НУЖНО
				if (!slut1.HasValue)
				{
					if (b.GetValue<long>("WC_PickAmmo")==1) {
						LoadAmmo(inventory, CumLoad, true); }
					else {
						LoadAmmo(inventory, CumLoad, false); }
				}

				// ЕСЛИ НЕХВАТАЕТ -> ЗАГРУЗИТЬ ОСТАТОК
				else if (!slut1.Value.Amount.Equals(CumLoad))
				{
					if (b.GetValue<long>("WC_PickAmmo")==1){
						LoadAmmo(inventory, CumLoad - slut1.Value.Amount, true);}
					else{
						LoadAmmo(inventory, CumLoad - slut1.Value.Amount, false);}
				}

				// ЕСЛИ БОЛЬШЕ -> УБРАТЬ ИЗ СЛОТА
				if (!slut1.HasValue) continue;
				if (slut1.Value.Amount > CumLoad)
				ReturnAmmo(inventory, slut1.Value.Amount - CumLoad, 0);

				// УБИРАЕМ ИЗ ВТОРОГО СЛОТА ВСЮ ХУЙНЮ
				if (!slut2.HasValue) continue;
				if (slut2.Value.Amount > 0)
				ReturnAmmo(inventory, slut2.Value.Amount, 1);

			continue;
			}


		}

		public void LoadAmmo(IMyInventory inventory, MyFixedPoint amount, bool AP)
		{
			inventory.GetAcceptedItems(ammo);

			string itemstring = "";
			foreach (var item1 in ammo)
			{
				if (item1.ToString().Contains("AP"))
				{
					ammo.Remove(item1);
					break;
				}
				itemstring = item1.SubtypeId;
			}

			var item = src_().FindItem(MyItemType.MakeAmmo(itemstring));
			var AP9item = src_().FindItem(MyItemType.MakeAmmo("Series900MagazineAP"));
			var AP3item = src_().FindItem(MyItemType.MakeAmmo("Series300MagazineAP"));

			if (item == null) return;
			if (AP9item == null || AP3item == null) return;
			if (!AP)
			{
				src_().TransferItemTo(inventory, item.Value, amount);
			}
			else
			{
				src_().TransferItemTo(inventory, (MyInventoryItem)AP9item, amount);
				src_().TransferItemTo(inventory, (MyInventoryItem)AP3item, amount);
			}
			ammo.Clear();
		}

		public void ReturnAmmo(IMyInventory inventory, MyFixedPoint amount, int index)
		{
			src_().TransferItemFrom(inventory, index, null, null, amount);
		}

		public int FindAmmo(string ammoID)
		{
			var item = src_().FindItem(MyItemType.MakeAmmo(ammoID));
			int amount;
			if (item != null) 
			{
				amount = (int)item.Value.Amount;
				return amount;
			}
			return 0;


		}

		public IMyInventory src_()
		{
			var Container = (IMyCargoContainer)GridTerminalSystem.GetBlockWithName(AmmoInventory);
			//var src = (MyInventory)Container.GetInventory();



            return (MyInventory)Container.GetInventory();

		}

		public void SetConfigurator()
		{
			//string AmmoInventory = Me.CustomData = "ABC";
		}

		internal void Print(object message)
		{Echo(Convert.ToString(message));}

		public void Main(string args)
		{
			Print
			(
			$"SHOOT TIME: {ShootTime}\n" +
			$"DELAY: {Delay}\n\n" + 
			"Available ammo:\n" +
			$"900 = | HE-{FindAmmo("Series900Magazine")} | AP-{FindAmmo("Series900MagazineAP")} |\n" + 
			$"300 = | HE-{FindAmmo("Series300Magazine")} | AP-{FindAmmo("Series300MagazineAP")} |\n" +
			$"CIWS = {FindAmmo("CIWSMagazine")}\n" +
			$"Heavy Gatling = {FindAmmo("Vulcan20x102")}\n"
			);

			SetAmmo();
		}

		public void Save()
		{
		// вот здесь будет инфа для кастом даты
		}

		public void Program() 
		{
			Runtime.UpdateFrequency = UpdateFrequency.Update100;
			SetConfigurator();
			SetTurrets();

		}





	// ...UNICODE1...

	}
}
