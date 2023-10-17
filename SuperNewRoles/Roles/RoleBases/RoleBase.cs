using System;
using System.Collections;
using System.Linq;
using SuperNewRoles.Roles.Role;
using SuperNewRoles.Roles.RoleBases.Interfaces;
using UnityEngine;

namespace SuperNewRoles.Roles.RoleBases;
public abstract class RoleBase : IDisposable
{
    public static int MaxInstanceId = int.MinValue;
    public PlayerControl Player { get; private set; }
    //比較用なので各クライアントごとに違ってもOK
    //逆に比較用以外に使うな
    private int InstanceId { get; }
    public readonly RoleInfo Roleinfo;
    public readonly OptionInfo Optioninfo;
    public readonly IntroInfo Introinfo;
    //後で処理書く
    public RoleBase(RoleInfo roleInfo, OptionInfo optionInfo, IntroInfo introInfo)
    {
        InstanceId = MaxInstanceId;
        MaxInstanceId++;
        this.Roleinfo = roleInfo;
        this.Optioninfo = optionInfo;
        this.Introinfo = introInfo;
    }
    //Disposeを実装
    public void Dispose()
    {

    }
    public void SetPlayer(PlayerControl player)
    {
        Player = player;
    }
    public AudioClip GetIntroAudioClip()
    {
        if (this is ICustomIntroSound customIntroSound)
            return customIntroSound.IntroSound;
        return RoleManager.Instance.GetRole(
                Introinfo.IntroSound
            )?.IntroSound;
    }
    //比較を時前にして高速化
    public override bool Equals(object obj)
    {
        if (obj is not RoleBase)
            return false;
        return this.GetHashCode() == obj.GetHashCode();
    }
    public override int GetHashCode()
    {
        return InstanceId;
    }
}