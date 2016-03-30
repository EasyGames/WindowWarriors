using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;



public enum monsterToSpawn { Slime, Zombie, Bat, Dragon, DarkWizzard, Death }
public class EntityFactory : MonoBehaviour {


    // prefab list, add new prefab for each new monster
    public GameObject SlimePrefab;
    public GameObject ChestPrefab;
	public GameObject RingStealerPrefab;
	public GameObject BatPrefab;
	public GameObject DragonPrefab;
	public GameObject DarkWizardPrefab;
	public GameObject DeathMonsterPrefab;
	public GameObject GhostPrefab;
	public GameObject HeadlessZombiePrefab;
	public GameObject MummyPrefab;
	public GameObject NecromancerPrefab;
	public GameObject PharaohPrefab;
	public GameObject SkeletonPrefab;
	public GameObject SkeletonArcherPrefab;
	public GameObject SkeletonWarriorPrefab;
	public GameObject SpiritPrefab;
	public GameObject VampirePrefab;
	public GameObject VampireKingPrefab;
	public GameObject VampirePrincePrefab;
	public GameObject ZombiePrefab;

    public GameObject initializeSlime(Vector3 pos, int strenght = 0, int agility = 0, int speed =0, int endurance =0, int charisma=0, int inteligence =0, int wisdom =0)
    {
        
        GameObject initializedSlime;
        Slime slimeScript;
        initializedSlime = (GameObject)Instantiate(SlimePrefab, pos, Quaternion.identity);
        slimeScript = initializedSlime.GetComponent<Slime>();
        slimeScript.initialize(strenght, agility, speed, endurance, charisma, inteligence, wisdom);

        return initializedSlime;
    }

    [SpawnFunctionAssembly(monsterToSpawn.Zombie)]
    public GameObject initializeZombie(Vector3 pos, int strenght = 0, int agility = 0, int speed =0, int endurance =0, int charisma=0, int inteligence =0, int wisdom =0)
	{
		GameObject initializedZombie;
		ZombieScript zombieScript;
		initializedZombie = (GameObject)Instantiate(ZombiePrefab, pos, Quaternion.identity);
		zombieScript = initializedZombie.GetComponent<ZombieScript>();
		zombieScript.initialize(strenght, agility, speed, endurance, charisma, inteligence, wisdom);

		return initializedZombie;
	}

    public GameObject initializeVampirePrince(Vector3 pos, int strenght = 0, int agility = 0, int speed =0, int endurance =0, int charisma=0, int inteligence =0, int wisdom =0)
	{
		GameObject initializedVampirePrince;
		VampirePrinceScript vampirePrinceScript;
		initializedVampirePrince = (GameObject)Instantiate(VampirePrincePrefab, pos, Quaternion.identity);
		vampirePrinceScript = initializedVampirePrince.GetComponent<VampirePrinceScript>();
		vampirePrinceScript.initialize(strenght, agility, speed, endurance, charisma, inteligence, wisdom);

		return initializedVampirePrince;
	}

    [ExposeToEditor]
    public GameObject initializeVampireKing(Vector3 pos, int strenght = 0, int agility = 0, int speed =0, int endurance =0, int charisma=0, int inteligence =0, int wisdom =0)
	{
		GameObject initializedVampireKing;
		VampireKingScript vampireKingScript;
		initializedVampireKing = (GameObject)Instantiate(VampireKingPrefab, pos, Quaternion.identity);
		vampireKingScript = initializedVampireKing.GetComponent<VampireKingScript>();
		vampireKingScript.initialize(strenght, agility, speed, endurance, charisma, inteligence, wisdom);

		return initializedVampireKing;
	}

    [ExposeToEditor]
    public GameObject initializeVampire(Vector3 pos, int strenght = 0, int agility = 0, int speed =0, int endurance =0, int charisma=0, int inteligence =0, int wisdom =0)
	{
		GameObject initializedVampire;
		VampireScript vampireScript;
		initializedVampire = (GameObject)Instantiate(VampirePrefab, pos, Quaternion.identity);
		vampireScript = initializedVampire.GetComponent<VampireScript>();
		vampireScript.initialize(strenght, agility, speed, endurance, charisma, inteligence, wisdom);

		return initializedVampire;
	}

    [ExposeToEditor]
    public GameObject initializeSpirit(Vector3 pos, int strenght = 0, int agility = 0, int speed =0, int endurance =0, int charisma=0, int inteligence =0, int wisdom =0)
	{
		GameObject initializedSpirit;
		SpiritScript spiritScript;
		initializedSpirit = (GameObject)Instantiate(SpiritPrefab, pos, Quaternion.identity);
		spiritScript = initializedSpirit.GetComponent<SpiritScript>();
		spiritScript.initialize(strenght, agility, speed, endurance, charisma, inteligence, wisdom);

		return initializedSpirit;
	}

    [ExposeToEditor]
    public GameObject initializeSkeletonWarrior(Vector3 pos, int strenght = 0, int agility = 0, int speed =0, int endurance =0, int charisma=0, int inteligence =0, int wisdom =0)
	{
		GameObject initializedSkeletonWarrior;
		SkeletonWarriorScript skeletonWarriorScript;
		initializedSkeletonWarrior = (GameObject)Instantiate(SkeletonWarriorPrefab, pos, Quaternion.identity);
		skeletonWarriorScript = initializedSkeletonWarrior.GetComponent<SkeletonWarriorScript>();
		skeletonWarriorScript.initialize(strenght, agility, speed, endurance, charisma, inteligence, wisdom);

		return initializedSkeletonWarrior;
	}

    [ExposeToEditor]
    public GameObject initializeSkeletonArcher(Vector3 pos, int strenght = 0, int agility = 0, int speed =0, int endurance =0, int charisma=0, int inteligence =0, int wisdom =0)
	{
		GameObject initializedSkeletonArcher;
		SkeletonArcherScript skeletonArcherScript;
		initializedSkeletonArcher = (GameObject)Instantiate(SkeletonArcherPrefab, pos, Quaternion.identity);
		skeletonArcherScript = initializedSkeletonArcher.GetComponent<SkeletonArcherScript>();
		skeletonArcherScript.initialize(strenght, agility, speed, endurance, charisma, inteligence, wisdom);

		return initializedSkeletonArcher;
	}

    [ExposeToEditor]
    public GameObject initializeSkeleton(Vector3 pos, int strenght = 0, int agility = 0, int speed =0, int endurance =0, int charisma=0, int inteligence =0, int wisdom =0)
	{
		GameObject initializedSkeleton;
		SkeletonScript skeletonScript;
		initializedSkeleton = (GameObject)Instantiate(SkeletonPrefab, pos, Quaternion.identity);
		skeletonScript = initializedSkeleton.GetComponent<SkeletonScript>();
		skeletonScript.initialize(strenght, agility, speed, endurance, charisma, inteligence, wisdom);

		return initializedSkeleton;
	}

    [ExposeToEditor]
    public GameObject initializeNecromancer(Vector3 pos, int strenght = 0, int agility = 0, int speed =0, int endurance =0, int charisma=0, int inteligence =0, int wisdom =0)
	{
		GameObject initializedNecromancer;
		NecromancerScript necromancerScript;
		initializedNecromancer = (GameObject)Instantiate(NecromancerPrefab, pos, Quaternion.identity);
		necromancerScript = initializedNecromancer.GetComponent<NecromancerScript>();
		necromancerScript.initialize(strenght, agility, speed, endurance, charisma, inteligence, wisdom);

		return initializedNecromancer;
	}

    [ExposeToEditor]
    public GameObject initializePharaoh(Vector3 pos, int strenght = 0, int agility = 0, int speed =0, int endurance =0, int charisma=0, int inteligence =0, int wisdom =0)
	{
		GameObject initializedPharaoh;
		PharaohScript pharaohScript;
		initializedPharaoh = (GameObject)Instantiate(NecromancerPrefab, pos, Quaternion.identity);
		pharaohScript = initializedPharaoh.GetComponent<PharaohScript>();
		pharaohScript.initialize(strenght, agility, speed, endurance, charisma, inteligence, wisdom);

		return initializedPharaoh;
	}

    [ExposeToEditor]
    public GameObject initializeMummy(Vector3 pos, int strenght = 0, int agility = 0, int speed =0, int endurance =0, int charisma=0, int inteligence =0, int wisdom =0)
	{
		GameObject initializedMummy;
		MummyScript mummyScript;
		initializedMummy = (GameObject)Instantiate(MummyPrefab, pos, Quaternion.identity);
		mummyScript = initializedMummy.GetComponent<MummyScript>();
		mummyScript.initialize(strenght, agility, speed, endurance, charisma, inteligence, wisdom);

		return initializedMummy;
	}

    [ExposeToEditor]
    public GameObject initializeHeadlessZombie(Vector3 pos, int strenght = 0, int agility = 0, int speed =0, int endurance =0, int charisma=0, int inteligence =0, int wisdom =0)
	{
		GameObject initializedHeadlessZombie;
		HeadlessZombieScript headlessZombieScript;
		initializedHeadlessZombie = (GameObject)Instantiate(HeadlessZombiePrefab, pos, Quaternion.identity);
		headlessZombieScript = initializedHeadlessZombie.GetComponent<HeadlessZombieScript>();
		headlessZombieScript.initialize(strenght, agility, speed, endurance, charisma, inteligence, wisdom);

		return initializedHeadlessZombie;
	}

    [ExposeToEditor]
    public GameObject initializeGhost(Vector3 pos, int strenght = 0, int agility = 0, int speed =0, int endurance =0, int charisma=0, int inteligence =0, int wisdom =0)
	{
		GameObject initializedGhost;
		GhostScript ghostScript;
		initializedGhost = (GameObject)Instantiate(GhostPrefab, pos, Quaternion.identity);
		ghostScript = initializedGhost.GetComponent<GhostScript>();
		ghostScript.initialize(strenght, agility, speed, endurance, charisma, inteligence, wisdom);

		return initializedGhost;
	}

    [SpawnFunctionAssembly(monsterToSpawn.DarkWizzard)]
    public GameObject initializeDarkWizard(Vector3 pos, int strenght = 0, int agility = 0, int speed =0, int endurance =0, int charisma=0, int inteligence =0, int wisdom =0)
	{
		GameObject initializedDarkWizard;
		DarkWizardScript darkWizardScript;
		initializedDarkWizard = (GameObject)Instantiate(DarkWizardPrefab, pos, Quaternion.identity);
		darkWizardScript = initializedDarkWizard.GetComponent<DarkWizardScript>();
		darkWizardScript.initialize(strenght, agility, speed, endurance, charisma, inteligence, wisdom);

		return initializedDarkWizard;
	}

    [SpawnFunctionAssembly(monsterToSpawn.Death)]
    public GameObject initializeDeathMonster(Vector3 pos, int strenght = 0, int agility = 0, int speed =0, int endurance =0, int charisma=0, int inteligence =0, int wisdom =0)
	{
		GameObject initializedDeathMonster;
		DeathMonsterScript deathMonsterScript;
		initializedDeathMonster = (GameObject)Instantiate(DeathMonsterPrefab, pos, Quaternion.identity);
		deathMonsterScript = initializedDeathMonster.GetComponent<DeathMonsterScript>();
		deathMonsterScript.initialize(strenght, agility, speed, endurance, charisma, inteligence, wisdom);

		return initializedDeathMonster;
	}

    [ExposeToEditor]
    public GameObject initializeRingStealer(Vector3 pos, int strenght = 0, int agility = 0, int speed =0, int endurance =0, int charisma=0, int inteligence =0, int wisdom =0)
	{
		GameObject initializedRingStealer;
		Slime slimeScript;
		initializedRingStealer = (GameObject)Instantiate(RingStealerPrefab, pos, Quaternion.identity);
		slimeScript = initializedRingStealer.GetComponent<Slime>();
		slimeScript.initialize(strenght, agility, speed, endurance, charisma, inteligence, wisdom);

		return initializedRingStealer;
	}

    [SpawnFunctionAssembly(monsterToSpawn.Bat)]
    public GameObject initializeBat(Vector3 pos, int strenght = 0, int agility = 0, int speed =0, int endurance =0, int charisma=0, int inteligence =0, int wisdom =0)
	{
		GameObject initializedBat;
		BatScript batScript;
		initializedBat = (GameObject)Instantiate(BatPrefab, pos, Quaternion.identity);
		batScript = initializedBat.GetComponent<BatScript>();
		batScript.initialize(strenght, agility, speed, endurance, charisma, inteligence, wisdom);

		return initializedBat;
	}

    [SpawnFunctionAssembly(monsterToSpawn.Dragon)]
    public GameObject initializeDragon(Vector3 pos, int strenght = 0, int agility = 0, int speed =0, int endurance =0, int charisma=0, int inteligence =0, int wisdom =0)
	{
		GameObject initializedDragon;
		DragonScript dragonScript;
		initializedDragon = (GameObject)Instantiate(DragonPrefab, pos, Quaternion.identity);
		dragonScript = initializedDragon.GetComponent<DragonScript>();
		dragonScript.initialize(strenght, agility, speed, endurance, charisma, inteligence, wisdom);

		return initializedDragon;
	}

    [ExposeToEditor]
    public GameObject initializeChest(Vector3 pos)
    {
        GameObject initializedChest;
        EntityBase chestScript;
        initializedChest = (GameObject)Instantiate(ChestPrefab, pos, Quaternion.identity);
        chestScript = initializedChest.GetComponent<EntityBase>();
        chestScript.life = 0;
        return initializedChest;
    }
}
