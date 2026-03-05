using UnityEditor;
using UnityEngine;

public static class OverdriveCustomActions
{
    [MenuItem("Tools/Overdrive Actions/Edit Selected (Mode)")]
    public static void EditSelectedCycleToolModes()
    {
        // This is a shortcut, must use shortcut simulator
        Overdrive.ShortcutSimulator.SimulateShortcut("Tools/Cycle Tool Modes");
    }

    [MenuItem("Tools/Overdrive Actions/Exit Editing (Mode)")]
    public static void ExitEditingEnterGameObjectMode()
    {
        // This is a shortcut, must use shortcut simulator
        Overdrive.ShortcutSimulator.SimulateShortcut("Tools/Enter GameObject Mode");
    }

    [MenuItem("Tools/Overdrive Actions/Toggle Gizmos")]
    public static void ToggleGizmos()
    {
        SceneView sceneView = SceneView.lastActiveSceneView;
        if (sceneView != null)
        {
            sceneView.drawGizmos = !sceneView.drawGizmos;
            sceneView.Repaint();
        }
    }

    [MenuItem("Tools/Overdrive Actions/Toggle Wireframe")]
    public static void ToggleWireframe()
    {
        SceneView sceneView = SceneView.lastActiveSceneView;
        if (sceneView != null)
        {
            sceneView.cameraMode = sceneView.cameraMode.drawMode == DrawCameraMode.Wireframe
                ? SceneView.GetBuiltinCameraMode(DrawCameraMode.Textured)
                : SceneView.GetBuiltinCameraMode(DrawCameraMode.Wireframe);
            sceneView.Repaint();
        }
    }

    [MenuItem("Tools/Overdrive Actions/Toggle Isolation")]
    public static void ToggleIsolation()
    {
        bool isInSceneIsolation = SceneVisibilityManager.instance.IsCurrentStageIsolated() &&
                                  UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() == null;

        if (isInSceneIsolation)
        {
            SceneVisibilityManager.instance.ExitIsolation();
        }
        else
        {
            GameObject[] selectedObjects = Selection.gameObjects;
            if (selectedObjects.Length > 0)
            {
                SceneVisibilityManager.instance.Isolate(selectedObjects, true);
            }
        }
    }
}
