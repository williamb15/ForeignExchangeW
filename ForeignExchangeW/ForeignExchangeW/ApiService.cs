

namespace ForeignExchangeW
{

    public class ApiService
    {
        public async Task<Responce> GetList<T>(string urlBase, string controller)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new
                    Uri(urlBase);
                var response = await client.GetAsync(controller);
                var result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Responce
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }
                var List = JsonConvert.DeserializeObject<List<Rates>>(result);

                return new Responce
                {
                    IsSuccess = true,
                    result = List,
                };
            }
            catch (Exception ex)
            {
                return new Responce
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
    }
}
