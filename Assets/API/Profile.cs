namespace CryptoQuest.API
{
    public static class Profile
    {
        public const string DEBUG_KEY = "GQwuFb5HYRrbodgHmlyeJPXYDfRUpxkOZrFlWarb"; //TODO: Remove
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
        public const string PUT_CHARACTERS_TO_DIMENSIONAL_BOX = "crypto/characters/dimention/to";
        public const string PUT_CHARACTERS_TO_GAME = "crypto/characters/dimention/from";
        public const string PUT_CHARACTERS_TO_BOX_AND_GAME = "crypto/characters/dimension/transfer";

        public const string GET_BEASTS = "crypto/beasts";
        public const string PUT_BEASTS_TO_DIMENSIONAL_BOX = "crypto/beasts/dimention/to";
        public const string PUT_BEASTS_TO_GAME = "crypto/beasts/dimention/from";
        public const string PUT_BEASTS_TO_BOX_AND_GAME = "crypto/beasts/dimension/transfer";

        public const string MAGIC_STONE = "crypto/magicstone";
        public const string MAGIC_STONE_TRANSFER = "crypto/magicstone/dimension/transfer";
    }
}