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

		// формула = (Кол-во патронов X Время стрельбы)/Кол-во пушек

		// ^ удалить до публикации

		// ...UNICODE1...

		string AmmoInventory = "[Cargo-Ammo]"; // Название инвентаря, в котором должны быть патроны для распределения | Name of inventory for ammo sorting
		string GunsDisplay = "[LCD-Ammo-Guns]"; // Название LCD дисплея с информацией о пушках | Name of LCD display for guns info
		string AmmoDisplay = "[LCD-Ammo-Ammo]"; // Название LCD дисплея с информацией о патронах в инвентаре | Name of LCD display for ammo info in inventory
		int ShootTime = 1; // Время стрельбы (в минутах) | Time of shooting (in minutes)




		List<IMyTerminalBlock> Gats = new List<IMyTerminalBlock>(); // guns
		List<IMyLargeMissileTurret> Arts = new List<IMyLargeMissileTurret>();

		private void GetTurrets()
		{
			GridTerminalSystem.GetBlocksOfType<IMyLargeGatlingTurret>(Gats, block => !block.CubeGrid.IsSameConstructAs(Me.CubeGrid));
			foreach (var block in Gats) { var g = block.GetInventory(); MoveAmmoBlyat(g); }
			GridTerminalSystem.GetBlocksOfType(Arts, block => !block.CubeGrid.IsSameConstructAs(Me.CubeGrid));


		}

		private void SetLCD()
		{
			var GunsLCD = (IMyTextPanel)GridTerminalSystem.GetBlockWithName(GunsDisplay);
			var AmmoLCD = (IMyTextPanel)GridTerminalSystem.GetBlockWithName(AmmoDisplay);
			var Container = (IMyCargoContainer)GridTerminalSystem.GetBlockWithName(AmmoInventory);

			Echo("Я ЗАЕБАЛСЯ ДЕЛАТЬ ЭТУ ХУЙНЮ");

			// AMMO LCD
			AmmoLCD.WriteText(Convert.ToString(
			$"{Container.DisplayNameText} - inventory\n\n"));


			// GUNS LCD
			GunsLCD.WriteText(Convert.ToString(
			$"{ShootTime} min - time of shooting\n" +
			"Script searches for guns ONLY on Subgrids \n(connected via Connector)\n" +
			"AMOUNT | NAME \n\n" +

			$"{Gats.Count} | Gatling turrets\n" +
			$"{Arts.Count} | Artillery turrets\n"));


		}

		private void MoveAmmoBlyat(IMyInventory dest)
		{
			var Container = (IMyCargoContainer)GridTerminalSystem.GetBlockWithName(AmmoInventory);

			MoveInventory(Container.GetInventory(), dest);
		}

		private static void MoveInventory(IMyInventory src, IMyInventory dest)
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
			SetLCD();


		}

	}
}