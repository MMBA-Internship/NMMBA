using UnityEngine;
using System.Linq;

public class FishScarer : MonoBehaviour
{
	[SerializeField] private AnimalFind animalFind;
	[SerializeField] private float scareCooldown = 5f;
	private Time lastScared;

	private void OnEnable()
	{
		GameEvents.OnHandEntered += HanleOnHandEntered;
	}

	private void OnDisable()
	{
		GameEvents.OnHandEntered -= HanleOnHandEntered;
	}

	// TODO: Make a cooldown so scare can't be called every 0.2 seconds
	private void HanleOnHandEntered()
	{
		var fishes = animalFind.GetFishVisibilityData();

		foreach (AnimalFindInfo fish in fishes)
		{
			FishManager fManager = fish.fishObject.GetComponent<FishManager>();
			if (fManager)
				fManager.Scare();
		}
	}
}
