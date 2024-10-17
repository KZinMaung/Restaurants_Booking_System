
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Helpers
{
    public class ApiRequestBase<T, U>
    {
        private static String API_URL = "";
        public static async Task<U> PostDiffRequest(string url, T entity, bool isCompressed = true)
        {
            try
            {
                url = API_URL + url;
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };

                using (var client = new HttpClient(handler))
                {
                    //client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", accessToken));
                    
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    using (var content = new StringContent(JsonConvert.SerializeObject(entity), UTF8Encoding.UTF8, "application/json"))
                    {
                        using (var response = await client.PostAsync(url, content))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                var objsJsonString = await response.Content.ReadAsStringAsync();
                                var jsonContent = JsonConvert.DeserializeObject<U>(objsJsonString);
                                return jsonContent;
                            }
                            else
                            {
                                return default(U);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return default(U);
            }


        }
    }



    public class ApiRequestBase<T>
    {
        private static String API_URL = "";
        public static async Task<T> Get(string url, bool isCompressed = false, string rootUrl = null)
        {

            if (rootUrl != null)
            {
                API_URL = rootUrl;
            }

            url = API_URL + url;
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            try
            {
                if (isCompressed)
                {
                    using (var client = new HttpClient(handler))
                    {
                        //client.DefaultRequestHeaders.Add("Authorization",string.Format("Bearer {0}",accessToken));
                        client.BaseAddress = new Uri(url);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        //client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                        //client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));

                        using (var response = await client.GetAsync(url))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                var objsJsonString = await response.Content.ReadAsStringAsync();
                                var jsonContent = JsonConvert.DeserializeObject<T>(objsJsonString);
                                return jsonContent;
                            }
                            else
                            {
                                return default(T);
                            }
                        }
                    }
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        //client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", accessToken));
                        client.BaseAddress = new Uri(url);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        using (var response = await client.GetAsync(url))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                var objsJsonString = await response.Content.ReadAsStringAsync();
                                var jsonContent = JsonConvert.DeserializeObject<T>(objsJsonString);
                                return jsonContent;
                            }
                            else
                            {
                                return default(T);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var x = ex;
                return default(T);
            }
        }

        public static async Task<T> GetRequest(string url, bool isCompressed = false, string rootUrl = null)
        {
            if (rootUrl != null)
            {
                API_URL = rootUrl;
            }
            url = API_URL + url;
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            try
            {
                if (isCompressed)
                {
                    using (var client = new HttpClient(handler))
                    {
                        //client.DefaultRequestHeaders.Add("Authorization",string.Format("Bearer {0}",accessToken));
                        client.BaseAddress = new Uri(url);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        //client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                        //client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
                        using (var response = await client.GetAsync(url))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                var objsJsonString = await response.Content.ReadAsStringAsync();
                                var jsonContent = JsonConvert.DeserializeObject<T>(objsJsonString);
                                return jsonContent;
                            }
                            else
                            {
                                return default(T);
                            }
                        }
                    }
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        //client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", accessToken));
                        client.BaseAddress = new Uri(url);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        using (var response = await client.GetAsync(url))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                var objsJsonString = await response.Content.ReadAsStringAsync();
                                var jsonContent = JsonConvert.DeserializeObject<T>(objsJsonString);
                                return jsonContent;
                            }
                            else
                            {
                                return default(T);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var x = ex;
                return default(T);
            }
        }
        public static async Task<T> DeleteRequest(string url)
        {
            url = API_URL + url;
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            try
            {
                using (var client = new HttpClient(handler))
                {
                    //client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", accessToken));
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    using (var response = await client.DeleteAsync(url))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var objsJsonString = await response.Content.ReadAsStringAsync();
                            var jsonContent = JsonConvert.DeserializeObject<T>(objsJsonString);
                            return jsonContent;
                        }
                        else
                        {
                            return default(T);
                        }
                    }
                }
            }
            catch
            {
                return default(T);
            }
        }
        public static async Task<T> PostRequest(string url, T entity, string rootUrl = null)
        {
            if (rootUrl != null)
            {
                API_URL = rootUrl;
            }
            url = API_URL + url;
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            using (var client = new HttpClient(handler))
            {
                //client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", accessToken));
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                //client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
                using (var content = new StringContent(JsonConvert.SerializeObject(entity), UTF8Encoding.UTF8, "application/json"))
                {
                    using (var response = await client.PostAsync(url, content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var objsJsonString = await response.Content.ReadAsStringAsync();
                            var jsonContent = JsonConvert.DeserializeObject<T>(objsJsonString);
                            return jsonContent;
                        }
                        else
                        {
                            return default(T);
                        }
                    }
                }
            }
        }

        
    }
}
