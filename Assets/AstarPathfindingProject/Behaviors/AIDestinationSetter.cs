using UnityEngine;
using System.Collections;

namespace Pathfinding
{
	/// <summary>
	/// Sets the destination of an AI to the position of a specified object.
	/// This component should be attached to a GameObject together with a movement script such as AIPath, RichAI or AILerp.
	/// This component will then make the AI move towards the <see cref="target"/> set on this component.
	///
	/// See: <see cref="Pathfinding.IAstarAI.destination"/>
	///
	/// [Open online documentation to see images]
	/// </summary>
	[UniqueComponent(tag = "ai.destination")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_a_i_destination_setter.php")]
	public class AIDestinationSetter : VersionedMonoBehaviour
	{
		/// <summary>The object that the AI should move to</summary>

		public GameObject enemy;

		public Transform target; //Ignore this

		public string target1;
		public string target2;
		public static Vector3 currentTarget;

		private float target1Distance;
		private float target2Distance;
		private Vector3 enemyPosition;
		

		IAstarAI ai;

		void OnEnable()
		{
			ai = GetComponent<IAstarAI>();
			// Update the destination right before searching for a path as well.
			// This is enough in theory, but this script will also update the destination every
			// frame as the destination is used for debugging and may be used for other things by other
			// scripts as well. So it makes sense that it is up to date every frame.
			if (ai != null) ai.onSearchPath += Update;
		}

		void OnDisable()
		{
			if (ai != null) ai.onSearchPath -= Update;
		}

		/// <summary>Updates the AI's destination every frame</summary>
		void Update()
		{
			if (target1 != null && target2 != null && ai != null)
			{
				Vector2 target1Pos = GameObject.Find(target1).transform.position;
				Vector2 target2Pos = GameObject.Find(target2).transform.position;

				enemyPosition = enemy.transform.position;
				target1Distance = Vector2.Distance(enemyPosition, target1Pos);
				target2Distance = Vector2.Distance(enemyPosition, target2Pos);
				if (target1Distance < target2Distance)
				{
					ai.destination = target1Pos;
					currentTarget = target1Pos;
				}
				else if (target2Distance < target1Distance)
				{
					ai.destination = target2Pos;
					currentTarget = target2Pos;
				}
			}
		}
	}
}
