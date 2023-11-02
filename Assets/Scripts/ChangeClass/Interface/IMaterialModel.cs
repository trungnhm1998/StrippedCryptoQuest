
namespace CryptoQuest.ChangeClass.Interfaces
{
    public interface IMaterialModel
    {
        public int Id { get; }
        public string MaterialId { get; }
        public int MaterialNum { get; }
        public string LocalizeKey { get; }
        public string NameJp { get; }
        public string DescriptionJp { get; }
        public string NameEn { get; }
        public string DescriptionEn { get; }
        public float Price { get; }
        public float SellingPrice { get; }
    }
}