using System.Collections.Generic;

namespace CheeseCraft.Script.Extension
{
    public static class CollectionExtension
    {
        public static bool IsTheSameAs<T>(this List<T> list1, List<T> list2)
        {
            var copiedList = new List<T>(list2);

            foreach (var item in list1)
            {
                if (!copiedList.Contains(item))
                    return false;

                list2.Remove(item);
            }

            return list2.Count <= 0;
        }
    }
}