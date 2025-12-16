using System.ComponentModel;

namespace Catalog.Domain.Enums
{
    public enum ProductStatus
    {
        [Description("Mới tạo, chưa bán")]
        Draft = 0,
        [Description("Đang bán")]
        Active = 1,
        [Description("Hết hàng")]
        OutOfStock = 2,
        [Description("Ngừng kinh doanh")]
        Discontinued = 3
    }
}
