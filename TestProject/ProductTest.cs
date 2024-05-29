using InternetStore.Models;

namespace TestProject
{
    public class ProductTest
    {
        [Fact]
        public void TestProduct()
        {
            Product product = new()
            {
                Id = 1,
                Name = "Помідор",
                Price = 4.50,
                Image = "/images/djlhfwrfer9fg"
            };
            Assert.Equal(1, product.Id);
            Assert.Equal("Помідор", product.Name);
            Assert.Equal(4.50, product.Price);
            Assert.Equal("/images/djlhfwrfer9fg", product.Image);
        }
    }
}
