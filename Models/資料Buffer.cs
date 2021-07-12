using System.Collections.Generic;

namespace Utils.Models
{
    public class 資料Buffer<T>
    {
        public Dictionary<string, int> IndexMap { set; get; }
        public List<T> MapList { set; get; }
        public 資料Buffer()
        {
            IndexMap = new Dictionary<string, int>();
            MapList = new List<T>();
        }
        public bool Add(T obj, string key)
        {
            if (IndexMap.ContainsKey(key))
                return false;
            IndexMap.Add(key, MapList.Count);
            MapList.Add(obj);
            return true;
        }
        public int Length()
        {
            return MapList.Count;
        }
        public T Get(string key)
        {
            if (!IndexMap.TryGetValue(key, out int index))
            {
                return default;
            }
            return MapList[index];
        }
        public bool IsExist(string key)
        {
            return IndexMap.ContainsKey(key);
        }
        public List<T> ToList()
        {
            return MapList;
        }
        public bool Remove(string key)
        {
            if (!IndexMap.TryGetValue(key, out int index))
            {
                return false;
            }
            MapList.RemoveAt(index);
            IndexMap.Remove(key);
            Dictionary<string, int> NewIndexMap = new Dictionary<string, int>();
            foreach (KeyValuePair<string, int> kvp in IndexMap)
            {
                NewIndexMap.Add(kvp.Key, (kvp.Value > index ? kvp.Value - 1 : kvp.Value));
            }
            IndexMap = NewIndexMap;
            return true;
        }
    }
}
