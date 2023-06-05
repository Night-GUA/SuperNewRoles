using System.Collections.Generic;
using UnityEngine;
using SuperNewRoles.Mode;
using SuperNewRoles.Patches;
using static SuperNewRoles.Modules.CustomOption;
using static SuperNewRoles.Modules.CustomOptionHolder;

namespace SuperNewRoles.Roles.Impostor.MadRole;

public static class MadRaccoon
{
    internal static class CustomOptionData
    {
        private static int optionId = 1321;
        public static CustomRoleOption Option;
        public static CustomOption PlayerCount;
        public static CustomOption IsCheckImpostor;
        public static CustomOption IsSettingNumberOfUniqueTasks;
        public static CustomOption CommonTask;
        public static CustomOption ShortTask;
        public static CustomOption LongTask;
        public static CustomOption IsParcentageForTaskTrigger;
        public static CustomOption ParcentageForTaskTriggerSetting;
        public static CustomOption IsUseVent;
        public static CustomOption IsImpostorLight;
        public static CustomOption ShapeshifterCooldown;
        public static CustomOption ShapeshifterDuration;

        public static void SetupCustomOptions()
        {
            Option = SetupCustomRoleOption(optionId, true, RoleId.MadRaccoon); optionId++;
            PlayerCount = Create(optionId, true, CustomOptionType.Crewmate, "SettingPlayerCountName", CrewPlayers[0], CrewPlayers[1], CrewPlayers[2], CrewPlayers[3], Option); optionId++;
            IsUseVent = Create(optionId, true, CustomOptionType.Crewmate, "MadmateUseVentSetting", false, Option); optionId++;
            IsImpostorLight = Create(optionId, true, CustomOptionType.Crewmate, "MadmateImpostorLightSetting", false, Option); optionId++;
            IsCheckImpostor = Create(optionId, true, CustomOptionType.Crewmate, "MadmateIsCheckImpostorSetting", false, Option); optionId++;
            IsSettingNumberOfUniqueTasks = Create(optionId, false, CustomOptionType.Crewmate, "IsSettingNumberOfUniqueTasks", true, IsCheckImpostor); optionId++;
            var taskOption = SelectTask.TaskSetting(optionId, optionId + 1, optionId + 2, IsSettingNumberOfUniqueTasks, CustomOptionType.Crewmate, true); optionId += 3;
            CommonTask = taskOption.Item1;
            ShortTask = taskOption.Item2;
            LongTask = taskOption.Item3;
            IsParcentageForTaskTrigger = Create(optionId, false, CustomOptionType.Crewmate, "IsParcentageForTaskTrigger", true, IsCheckImpostor); optionId++;
            ParcentageForTaskTriggerSetting = Create(optionId, false, CustomOptionType.Crewmate, "ParcentageForTaskTriggerSetting", rates4, IsParcentageForTaskTrigger); optionId++;
            ShapeshifterCooldown = Create(optionId, true, CustomOptionType.Crewmate, "DoppelgangerCooldownSetting", 5f, 5f, 60f, 2.5f, Option); optionId++;
            ShapeshifterDuration = Create(optionId, true, CustomOptionType.Crewmate, "DoppelgangerDurationTimeSetting", 90f, 0f, 250f, 5f, Option);
        }
    }

    internal static class RoleClass
    {
        public static List<PlayerControl> Player;
        public static Color32 color = Roles.RoleClass.ImpostorRed;
        public static bool IsUseVent;
        public static bool IsImpostorLight;
        public static bool IsImpostorCheck;
        public static int ImpostorCheckTask;
        public static float ShapeshifterCooldown;
        public static float ShapeshifterDuration;
        public static void ClearAndReload()
        {
            Player = new();

            IsUseVent = CustomOptionData.IsUseVent.GetBool();
            IsImpostorLight = CustomOptionData.IsImpostorLight.GetBool();
            IsImpostorCheck = CustomOptionData.IsCheckImpostor.GetBool() && !ModeHandler.IsMode(ModeId.SuperHostRoles);

            bool IsFullTask = !CustomOptionData.IsSettingNumberOfUniqueTasks.GetBool();
            int AllTask = SelectTask.GetTotalTasks(RoleId.Worshiper);
            ImpostorCheckTask = ModeHandler.IsMode(ModeId.SuperHostRoles) ? 0 : IsFullTask ? AllTask : (int)(AllTask * (int.Parse(CustomOptionData.ParcentageForTaskTriggerSetting.GetString().Replace("%", "")) / 100f));

            ShapeshifterCooldown = CustomOptionData.ShapeshifterCooldown.GetFloat();
            ShapeshifterDuration = CustomOptionData.ShapeshifterDuration.GetFloat();
        }
    }
}