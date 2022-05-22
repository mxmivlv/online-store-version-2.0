using System.Collections.Generic;

#nullable disable

namespace Module_18
{
    public partial class PurchaseClient
    {
        public int Id { get; set; }
        public string EmailBuyer { get; set; }
        public long CodeProduct { get; set; }
        public string NameProduct { get; set; }
        public PurchaseClient(string emailBuyer, long codeProduct, string nameProduct)
        {
            EmailBuyer = emailBuyer;
            CodeProduct = codeProduct;
            NameProduct = nameProduct;
        }  
    }
}
