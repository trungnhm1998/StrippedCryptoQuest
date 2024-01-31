using System;
using System.Collections.Generic;

namespace CryptoQuest.ChangeClass.API
{
    [Serializable]
    public class MaterialResponseData
    {
        public int code;
        public bool success;
        public string message;
        public string uuid;
        public int gold;
        public float diamond;
        public int soul;
        public long time;
        public MaterialData data;
    }

    [Serializable]
    public class MaterialData
    {
        public List<MaterialAPI> materials;
    }

    [Serializable]
    public class MaterialAPI
    {
        public int id;
        public string materialId;
        public int materialNum;
        public string localizeKey;
        public string nameJp;
        public string descriptionJp;
        public string nameEn;
        public string descriptionEn;
        public float price;
        public float sellingPrice;
    }
}
