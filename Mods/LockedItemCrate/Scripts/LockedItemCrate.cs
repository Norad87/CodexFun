using System;
using HarmonyLib;

public class LockedItemCrateMod : IModApi
{
    public void InitMod(Mod _modInstance)
    {
        var harmony = new Harmony("com.codex.lockeditemcrate");
        harmony.PatchAll();
    }
}

[HarmonyPatch(typeof(TileEntityLootContainer), "TryAddItem")]
public class LockedItemCrate_TryAddItem_Patch
{
    private const string LockedItemKey = "LockedItemCrateItem";

    public static bool Prefix(TileEntityLootContainer __instance, ItemStack _itemStack, ref bool __result)
    {
        if (_itemStack == null || _itemStack.IsEmpty())
        {
            return true;
        }

        if (!IsLockedItemCrate(__instance))
        {
            return true;
        }

        var lockedItemName = GetLockedItemName(__instance);
        if (string.IsNullOrEmpty(lockedItemName))
        {
            lockedItemName = _itemStack.itemValue.ItemClass.GetItemName();
            SetLockedItem(__instance, lockedItemName, _itemStack.itemValue.ItemClass.GetLocalizedItemName());
            return true;
        }

        if (!string.Equals(lockedItemName, _itemStack.itemValue.ItemClass.GetItemName(), StringComparison.OrdinalIgnoreCase))
        {
            __result = false;
            return false;
        }

        return true;
    }

    private static bool IsLockedItemCrate(TileEntityLootContainer container)
    {
        var block = container.blockValue.Block;
        return block.Properties.Values.ContainsKey("LockItemToFirstStack")
               && block.Properties.Values["LockItemToFirstStack"].Equals("true", StringComparison.OrdinalIgnoreCase);
    }

    private static string GetLockedItemName(TileEntityLootContainer container)
    {
        if (container.persistentData.TryGetValue(LockedItemKey, out var value))
        {
            return value as string;
        }

        return string.Empty;
    }

    private static void SetLockedItem(TileEntityLootContainer container, string itemName, string displayName)
    {
        container.persistentData[LockedItemKey] = itemName;

        if (container.blockValue.Block.Properties.Values.ContainsKey("NameOnLockedItem"))
        {
            container.SetCustomName(displayName);
            container.SetModified();
        }
    }
}
