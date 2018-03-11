namespace Wikiled.Common.Helpers
{
    public static class HashHelper
    {
        public static int GenerateHash(params object[] objects)
        {
            var hash = 0;
            if (objects == null ||
                objects.Length == 0)
            {
                return hash;
            }

            foreach (var item in objects)
            {
                if (item == null)
                {
                    continue;
                }

                hash = (hash << 5 | hash >> 27) ^ item.GetHashCode();
            }

            return hash;
        }
    }
}
