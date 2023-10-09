using UnityEditor;
using UnityEngine;

namespace DialogSystem.Editor
{
    [CustomEditor(typeof(DialogueResponseEvents))]
    public class DialogueResponseEventsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var responseEvents = (DialogueResponseEvents)target;

            if (GUILayout.Button("Refresh"))
            {
                responseEvents.OnValidate();
            }
        }
    }
}