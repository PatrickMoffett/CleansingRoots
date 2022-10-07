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

        private void Start()
        {
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
                batteryDoor.SwitchOff();
                foreach (var doorSwitch in doorSwitches)
                {
                    doorSwitch.SwitchOff();
                }
                foreach (var enemyDoor in enemyDoors)
                {
                    enemyDoor.SwitchOff();
                }
            }
        }

        public void EndFight()
        {
            StartCoroutine(StartEndGameExplosion());
        }

        private IEnumerator StartEndGameExplosion()
        {
            // At end call this
            // Stop player from being able to be killed
            yield return new WaitForSeconds(5);
            
            ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(typeof(GameWonState),true);
        }
    }
}