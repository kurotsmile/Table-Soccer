// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("gzGykYO+tbqZNfs1RL6ysrK2s7CB4ia/WgjEJYJH/7C5TMO2/JGTB8PZHBw3dl1rVc0wBn2kbZWanipZzlu8rA4HMiDW0g/0h+TqYKzQm6QxsryzgzGyubExsrKzAz7q0LNJvo8MMWCqYAXYr2ZM3hIvpbDU4cEIUusfPQb5VOWABSTSThUZnT2/huRmFrPo6oO5aKX6geEif9uPRvk+ifF49ssU/jDHg8EW3/5DauIhTYH7V2rwVlDNzSqlafkGB+9LHd0htSt9emoG1eT9H9/R9jVoOAv7l1GnGkTHY6m04GwMTJKIQM2gKsnNm1r8Kz5gesusGQJFs5lPWI+h90itkGwYjzddlM19CacIIIb4APNkJWip8uFobH512eoScrGwsrOy");
        private static int[] order = new int[] { 0,3,8,6,6,6,8,7,11,13,11,11,13,13,14 };
        private static int key = 179;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
