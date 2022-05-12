using UnityEditor;
using UnityEngine;
using static Character;

[CustomEditor(typeof(Character), true)]
public class CharacterEditor : Editor
{
	Character character;

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		serializedObject.Update();

		if (character == null)
			character = (Character)target;

		if (character.side == 0)
			character.side = 1; 

		SerializedProperty attackArray = serializedObject.FindProperty(nameof(Character.attacksData));

		if (GUILayout.Button("Add an hitbox"))
		{
			Hitbox2D hb = new Hitbox2D(character, Vector2.zero, Vector2.one);
			AttackData a = new AttackData(hb) { showGizmosHitbox = true };
			character.attacksData.Add(a);
			serializedObject.Update();
		}

		int index = -1;
		for (int i = 0; i < character.attacksData.Count; i++)
		{
			EditorGUILayout.Space(10);
			
			SerializedProperty soAttackData = attackArray.GetArrayElementAtIndex(i);

			EditorGUILayout.PropertyField(soAttackData.FindPropertyRelative(nameof(AttackData.hitbox)).FindPropertyRelative(nameof(Hitbox2D.offset)));
			EditorGUILayout.PropertyField(soAttackData.FindPropertyRelative(nameof(AttackData.hitbox)).FindPropertyRelative(nameof(Hitbox2D.size)));
			EditorGUILayout.PropertyField(soAttackData.FindPropertyRelative(nameof(AttackData.damageMultiplier)));
			EditorGUILayout.PropertyField(soAttackData.FindPropertyRelative(nameof(AttackData.showGizmosHitbox)));

			if (GUILayout.Button("Delete"))
				index = i;
		}

		if (index >= 0)
			character.attacksData.RemoveAt(index);

		serializedObject.ApplyModifiedProperties();
	}
}
