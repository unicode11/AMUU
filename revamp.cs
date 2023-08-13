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

		// ^ удалить до публикации

		// формула = (Кол-во патронов X Время стрельбы)/Кол-во пушек

		// ...UNICODE1...

		string AmmoInventory = "[Cargo-Ammo]"; // Название инвентаря, в котором должны быть патроны для распределения | Name of inventory for ammo sorting
		//int ShootTime = 1; // Время стрельбы (в минутах) | Time of shooting (in minutes)


		public class YoGun // yo mama
		{
			public string Name = "", Id = "";
			public int Count = 0;
		}


		private void GetTurrets()
		{
			List<IMyLargeGatlingTurret> Gats = new List<IMyLargeGatlingTurret>();
			List<IMyLargeMissileTurret> Arts = new List<IMyLargeMissileTurret>();



			GridTerminalSystem.GetBlocksOfType(Gats);
			GridTerminalSystem.GetBlocksOfType(Arts, block => !block.CubeGrid.IsSameConstructAs(Me.CubeGrid));
			
			foreach(var b in Gats)
			{
				MoveAmmo(b.GetInventory());
			}


		}

		private void MoveAmmo(IMyInventory dest)
		{
			var Container = (IMyCargoContainer)GridTerminalSystem.GetBlockWithName(AmmoInventory);

			MoveInventory(Container.GetInventory(), dest);
		}

		private void MoveInventory(IMyInventory src, IMyInventory dest)
		{
			for (int i = 0; i < src.ItemCount; i++)
			{
				var item = src.GetItemAt(i);
				if (!item.HasValue) continue;
				// the destination actually has space...
				if (dest.CanItemsBeAdded(item.Value.Amount, item.Value.Type))
				{
					src.TransferItemTo(dest, item.Value, null);
				}
			}
		}

		private void Main()
		{
			Runtime.UpdateFrequency = UpdateFrequency.Update100;


			// name, rate of fire
			var Gun = new Dictionary<string, int>
			{
				{ "300-1", 13 },
				{ "300-2", 25 },
				{ "300-3", 35 },
				{ "900-1", 9 },
				{ "900-2", 17 },
				{ "900-3", 25 },
				{ "quad", 12 },
				{ "arty", 10 },
				{ "gatly", 4 }
			};

			// Echo("LCD: " LCDstate +); // доделать интерфейс внутри самого блока 

			GetTurrets();



		}

	}
}