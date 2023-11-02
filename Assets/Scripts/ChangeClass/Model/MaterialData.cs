using CryptoQuest.ChangeClass.Interfaces;
using CryptoQuest.ChangeClass.API;

namespace CryptoQuest.ChangeClass.Models
{
    public class MaterialData : IMaterialModel
    {
        public int Id { get; private set; }
        public string MaterialId { get; private set; }
        public int MaterialNum { get; private set; }
        public string LocalizeKey { get; private set; }
        public string NameJp { get; private set; }
        public string DescriptionJp { get; private set; }
        public string NameEn { get; private set; }
        public string DescriptionEn { get; private set; }
        public float Price { get; private set; }
        public float SellingPrice { get; private set; }
        public MaterialData(MaterialAPI material)
        {
            Id = material.id;
            MaterialId = material.materialId;
            MaterialNum = material.materialNum;
            LocalizeKey = material.localizeKey;
            NameJp = material.nameJp;
            DescriptionJp = material.descriptionJp;
            NameEn = material.nameEn;
            DescriptionEn = material.descriptionEn;
            Price = material.price;
            SellingPrice = material.sellingPrice;
        }
    }
}
