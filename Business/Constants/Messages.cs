using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Constants
{
    public static class Messages
    {
        public static string ProductAdded = "Ürün Eklendi";
        public static string ProductNameInvalid = "Ürün ismi geçersiz";
        public static string GetAllSuccess = "Bütün veriler başarıyla getirildi";
        public static string MaintenanceTime = "Bakım zamanı";
        public static string ProductCountOfCategoryError = "Bir kategoride en fazla 10 ürün olabilir.";
        internal static string ProductNameAlreadyUsed = "Bu ürün adı önceden kullanılmış.";
        internal static string CategoryCountIsHigher= "Mevcut kategori sayısı 15ten fazla olduğu için ürün eklenemedi.";
    }
}
