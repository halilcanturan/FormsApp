namespace FormsApp.Models
{
    public class Repository
    {
        private static readonly List<Product> _product = new();
        private static readonly List<Category> _categories = new();

        static Repository()
        {
            _categories.Add(new Category { CategoryId = 1, Name = "Telefon" });
            _categories.Add(new Category { CategoryId = 2, Name = "Bilgisayar" });

            _product.Add(new Product { ProductId = 1, Name = "Iphone 14", Price = 4000, IsActive = true, Image = "1.jpg", CategoryId = 1 });
            _product.Add(new Product { ProductId = 2, Name = "Iphone 15", Price = 5000, IsActive = true, Image = "2.jpg", CategoryId = 1 });
            _product.Add(new Product { ProductId = 3, Name = "Iphone 16", Price = 6000, IsActive = false, Image = "3.jpg", CategoryId = 1 });
            _product.Add(new Product { ProductId = 4, Name = "Iphone 17", Price = 7000, IsActive = false, Image = "4.jpg", CategoryId = 1 });
            _product.Add(new Product { ProductId = 5, Name = "MacBook Air", Price = 8000, IsActive = true, Image = "5.jpg", CategoryId = 2 });
            _product.Add(new Product { ProductId = 6, Name = "MacBook Pro", Price = 9000, IsActive = true, Image = "6.jpg", CategoryId = 2 });
        }

        public static List<Product> Product
        {
            get
            {
                return _product;
            }
        }

        public static void CreateProduct(Product entity)
        {
            _product.Add(entity);
        }

        public static List<Category> Categories
        {
            get
            {
                return _categories;
            }
        }

        public static void EditProduct(Product updateProduct)
        {
            var entity = _product.FirstOrDefault(p => p.ProductId == updateProduct.ProductId);

            if (entity != null)
            {
                if (!string.IsNullOrEmpty(updateProduct.Name))
                {
                    entity.Name = updateProduct.Name;
                }
                entity.Price = updateProduct.Price;
                entity.Image = updateProduct.Image;
                entity.CategoryId = updateProduct.CategoryId;
                entity.IsActive = updateProduct.IsActive;
            }
        }

        public static void EditIsActive(Product updateProduct)
        {
            var entity = _product.FirstOrDefault(p => p.ProductId == updateProduct.ProductId);

            if (entity != null)
            {
                entity.IsActive = updateProduct.IsActive;
            }
        }

        public static void DeleteProduct(Product deleteProduct) //get ile yapıyoruz çünkü sayfa değişmiyor
        {
            var entity = _product.FirstOrDefault(p => p.ProductId == deleteProduct.ProductId);

            if (entity != null)
            {
                _product.Remove(entity);
            }
        }
    }
}
