using HNG_Stage_0.Models;

namespace HNG_Stage_0.Interfaces
{
    public interface IGenderizeService
    {
        Task<BaseResponse<GenderizeResponseDto>> ClassifyUser(string name);
         
    }
}
