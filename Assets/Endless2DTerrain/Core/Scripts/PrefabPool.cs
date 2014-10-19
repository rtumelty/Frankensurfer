using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Endless2DTerrain
{
    public class PrefabPool
    {
		bool preinstanciated = false;
		Settings settings;

        public PrefabPool(Settings s)
        {
            Prefabs = new List<PrefabQueue>();
			settings = s;
        }
		
		public void Preinstanciate() {
			preinstanciated = true;
			Vector3 preinstanciatePosition = new Vector3 (0, -10000, 0);
			Transform parent = GameObject.Find ("Prefab Manager").transform;

			foreach (PrefabRule rule in settings.PrefabRules) {
				for (int i = 0; i < rule.PreinstanciatedCount; i++) {
					PrefabQueue prefabToAdd = new PrefabQueue();
					prefabToAdd.IsSpawned = false;

					GameObject prefab = (GameObject)GameObject.Instantiate(rule.PrefabToClone, preinstanciatePosition, new Quaternion());
					prefab.SetActive(false);
					prefab.transform.parent = parent;
					prefab.name = rule.PrefabToClone.name;
					
					prefabToAdd.PrefabType = rule.PrefabToClone.name;
					prefabToAdd.Prefab = prefab;
					Prefabs.Add(prefabToAdd);
				}
			}
		}

        public List<PrefabQueue> Prefabs { get; set; }

        public GameObject Add(GameObject prefabToClone, Vector3 position, float angle, string type, bool matchGroundAngle)
        {
			if (!preinstanciated)
								Preinstanciate ();
     
            PrefabQueue prefabToAdd = Prefabs.Where(t=>t.PrefabType == type && t.IsSpawned == false).FirstOrDefault();
            
            if (prefabToAdd == null){
                //Let's create a new one
                prefabToAdd = new PrefabQueue();
                GameObject prefab = (GameObject)GameObject.Instantiate(prefabToClone, position, new Quaternion());
                prefab.name = prefabToClone.name;
                if (angle != 0 && matchGroundAngle){
                     prefab.transform.localEulerAngles = new Vector3(0, 0, angle);
                }
                prefabToAdd.PrefabType = type;
                prefabToAdd.Prefab = prefab;
                prefabToAdd.IsSpawned = true;
                prefabToAdd.Prefab.SetActive(true);
                Prefabs.Add(prefabToAdd);
            }else{ 
                //Just update one from the queue
				Debug.Log(prefabToAdd + " " + prefabToAdd.Prefab);
                prefabToAdd.Prefab.transform.position = position;
                if (angle != 0 && matchGroundAngle){
                     prefabToAdd.Prefab.transform.localEulerAngles = new Vector3(0, 0, angle);
                }
                prefabToAdd.IsSpawned =true;
                prefabToAdd.Prefab.SetActive(true);
            }
            return prefabToAdd.Prefab;
        }

        public void Remove(GameObject prefab)
        {
            var prefabToRemove = Prefabs.Where(p => p.Prefab == prefab).FirstOrDefault();        
            prefabToRemove.IsSpawned = false;
            prefabToRemove.Prefab.SetActive(false);
      
     
        }

        public class PrefabQueue
        {
            public GameObject Prefab { get; set; }
            public string PrefabType { get; set; }
            public bool IsSpawned { get; set; }
        }
    }
}
