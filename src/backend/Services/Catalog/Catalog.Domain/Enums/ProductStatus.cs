namespace Catalog.Domain.Enums
{
    public enum ProductStatus
    {
        Draft = 0,      // Mới tạo, chưa bán
        Active = 1,     // Đang bán
        OutOfStock = 2, // Hết hàng
        Discontinued = 3 // Ngừng kinh doanh món này
    }
}
