namespace Features.Externals.Services;

public interface IEmbeddingService
{
    /// <summary>
    /// Nhận một chuỗi text và trả về vector embedding của nó.
    /// </summary>
    /// <param name="textToEmbed">Đoạn text cần tạo vector.</param>
    /// <returns>Một mảng float[] đại diện cho vector, hoặc null nếu có lỗi.</returns>
    Task<float[]?> GetEmbeddingAsync(string textToEmbed);
}