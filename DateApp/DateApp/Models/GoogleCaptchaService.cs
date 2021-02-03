using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DateApp.Models
{
    public class GoogleCaptchaService
    {
        private ReCaptchaSettings _settings { get; set; }
        public GoogleCaptchaService(IOptions<ReCaptchaSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<GoogleResponse> RecVer(string _Token)
        {
            GoogleReCaptchaData _MyData = new GoogleReCaptchaData()
            {
                response = _Token,
                secret = _settings.ReCaptcha_Secret_Key

            };


            GoogleResponse capresp = new GoogleResponse() {success=true,score=0.6 };
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetStringAsync($"https://www.google.com/recaptcha/api/siteverify?secret={_MyData.secret}&response={_MyData.response}");
                 capresp = JsonConvert.DeserializeObject<GoogleResponse>(response);
            }
            catch (Exception ex)
            {

            }


            return capresp;

        }



    }

    public class GoogleReCaptchaData
    {
        public string response { get; set; }
        public string secret { get; set; }

    }

    public class GoogleResponse
    {
        public bool success { get; set; }
        public double score { get; set; }
        public string action { get; set; }
        public DateTime challenge_ts { get; set; }
        public string HostName { get; set; }
    }


}
