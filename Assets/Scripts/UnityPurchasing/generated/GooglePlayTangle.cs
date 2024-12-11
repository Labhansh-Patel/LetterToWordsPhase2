// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("u9LInMVPQNRs9RbmzezMsyvUPxBgQK9X5Tt9Y6//4QnEbgztN4yXaBDha8lk7LeXCWUZd0FlJalG4Iq8UU9jDJiPMren1EoC4xfO0CfOjjWkKxTx26UwP59Me+GUkKnEqnpDkHPgTk71FsxELDDt4Kql1yW9kaR7eNM5zuuY0pZQRgFC4wy04GcHpj+LCbIGUfIGV0A+6GVrrKYg4RY4nZxZqq9t5vSpmTET/1dva+FVY/33K5kaOSsWHRIxnVOd7BYaGhoeGxhFxS6BL50kD4xGcKzHodC0AtU9asBbMKwS/Pb0LpfZRjnK7Izw5RammRoUGyuZGhEZmRoaG9q6JZpaIweY/zSDAje6Wxsx3K4i9gMzg2EatqOB6OkdAt1grBkYGhsa");
        private static int[] order = new int[] { 6,10,7,8,13,7,7,13,9,13,13,11,13,13,14 };
        private static int key = 27;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
