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



string AmmoInventory = "[Cargo-Ammo]"; // Название инвентаря, в котором должны быть патроны для распределения (Станд. - [Cargo-Ammo])
string APtag = "[AP]"; // Тег для разметки орудий, использующих AP боеприпасы. Нужно вписать в название орудия. (Станд. - [AP]) 
int ShootTime = 1; // Время стрельбы (в минутах) (Станд. - 1)
bool Delay = true; // Исскуственная задержка на 3 секунды, нужная для оптимизации. Может быть отключена, установив значение false (Станд. - true)




// NO EDIT BELOW.
// OR ELSE I WILL NUKE PAKISTANIAN REBELS.


List<IMyTerminalBlock> Gatling = new List<IMyTerminalBlock>();
List<IMyTerminalBlock> RocketLauncher = new List<IMyTerminalBlock>();
List<IMyTerminalBlock> Artillery = new List<IMyTerminalBlock>();
List<IMyTerminalBlock> Autocannon = new List<IMyTerminalBlock>();

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
List<IMyConveyorSorter> Cannonsentry = new List<IMyConveyorSorter>();
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
GridTerminalSystem.GetBlocksOfType(Cannonsentry, a => a.BlockDefinition.SubtypeId.Equals("SG_RocketSentry"));
GridTerminalSystem.GetBlocksOfType(Autocannon, a => a.BlockDefinition.SubtypeId.Equals("AutoCannonTurret"));
GridTerminalSystem.GetBlocksOfType<IMyLargeGatlingTurret>(Gatling);
GridTerminalSystem.GetBlocksOfType<IMyLargeMissileTurret>(Artillery);
GridTerminalSystem.GetBlocksOfType<IMySmallMissileLauncher>(RocketLauncher); 
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

private void MoveAmmo(IList GunList, int RoF)
{
	MyFixedPoint CumLoad = (RoF * ShootTime);

	foreach (IMyFunctionalBlock b in GunList)
	{
		var inventory = b.GetInventory();
		var slut = inventory.GetItemAt(0); 

		if (!slut.HasValue) 
		{ 
			if(!b.CustomName.Contains(APtag)){
				LoadAmmo(inventory, (int)CumLoad);}
			else{
				LoadAP(inventory, (int)CumLoad);}
		}
		else if (!slut.Value.Amount.Equals(CumLoad))
		{
			if(!b.CustomName.Contains(APtag)){
				LoadAmmo(inventory, (int)CumLoad - (int)slut.Value.Amount);}
			else{
				LoadAP(inventory, (int)CumLoad - (int)slut.Value.Amount);}
		}
	continue;
	}
}

public void LoadAmmo(IMyInventory dest, int amount)
{
var Container = (IMyCargoContainer)GridTerminalSystem.GetBlockWithName(AmmoInventory);
var src = Container.GetInventory();

for (int i = 0; i < src.ItemCount; i++)
{
	var item = src.GetItemAt(i);
	if (!item.HasValue) continue;
	if (item.Value.Type.TypeId.Equals("MyObjectBuilder_AmmoMagazine") & 
		!item.Value.Type.SubtypeId.Contains("AP")){ // to check if item is actually an ammo before trying to put it into container
		src.TransferItemTo(dest, item.Value, amount);
	}
}

}

public void LoadAP(IMyInventory dest, int amount)
{
	var Container = (IMyCargoContainer)GridTerminalSystem.GetBlockWithName(AmmoInventory);
	var src = Container.GetInventory();

	var AP9item = src.FindItem(MyItemType.MakeAmmo("Series900MagazineAP"));
	var AP3item = src.FindItem(MyItemType.MakeAmmo("Series300MagazineAP"));
	if  (AP9item.HasValue && AP3item.HasValue){
			src.TransferItemTo(dest, (MyInventoryItem)AP9item, amount);
			src.TransferItemTo(dest, (MyInventoryItem)AP3item, amount);
		}
}

public void SetConfigurator()
{
//string AmmoInventory = Me.CustomData = "ABC";
}

internal void Console(object message)
{Echo(Convert.ToString(message));}

public void Main(string args)
{
	Console
	(
		"Sowwy, doesn't work rn /shrug\n\n" +
		$"SHOOT TIME: {ShootTime}\n" +
		$"DELAY: {Delay}\n" + 
		"\n" +
		"Available ammo:\n" +
		$"900 = | HE-0 | AP-0 |\n" + // вот тут добавить список доступных ХЕ и АП
		$"300 = | HE-0 | AP-0 |\n"   // ^^^
		// $"CIWS =\n" +
		// $"Artillery =\n" +
		// $"Cannon Sentry =\n"
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