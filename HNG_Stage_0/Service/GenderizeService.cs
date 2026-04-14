using HNG_Stage_0.Interfaces;
using HNG_Stage_0.Models;
using System.Text.Json;

namespace HNG_Stage_0.Service
{
    public class GenderizeService : IGenderizeService
    {
        private readonly HttpClient _httpClient;
        public GenderizeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BaseResponse<GenderizeResponseDto>> ClassifyUser(string name)
        {
            if (name == null || name.Length == 0)
            {
                return new BaseResponse<GenderizeResponseDto>
                {
                    Status = "error",
                    Message = "error Must be inputed"
                   
                };
            }
            var response = await _httpClient.GetAsync($"https://api.genderize.io?name={name}");
            if(!response.IsSuccessStatusCode)
            {
                return new BaseResponse<GenderizeResponseDto>
                {
                    Status = "error",
                    Message = "Failed to fetch data from Genderize"
                    
                };
            }
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<GenderizeDto>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });
            if (data.Gender == null || data.Count == 0)
            {
                return new BaseResponse<GenderizeResponseDto>
                {
                    Status = "error",
                    Message = "no prediction available for the provided name"
                };
            }
            var isConfident = data.Probability >= 0.7 && data.Count >= 100;
            //if (!isConfident)
            //{
            //    return new BaseResponse<GenderizeDto> { Status = "error",
            //        Message = "isConfident must not be false"
            //}
            return new BaseResponse<GenderizeResponseDto>
            {
                Status = "success",
                Data = new GenderizeResponseDto
                {
                    Name = name,
                    Gender = data.Gender,
                    Probability = data.Probability,
                    Sample_size = data.Count,
                    Is_confident = isConfident,
                    Processed_at = DateTime.UtcNow.ToString("u")
                }
            };

        }
    }
}
