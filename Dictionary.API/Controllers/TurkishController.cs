using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Dictionary.API.Base;
using Dictionary.API.Helpers;
using Dictionary.API.Models.Request;
using Dictionary.Data.Entities;
using Dictionary.Domain.Enums;
using Dictionary.Domain.Infrastructures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dictionary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TurkishController : BaseApiController
    {
        private readonly ApiSettings _appSettings;
        public TurkishController(IOptions<ApiSettings> appSettings) : base(appSettings)
        {
            _appSettings = appSettings.Value;
        }


        [HttpGet("GetWordMeanings")]
        public async Task<ApiReturn<TrWord>> GetWordMeaning(string request)
        {
            try
            {
                var query = await TurkishDictionaryDb.Words.Include(x => x.Meanings)
                                                                .ThenInclude(meaning => meaning.WordTypes)
                                                          .Include(x => x.Sayings)
                                                          .FirstOrDefaultAsync(x => x.Text == request.ToLower() || x.TextSimple == request.ToLower());

                if (query == null)
                {
                    var word = await TdkRequest(request);

                    if (word is null)
                    {
                        return new ApiReturn<TrWord>
                        {
                            Data = null,
                            Success = true,
                            Code = ApiStatusCode.NotFound,
                            Message = "Not Found"
                        };
                    }

                    if (TurkishDictionaryDb.Words.FirstOrDefault(x => x.TextSimple == word.TextSimple) == null)
                    {
                        await TurkishDictionaryDb.Words.AddAsync(word);

                        await TurkishDictionaryDb.SaveChangesAsync();

                        return new ApiReturn<TrWord>
                        {
                            Data = word,
                            Success = true,
                            Code = ApiStatusCode.Success,
                            Message = "Meanings listed succesfully"
                        };
                    }
                    else
                    {
                        query = await TurkishDictionaryDb.Words.Include(x => x.Meanings)
                                                                  .ThenInclude(meaning => meaning.WordTypes)
                                                               .Include(x => x.Sayings).FirstOrDefaultAsync(x => x.TextSimple == word.TextSimple);
                        return new ApiReturn<TrWord>
                        {
                            Data = query,
                            Success = true,
                            Code = ApiStatusCode.Success,
                            Message = "Meanings listed succesfully"
                        };

                    }


                }
                else
                {
                    return new ApiReturn<TrWord>
                    {
                        Data = query,
                        Success = true,
                        Code = ApiStatusCode.Success,
                        Message = "Meanings listed succesfully"
                    };
                }


            }
            catch (Exception ex)
            {
                return Error<TrWord>(new ApiErrorCollection
                {
                    new ApiError
                    {
                        Code = ApiStatusCode.InternalServerError,
                        InternalMessage = ex.StackTrace,
                        Message = ex.Message
                    }
                });
            }
        }

        [HttpPost("SentenceMeanings")]
        public async Task<ApiReturn<int>> WordsMeanings([FromBody] SentenceRequest request)
        {
            int cnt = 0;
            try
            {
                var punctuation = request.Text.Where(Char.IsPunctuation).Distinct().ToArray();
                var words = request.Text.Split().Select(x => x.Trim(punctuation));

                if (TurkishDictionaryDb.Sayings.FirstOrDefault(saying => saying.Text == request.Text.ToLower()) is null)
                {
                    foreach (var item in words)
                    {
                        var query = await TurkishDictionaryDb.Words.Include(x => x.Meanings)
                                                                    .ThenInclude(meaning => meaning.WordTypes)
                                                              .Include(x => x.Sayings)
                                                              .FirstOrDefaultAsync(x => x.Text == item.ToLower() || x.TextSimple == item.ToLower());

                        if (query == null)
                        {
                            var word = await TdkRequest(item);

                            if (word != null)
                            {
                                if (TurkishDictionaryDb.Words.FirstOrDefault(x => x.TextSimple == word.TextSimple) is null)
                                {
                                    await TurkishDictionaryDb.Words.AddAsync(word);

                                    cnt++;
                                }
                            }
                        }


                    }
                }

                await TurkishDictionaryDb.SaveChangesAsync();

                return new ApiReturn<int>
                {
                    Data = cnt,
                    Success = true,
                    Code = ApiStatusCode.Success,
                    Message = "Meanings saved successfully"
                };
            }
            catch (Exception ex)
            {
                return Error<int>(new ApiErrorCollection
                {
                    new ApiError
                    {
                        Code = ApiStatusCode.InternalServerError,
                        InternalMessage = ex.StackTrace,
                        Message = ex.Message
                    }
                });
            }
        }

        [NonAction]
        private async Task<TrWord> ConvertArrayToWordAsync(JArray array)
        {

            var word = new TrWord
            {
                Text = array[0]["madde"].ToString(),
                TextSimple = array[0]["madde_duz"].ToString(),
                TdkId = Convert.ToInt32(array[0]["madde_id"])
            };

            if (Convert.ToInt16(array[0]["ozel_mi"]) == 1)
            {
                word.isPrivate = true;
            }

            if (Convert.ToInt16(array[0]["cogul_mu"]) == 1)
            {
                word.isPlural = true;
            }

            word.MeaningNumber = Convert.ToInt32(array[0]["anlam_say"]);

            foreach (var item in array[0]["anlamlarListe"])
            {
                TrMeaning meaning = new TrMeaning
                {
                    MeaningText = item["anlam"].ToString(),
                    isVerb = Convert.ToInt16(item["fiil"]) == 1 ? true : false
                };


                if (item["ozelliklerListe"] != null)
                {
                    foreach (var itemFeature in item["ozelliklerListe"])
                    {
                        //var type = ConvertStringToWordType(itemFeature["tam_adi"].ToString());
                        var type = await TurkishDictionaryDb.WordTypes.FirstOrDefaultAsync(x => x.TdkText == itemFeature["tam_adi"].ToString().Trim());

                        if (type is null)
                        {
                            var a = new object();
                        }


                        TrMeaningWordType meaningWordType = new TrMeaningWordType
                        {
                            WordType = type
                        };

                        meaning.WordTypes.Add(meaningWordType);

                    }

                }

                word.Meanings.Add(meaning);

            }

            if (array[0]["atasozu"] != null)
            {
                foreach (var itemSaying in array[0]["atasozu"])
                {
                    TrSaying saying = new TrSaying
                    {
                        Text = itemSaying["madde"].ToString()
                    };

                    word.Sayings.Add(saying);
                }
            }


            return word;

        }


        [NonAction]
        private async Task<TrWord> TdkRequest(string request)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string getUrl = _appSettings.BaseURL + _appSettings.MeaningURL + request;


            HttpResponseMessage result = await httpClient.GetAsync(getUrl);
            string resultString = result.Content.ReadAsStringAsync().Result;

            if (resultString != "{\"error\":\"Sonuç bulunamadı\"}")
            {
                var responseArray = JsonConvert.DeserializeObject<JArray>(resultString);

                var response = await ConvertArrayToWordAsync(responseArray);

                return response;
            }
            else
            {
                return null;
            }

        }
    }
}