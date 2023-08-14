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
//using Sandbox.ModAPI;
//using SpaceEngineers.Game.ModAPI.Ingame;

namespace dolboeb
{
	public class MyClass : MyGridProgram
	{

		// идея следующая - патроны кладутся в оперделенный ящик
		// задается параметр "время стрельбы (в минутах)"
		// патроны распределяются по кораблям 
		// 2 лсд, на одном сколько пушек, на втором обнаруженные гриды
		// может быть добавить кнопочную панель чтобы включать распределение? -боже чтоже ты натворил еблан

		//          ⠀⠀⡰⠀⠀⠀⠀⠀⠀⠀⠢⡀⠀⠀⠀⢫⠙⠲⢤⣀⠀⠀⠀⠀⠀⠀⢀⡔⠀⠀⠀⠀⠀⢆⠀⠀⠀⠀⠀
		//          ⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠙⣄⠀⠀⠘⡆⠀⠀⠉⠛⢦⣀⠀⠀⣰⠋⠀⠀⠀⠀⠀⠀⠸⡄⠀⠀⠀⠀
		//          ⠀⢠⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⢦⠀⠀⣸⠀⠀⠀⠀⠀⠉⠓⢦⡇⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀
		//          ⠀⢸⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣤⠗⠉⠀⠀⠀⠀⠀⠀⠀⠀⠀⠁⠀⠀⠀⠀⠀⠀⠀⠀⢿⠀⠀⠀⠀
		//          ⠀⢸⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⠉⠉⠉⠉⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⠀⠀⠀⠀
		//          ⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣼⠀⠀⠀⠀
		//          ⠀⠀⢧⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀
		//          ⠀⠀⠸⡀⠀⠀⠀⠀⠘⠦⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡴⠋⠀⠀⠀⠀⢸⠁⠀⠀⠀⠀
		//          ⠀⠀⠀⢣⠀⠀⠀⠀⠀⠀⠈⠳⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⠞⠉⠀⠀⠀⠀⠀⠀⢎⣀⣀⣀⣀⠀
		//          ⢯⡉⠙⠒⠒⠀⠀⠀⠀⠀⠀⠀⠈⣙⡦⠀⠀⠀⠀⠀⠀⠀⢰⣞⡁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⡴⠃
		//          ⠀⠙⢦⡀⠀⠀⠀⠀⠀⣀⡤⠖⠋⠁⠀⠀⠀⢀⣀⠀⠀⠀⠀⠈⠙⠳⢦⣄⡀⠀⠀⠀⠀⠀⢠⠞⠁⠀⠀
		//          ⠀⠀⠈⢳⠀⠀⠀⠀⠈⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠁⠀⠀⠀⠀⠀⢻⡀⠀⠀⠀
		//          ⠀⠀⢀⡞⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⣆⠀⠀
		//          ⠀⠀⠾⡤⣤⣤⣄⣀⡀⠀⠀⠀⠀⠀⠀⠐⠉⠉⠳⠶⠋⠀⠀⠀⠀⠀⠀⠀⢀⣠⠆⠀⠤⠤⠴⠶⠋⠀⠀
		//          ⠀⠀⠀⠀⠀⠀⠀⠈⠉⢻⡲⠦⣤⣀⣀⣀⡀⠀⠀⠀⠀⠀⠀⠀⠀⠤⠖⠚⠉⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
		//          ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠳⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⢦⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
		//          ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⣳⠄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⢧⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
		//          ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣾⣁⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⣇⠀⠀⠀⠀⠀⠀⠀⠀⠀
		//          ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⢩⡟⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢹⡄⠀⠀⠀⠀⠀⠀⠀⠀
		//          ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣧⠀⠀⠀⠀⠀⠀⠀⠀
		//          ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠸⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠹⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀


		// --- прогрессия сумашествия

		// короче главная проблема с которой я столкнулся (пока что) это проблема переноса предметво между инвентарями
		// хаха муха в супе
		// я съел две пачки фенибута убиваю себя под нирвану
		// выше я написал(напевал) текст песни, сейчас я походу реально иду за таблетками. код выводит меня блять
		// CERF RNJ LFK T,EXTQ FHNBKKTHBQCRJQ NEHTKB YFPDFYBT MISSILELARGETURRET RFRJQ YF[EQ VBCCFQKS LJK,J`,S
		// сука оно мне снится 
		// кулл стори - когда я жил в двух-этажном доме, то ночью ровно после 11 у меня был принцип не выходить из комнаты потому что это моя сейфзона в которой никто меня не тронет
		// Я НЕНАВИЖУ БЛЯТЬ ЭТУ ИГРУ ЭТОТ СКРИПТ ЭТИ ПЛАГИНЫ НАХУЯ Я ВООБЩЕ ЧТО-ТО ДЕЛАЮ РАДИ КОГО ТО БЛЯТЬ УБЕЙТЕ МЕНЯ НАХУЙ Я НЕНАВИЖУ СВОЮ ЖИЗНЬ НЕНАВИЖУ ВСЕХ КИНЫ ТУПОРЫЛЫЕ УЕБАНЫ НЕ УМЕЮТ НИХУЯ АБСОЛЮТНО ДЕЛАТЬ В ЭТОЙ ИГРЕ ДИГЛО ВРЕЙЖ ИДИОТСКОЕ ТУПОЕ ГОВНО БЛЯТЬ ХУЕТА ПОЛНАЯ КИНЫ ИДИТЕ НАХУЙ ВСЕ ИДИТЕ НАХУЙ
		// ХАХАХАХАХАХАХАХХАХАХАХАХХАХАХАХААХ ОНО У МЕНЯ В СТЕНАХ ХАХАХАХА

		// --- конец стенки


		// код проходит две(три(3)) стадии: 1-сделать его рабочим, 2-сделать его удобным, 3-автоматизировать

		// формула = (Кол-во патронов X Время стрельбы)/Кол-во пушек
		// возможно кол-во патронов не нужно? и тогда будет Время стрельбы x rate of fire

		// ИНВЕНТАРЬ НЕ ПЕРЕДАЕТ ЕСЛИ ОН ПЕРЕЗАПОЛНЯЕТ

		// ^ удалить до публикации


		// ...UNICODE1...

		// Чтобы скрипт работал как нужно: 
		// 1) Поставьте контейнер(ванильный), подключенный к общей системе конвееров
		// 2) Назовите его также, как установлено в переменной "AmmoInventory"
		// 3) Установите требуемое время стрельбы в переменной "ShootTime" (формула для распределения Темп стрельбы * ShootTime)
		// 4) Положите в контейнер максимальное (скрипт не дозаряжает пушки в случае нехватки боеприпасов) количество подходящих боеприпасов (Gatling ammo box, 300mm HE Shells и т.д.)
		// 5) Запустите скрипт
		// Скрипт продолжит работу в фоновом режиме. Отключите програмный блок чтобы выключить скрипт.

		string AmmoInventory = "[Cargo-Ammo]"; // Название инвентаря, в котором должны быть патроны для распределения
		int ShootTime = 2; // Время стрельбы (в минутах)

















		// NO EDIT BELOW.
		// OR ELSE I WILL NUKE PAKISTANIAN REBELS.



		




		List<IMyConveyorSorter> Archer = new List<IMyConveyorSorter>(); 
		List<IMyTerminalBlock> Gatling = new List<IMyTerminalBlock>(); 
		List<IMyConveyorSorter> Crossbow = new List<IMyConveyorSorter>();
		List<IMyConveyorSorter> Longbow = new List<IMyConveyorSorter>();
		List<IMyConveyorSorter> Ballista = new List<IMyConveyorSorter>();
		List<IMyConveyorSorter> Elephant = new List<IMyConveyorSorter>();
		List<IMyConveyorSorter> Rhino = new List<IMyConveyorSorter>();
		List<IMyConveyorSorter> Mammoth = new List<IMyConveyorSorter>();
		List<IMyConveyorSorter> Mastadon = new List<IMyConveyorSorter>();
		List<IMyConveyorSorter> CIWS = new List<IMyConveyorSorter>();
		List<IMyConveyorSorter> FLAK = new List<IMyConveyorSorter>();
		List<IMyTerminalBlock> Artillery = new List<IMyTerminalBlock>();		

		private void Unload()
		{
			// ¯\_(:3)_/¯
		}

		public void SetTurrets()
		{
			GridTerminalSystem.GetBlocksOfType(Archer, a => a.BlockDefinition.SubtypeId.Equals("Series300SingleBarrel"));
			GridTerminalSystem.GetBlocksOfType(Longbow, a => a.BlockDefinition.SubtypeId.Equals("Series300DoubleBarrel"));
			GridTerminalSystem.GetBlocksOfType(Ballista, a => a.BlockDefinition.SubtypeId.Equals("Series300TripleBarrel"));
			GridTerminalSystem.GetBlocksOfType(Rhino, a => a.BlockDefinition.SubtypeId.Equals("Series900SingleBarrel"));
			GridTerminalSystem.GetBlocksOfType(Mammoth, a => a.BlockDefinition.SubtypeId.Equals("Series900DoubleBarrel"));
			GridTerminalSystem.GetBlocksOfType(Mastadon, a => a.BlockDefinition.SubtypeId.Equals("Series900TripleBarrel"));
			GridTerminalSystem.GetBlocksOfType(CIWS, a => a.BlockDefinition.SubtypeId.Equals("CIWS"));
			GridTerminalSystem.GetBlocksOfType(FLAK, a => a.BlockDefinition.SubtypeId.Equals("Bofors"));
			GridTerminalSystem.GetBlocksOfType(Crossbow, a => a.BlockDefinition.SubtypeId.Equals("Series300SideCannon"));
			GridTerminalSystem.GetBlocksOfType(Elephant, a => a.BlockDefinition.SubtypeId.Equals("Series900SideCannon"));
			GridTerminalSystem.GetBlocksOfType<IMyLargeGatlingTurret>(Gatling);
			GridTerminalSystem.GetBlocksOfType<IMyLargeMissileTurret>(Artillery);


		}

		public void SetAmmo()
		{
			MoveAmmo(Gatling, 4);
			MoveAmmo(Artillery, 10);

			MoveAmmoWC(Crossbow, 13);
			MoveAmmoWC(Archer, 13);
			MoveAmmoWC(Longbow, 25);
			MoveAmmoWC(Ballista, 35);

			MoveAmmoWC(Rhino, 9);
			MoveAmmoWC(Mammoth, 17);
			MoveAmmoWC(Mastadon, 20);
			MoveAmmoWC(Elephant, 9);

			MoveAmmoWC(CIWS, 2);
			MoveAmmoWC(FLAK, 12);

		}




		private void MoveAmmoWC(List<IMyConveyorSorter> GunList, int RoF)
		{
			int CumLoad = RoF * ShootTime;

			foreach (var b in GunList)
			{
				var inventory = b.GetInventory();
				if (inventory.GetItemAt(0) != null) continue;
				{ MoveInventory(inventory, CumLoad); }
			}
		}

		private void MoveAmmo(List<IMyTerminalBlock> GunList, int RoF)
		{
			int CumLoad = RoF * ShootTime ;

			foreach (var b in GunList)
			{
				var inventory = b.GetInventory();
				if (inventory.GetItemAt(0) != null) continue;
				{ MoveInventory(inventory, CumLoad); }

			}
		}

		private void MoveInventory(IMyInventory dest, int amount)
		{
			var Container = (IMyCargoContainer)GridTerminalSystem.GetBlockWithName(AmmoInventory);
			var src = Container.GetInventory();

			for (int i = 0; i < src.ItemCount; i++)
			{
				var item = src.GetItemAt(i);
				if (!item.HasValue) continue;
				if (dest.CanItemsBeAdded(item.Value.Amount, item.Value.Type))
				{
					src.TransferItemTo(dest, item.Value, amount);
				}
			}
		}

		private void Main(string args)
		{
			Runtime.UpdateFrequency = UpdateFrequency.Update100;
			SetTurrets();
			SetAmmo();

			Echo("Done");
			if (args == "Unload") { Unload(); } // bro this is most complicated line ever


		}

		 // ...UNICODE1...

	}
}