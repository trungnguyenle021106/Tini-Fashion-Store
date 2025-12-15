using BuildingBlocks.Core.Exceptions;
using Catalog.Application.Common.Interfaces;
using MediatR;

namespace Catalog.Application.CQRS.Products.Commands.UpdateProduct
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResult>
    {
        private readonly ICatalogDbContext _dbContext;

        public UpdateProductHandler(ICatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            // 1. Tìm Product
            var product = await _dbContext.Products.FindAsync(new object[] { command.Id }, cancellationToken);

            // TẬN DỤNG: Dùng DomainException để báo lỗi không tìm thấy.
            // GlobalExceptionHandler sẽ bắt lỗi này và trả về 400 Bad Request.
            if (product == null)
            {
                throw new DomainException($"Không tìm thấy sản phẩm với Id: {command.Id}");
            }

            // 2. Cập nhật dữ liệu vào Entity
            // Các logic như Price < 0 đã được check trong Entity
            product.UpdateDetails(
                command.Name,
                command.Price,
                command.Description,
                command.ImageUrl,
                command.CategoryId
            );

            // 3. Update và Save
            _dbContext.Products.Update(product);

            // TẬN DỤNG: Nếu CategoryId không tồn tại trong DB -> SQL sẽ báo lỗi Foreign Key.
            // EF Core ném DbUpdateException -> GlobalExceptionHandler bắt được -> Trả về lỗi "Invalid Reference".
            // Bạn không cần query kiểm tra CategoryId thủ công ở đây => Code gọn hơn.
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }
    }
}