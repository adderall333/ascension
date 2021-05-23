using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public int Count { get; set; }

        [InverseProperty("Purchases")] public Product FirstProduct { get; set; }
        public int FirstProductId { get; set; }

        public Product SecondProduct { get; set; }
        public int SecondProductId { get; set; }

        public static void UpdatePurchases(Order order, ApplicationContext context)
        {
            var productLines = order.ProductLines;
            var combinations = productLines
                .SelectMany(productLine => productLines, Tuple.Create)
                .Where(tuple => tuple.Item1.Id != tuple.Item2.Id);

            foreach (var (productLine1, productLine2) in combinations)
            {
                var purchase = context
                    .Purchase
                    .FirstOrDefault(p => p.FirstProductId == productLine1.Product.Id &&
                                         p.SecondProductId == productLine2.Product.Id);

                if (purchase == null)
                {
                    purchase = new Purchase
                    {
                        FirstProduct = productLine1.Product,
                        SecondProduct = productLine2.Product
                    };
                    context.Purchase.Add(purchase);
                }

                purchase.Count++;
                context.SaveChanges();
            }

        }
    }
}