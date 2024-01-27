using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;


namespace FFramework
{

    public static class GameObjectPool
    {
        private static Dictionary<string, Spawner> pools = new Dictionary<string, Spawner>();
        

        public static void CreateSpawner(string tag,AssetHandle handle,bool keepMin=false)
        {
            if (!pools.ContainsKey(tag))
            {
                Spawner sp = new Spawner(handle);
                if (keepMin) sp.EnableUpdate();
                pools.Add(tag, sp);
            }
        }
        public static void DestroySpawner(string tag)
        {
            if(pools.ContainsKey(tag))
            {
                Spawner sp = pools[tag];
                sp.DisableUpdate();
                pools.Remove(tag);
            }
        }

        public static Spawner GetSpawner(string tag)
        {
            if(pools.ContainsKey(tag))
            {
                return pools[tag];
            }
            throw new KeyNotFoundException("No such spawner");
        }

    }
}