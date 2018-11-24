using UnityEngine;
using UnityEditor;

public class ObjectLaider : EditorWindow
{
    GameObject parent;
	GameObject baseObject;
    Vector2 scale = Vector2.one;
    Vector2 interval = Vector2.zero;

	[MenuItem("MyTools/Object Laider")]
	static void Init()
	{
		EditorWindow.GetWindow<ObjectLaider>(true, "Object Laider");
	}

	void OnSelectionChange()
	{
		if (Selection.gameObjects.Length > 0) baseObject = Selection.gameObjects[0];
		Repaint();
	}

	void OnGUI()
	{
		try
		{
            parent = EditorGUILayout.ObjectField("Parent", parent, typeof(GameObject), true) as GameObject;
			baseObject = EditorGUILayout.ObjectField("Object", baseObject, typeof(GameObject), true) as GameObject;

            scale = EditorGUILayout.Vector2Field("Scale", scale);
            interval = EditorGUILayout.Vector2Field("Interval", interval);

			GUILayout.Label("", EditorStyles.boldLabel);
			if (GUILayout.Button("Create")) Create();
		}
		catch (System.FormatException) { }
	}

	void Create()
	{
        if (baseObject != null)
        {
            GameObject parentObject;

            if (parent != null)
            {
                if(parent.Equals(baseObject))
                {
                    return;
                }

                parentObject = parent;
            }
            else
            {
                parentObject = new GameObject("NewParent");
            }

            for (int i = 0; i < scale.x; ++i)
            {
                for (int j = 0; j < scale.y; ++j)
                {
                    Vector3 pos = new Vector3(i*(baseObject.transform.localScale.x+interval.x), 0,j*(baseObject.transform.localScale.y+interval.y));
                    GameObject obj = Instantiate(baseObject, Vector3.zero, Quaternion.identity) as GameObject;
                    obj.name = baseObject.name;
                    obj.transform.parent = parentObject.transform;
                    obj.transform.localPosition = pos;
                    Undo.RegisterCreatedObjectUndo(obj, "Object Laider");
                }
            }
        }
	}
}