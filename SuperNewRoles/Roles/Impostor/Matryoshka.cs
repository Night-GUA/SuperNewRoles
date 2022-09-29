using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hazel;
using SuperNewRoles.Helpers;
using UnityEngine;

namespace SuperNewRoles.Roles.Impostor
{
    public static class Matryoshka
    {
        public static void FixedUpdate()
        {
            foreach (var Data in RoleClass.Matryoshka.Datas.ToArray())
            {
                if (Data.Value.Item1 != null) continue;
                Data.Value.Item1.Reported = !CustomOptions.MatryoshkaWearReport.GetBool();
                Data.Value.Item1.bodyRenderer.enabled = false;
                Data.Value.Item1.transform.position = ModHelpers.PlayerById(Data.Key).transform.position;
                RoleClass.Matryoshka.Datas[Data.Key] = (Data.Value.Item1, Data.Value.Item2 - Time.fixedDeltaTime);
                if (RoleClass.Matryoshka.Datas[Data.Key].Item2 <= 0) continue;
                if (Data.Key == CachedPlayer.LocalPlayer.PlayerId)
                {
                    Buttons.HudManagerStartPatch.MatryoshkaButton.MaxTimer = CustomOptions.MatryoshkaCoolTime.GetFloat();
                    Buttons.HudManagerStartPatch.MatryoshkaButton.Timer = Buttons.HudManagerStartPatch.MatryoshkaButton.MaxTimer;
                }
                Set(ModHelpers.PlayerById(Data.Key), null, false);
            }
        }
        public static void WrapUp()
        {
            RoleClass.Matryoshka.Datas = new();
        }
        public static void Set(PlayerControl source, PlayerControl target, bool Is)
        {
            if (Is)
            {
                source.setOutfit(target.Data.DefaultOutfit);
            }
            else
            {
                source.setOutfit(source.Data.DefaultOutfit);
            }
            if (!Is)
            {
                if (RoleClass.Matryoshka.Datas[source.PlayerId].Item1 != null)
                {
                    RoleClass.Matryoshka.Datas[source.PlayerId].Item1.Reported = false;
                    if (RoleClass.Matryoshka.Datas[source.PlayerId].Item1.bodyRenderer != null)
                        RoleClass.Matryoshka.Datas[source.PlayerId].Item1.bodyRenderer.enabled = true;
                }
                RoleClass.Matryoshka.Datas[source.PlayerId] = (null, 0);
            }
            else
            {
                DeadBody[] array = UnityEngine.Object.FindObjectsOfType<DeadBody>();
                for (int i = 0; i < array.Length; i++)
                {
                    if (GameData.Instance.GetPlayerById(array[i].ParentId).PlayerId == target.PlayerId)
                    {
                        RoleClass.Matryoshka.Datas[source.PlayerId] = (array[i], RoleClass.Matryoshka.WearDefaultTime);
                    }
                }
            }
        }
        public static void RpcSet(PlayerControl target, bool Is)
        {
            MessageWriter writer = RPCHelper.StartRPC(CustomRPC.SetMatryoshkaDeadbody);
            writer.Write(CachedPlayer.LocalPlayer.PlayerId);
            writer.Write(target == null ? (byte)255 : target.PlayerId);
            writer.Write(Is);
            writer.EndRPC();
            RPCProcedure.SetMatryoshkaDeadBody(CachedPlayer.LocalPlayer.PlayerId, target == null ? (byte)255 : target.PlayerId, Is);
        }
    }
}