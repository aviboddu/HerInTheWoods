using System;
using System.Diagnostics;
using Godot;

namespace Utilities;

public static class Miscellaneous
{
    private static readonly Lazy<TimeSpan> LocalUtcOffset = new(() => TimeZoneInfo.Local.GetUtcOffset(DateTime.Now));

    // Node is valid.
    public static bool IsValid<T>(this T node) where T : GodotObject
    {
        return node is not null
               && GodotObject.IsInstanceValid(node)
               && !node.IsQueuedForDeletion();
    }

    // Same as connect but doesn't throw an error if trying to connect a duplicate.
    public static Error CheckedConnect<T>(this T node, in StringName signal, in Callable callable, uint flags = 0)
        where T : Node
    {
        if (!node.IsConnected(signal, callable)
            || ((GodotObject.ConnectFlags)flags).HasFlag(GodotObject.ConnectFlags.ReferenceCounted))
            return node.Connect(signal, callable, flags);
        return Error.InvalidParameter;
    }

    // Crash the game with correct notifications and exit codes. https://docs.godotengine.org/en/stable/tutorials/inputs/handling_quit_requests.html
    public static void Crash<T>(this T tree, ExitCodes exitCode) where T : SceneTree
    {
        Debug.Assert(exitCode > ExitCodes.OK, $"SceneTree.Crash({exitCode}) - exitCode must be greater than 0");
        Debug.Assert((int)exitCode < 256,
                     $"SceneTree.Crash({exitCode}) - exitCode must be less than 256 for compatibility purposes");
        tree.Root.PropagateNotification((int)Node.NotificationCrash);
        tree.Quit((int)exitCode);
    }

    // Exit the game with correct notifications. https://docs.godotengine.org/en/stable/tutorials/inputs/handling_quit_requests.html
    public static void Exit<T>(this T tree) where T : SceneTree
    {
        tree.Root.PropagateNotification((int)Node.NotificationWMCloseRequest);
        tree.Quit();
    }

    // A faster Now method which doesn't adjust for local time during the game.
    // Which makes our logs easier to understand if they happen in between daylight savings as well.
    public static DateTime FastNow()
    {
        return DateTime.SpecifyKind(DateTime.UtcNow + LocalUtcOffset.Value, DateTimeKind.Local);
    }
}
