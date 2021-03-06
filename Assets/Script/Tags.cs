﻿public static class Tags
{
    public const string player = "Player";
    public const string mainCamera = "MainCamera";
    public const string mainLight = "MainLight";
    public const string enemy = "Enemy";
    public const string terrain = "Terrain";
    public const string NPC = "NPC";
    public const string worktop = "Worktop";
    public const string boss = "Boss";
    //UI:
    public const string UIRoot = "UIRoot";

    //Position
    public const string UpPosition = "UpPosition";
    public const string DownPosition = "DownPosition";
    public const string PortalPosition = "PortalPosition";

}
public static class SceneName
{
    public const string StartMenu = "Start";
    public const string Opening = "Opening";
    public const string Town = "Town";
    public const string FirstFloor = "FirstFloor";
    public const string LastFloor = "LastFloor";
    public const string BossFloor = "BossFloor";
    public const string ShopFloor = "ShopFloor";
    public const string RandomMapFloor = "RandomFloor";

}
public static class SceneFloor
{
    public const int Town = 0;
    public const int FirstFloor = -1;
    public const int LastFloor = -10;
    public const int ShopFloor = -11;
}
public enum SceneFloorInfo
{
    Town=0, FirstFloor = -1, LastFloor = -10, ShopFloor = -11, BossFloor=-9, RandomMapFloor=-1000
}

public static class AudioName
{
    public const string coin = "coin";
    public const string gift = "gift";

    public const string hurt = "Hurt";

    public const string attack1 = "attack1";
    public const string attack2 = "attack2";
    public const string attack3 = "attack3";

    public const string skill1 = "bird";
    public const string skill2 = "ice_attack";
    public const string skill3_1 = "skill3_1";
    public const string skill3_2 = "skill3_2";
    public const string skill3_3 = "skill3_3";

    public const string compose = "make";

}
