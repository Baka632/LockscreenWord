using Microsoft.Toolkit.Uwp.Notifications;

namespace LockscreenWord.Common;

public sealed class TileCreationHelper
{
    public static TileContent CreateLockscreenWordTile(string lockDetailedStatus1, string lockDetailedStatus2 = "", string lockDetailedStatus3 = "")
    {
        TileContentBuilder builder = new();
        builder.AddTile(TileSize.Wide)
               .AddText("正在显示以下内容：")
               .AddText(lockDetailedStatus1)
               .AddText(lockDetailedStatus2)
               .AddText(lockDetailedStatus3);

        builder.Content.Visual.LockDetailedStatus1 = lockDetailedStatus1;
        builder.Content.Visual.LockDetailedStatus2 = lockDetailedStatus2;
        builder.Content.Visual.LockDetailedStatus3 = lockDetailedStatus3;

        return builder.GetTileContent();
    }
}
