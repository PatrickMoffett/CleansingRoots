using System;
using System.Collections;
using System.Collections.Generic;
using Interactables.Misc;
using Interactables.Switches;
using UnityEngine;

namespace Combat.Boss
{
    public class FactoryBoss: MonoBehaviour, IDamageable
    {
        [SerializeField] private List<BossSpawner> bossSpawners;
        [SerializeField] private int health = 3;
        [SerializeField] private MovableDoor entranceDoor;
        [SerializeField] private List<MovableDoor> enemyDoors;
        [SerializeField] private MovableDoor batteryDoor;
        [SerializeField] private PlayerDetector playerDetector;
        [SerializeField] private List<SwitchEventForwarder> doorSwitches;
        [SerializeField] private List<RespondsToSwitch> wires;
        [SerializeField] private GameObject endGameExplosion;
        [SerializeField] private Texture damagedTex;
        [SerializeField] private Texture heavyDamageTex;
        [SerializeField] private GameObject destroyedMesh;
        [SerializeField] private GameObject deathPE;

        private Material mat;
        private static readonly int MainTex = Shader.PropertyToID("MainTex");

        private void Start()
        {
            mat = GetComponent<MeshRenderer>().materials[1];
            playerDetector.OnPlayerDetected += StartFight;
            foreach (var doorSwitch in doorSwitches)
            {
                doorSwitch.SwitchOff();
                doorSwitch.NewSwitchState += SwitchChanged;
            }
        }

        private void SwitchChanged(bool newSwitchState)
        {
            //if the new state of the switch is on check if all switches are on
            //and if they are open the door
            //else close the door
            if (newSwitchState)
            {
                for(int i = 0; i < doorSwitches.Count; i++)
                {
                    if (!doorSwitches[i].GetSwitchState())
                    {
                        batteryDoor.SwitchOff();
                        return;
                    }
                }
                batteryDoor.SwitchOn();
                
                foreach(var enemyDoor in enemyDoors){
                    enemyDoor.SwitchOn();
                }

                foreach (var bossSpawner in bossSpawners)
                {
                    bossSpawner.SpawnEnemies();
                }
            }
            else
            {
                batteryDoor.SwitchOff();
            }
        }

        private void StartFight()
        {
            entranceDoor.SwitchOff();
        }

        public void TakeDamage(int damage)
        {
            //cant take damage if door isn't open
            if (!batteryDoor.IsOpen()) return;
            
            health--;
            if (health == 0)
            {
                EndFight();
            }
            else
            {
                if (health == 1)
                {
                    mat.SetTexture(MainTex,heavyDamageTex);
                }else if (health == 2)
                {
                    mat.SetTexture(MainTex,damagedTex);
                }
                batteryDoor.SwitchOff();
                foreach (var doorSwitch in doorSwitches)
                {
                    doorSwitch.SwitchOff();
                }
                foreach (var enemyDoor in enemyDoors)
                {
                    enemyDoor.SwitchOff();
                }

                foreach (var wire in wires)
                {
                    wire.SwitchOff();
                }
            }
        }

        public void EndFight()
        {
            Instantiate(destroyedMesh, transform.position, transform.parent.rotation).transform.localScale = transform.parent.localScale;
            Instantiate(deathPE, transform.position, Quaternion.identity);
            Destroy(gameObject.transform.parent.gameObject);
            ServiceLocator.Instance.Get<MonoBehaviorService>().StartCoroutine(StartEndGameExplosion(endGameExplosion));
        }

        private IEnumerator StartEndGameExplosion(GameObject explosionVFX)
        {
            // Stop player from being able to be killed
            var player = GameObject.FindWithTag("Player").GetComponent<Health>().canTakeDamage = false;
            
            explosionVFX.SetActive(true);
                
            yield return new WaitForSeconds(5);
            
            ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(typeof(GameWonState),true);
        }
    }
}