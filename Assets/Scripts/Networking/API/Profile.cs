namespace CryptoQuest.Networking.API
{
    public static class Profile
    {
        public const string GET_PROFILE = "crypto/user/profile";
        
        public const string EQUIPMENTS = "crypto/equipments";
        public const string LOAD_EQUIPMENT_PATH = "crypto/equipments?source=1";
        public const string LOAD_INGAME_EQUIPMENT_PATH = "crypto/equipments?source=2";
        public const string PUT_EQUIPMENTS_TO_DIMENSIONAL_BOX = "crypto/equipments/dimention/to";
        public const string PUT_EQUIPMENTS_TO_GAME = "crypto/equipments/dimention/from";

        public const string LOAD_METAD_PATH = "/crypto/dimention/token";
        public const string TRANSFER_TO_METAD_PATH = "/crypto/dimention/token/to";
        public const string TRANSFER_TO_DIAMOND_PATH = "/crypto/dimention/token/from";

        public const string GET_CHARACTERS = "crypto/characters";
    }
}