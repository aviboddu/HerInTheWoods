using Godot;

namespace Utilities;

public partial class SceneManager : Node
{
    private const string LOADING_SCREEN_PATH = "res://ui/loading_screen/LoadingScreen.tscn";
    private static readonly PackedScene LoadingScreen = GD.Load<PackedScene>(LOADING_SCREEN_PATH);

    private static SceneManager _instance;

    // This should always be the scene we want to load.
    public static string DesiredScene { get; private set; }

    public override void _EnterTree()
    {
        if (_instance is not null)
        {
            QueueFree();
            return;
        }

        _instance = this;
        base._EnterTree();
    }

    public static void ChangeScene(in string sceneToLoad)
    {
        _instance._changeScene(sceneToLoad);
    }

    private void _changeScene(in string sceneToLoad)
    {
        Logger.WriteDebug($"SceneManager::_changeScene({sceneToLoad}) - Loading {sceneToLoad}");
        DesiredScene = sceneToLoad;
        GetTree().ChangeSceneToPacked(LoadingScreen);
    }
}
