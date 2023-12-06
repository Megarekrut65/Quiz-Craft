namespace Quiz
{
    public static class WeaponConverter
    {
        public static int Convert(string key)
        {
            switch (key)
            {
                case "SWORD": return 2;
                case "HAMMER": return 1;
                case "SCYTHE": return 0;
            }

            return 1;
        }
    }
}