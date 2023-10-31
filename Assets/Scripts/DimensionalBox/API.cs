namespace CryptoQuest.DimensionalBox
{
    public static class API
    {
        public const string EQUIPMENTS = "crypto/equipments";
        public const string LOAD_EQUIPMENT_PATH = "/crypto/equipments?source=1";
        public const string UPDATE_EQUIPMENT_FROM_WALLET_PATH = "/crypto/equipments/dimention/from";

        public const string LOAD_METAD_PATH = "/crypto/dimention/token";
        public const string TRANSFER_TO_METAD_PATH = "/crypto/dimention/token/to";
        public const string TRANSFER_TO_DIAMOND_PATH = "/crypto/dimention/token/from";
    }
}