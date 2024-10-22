﻿using Hymson.MES.HttpClients.Requests.Print;
using System.Net.Http.Json;

namespace Hymson.MES.HttpClients
{
    /// <summary>
    /// FastReport 打印请求
    /// </summary>
    public class FastReportPrintRequest : ILabelPrintRequest
    {
        private readonly HttpClient _httpClient;
        public FastReportPrintRequest(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// 上传文件到打印服务器
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<(string msg, bool result, string data)> GetTemplateContextAsync(string url)
        {
            try
            {
                string api = $"api/LabelPrint/gettc?url={url}";
                _httpClient.Timeout = TimeSpan.FromSeconds(5);
                var httpResponseMessage = await _httpClient.GetAsync(api);

                if (!httpResponseMessage.IsSuccessStatusCode) return (msg: "调用失败", result: false, data: "");

                using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                var r = await System.Text.Json.JsonSerializer.DeserializeAsync<PrintResponse>(contentStream);
                return (msg: r.Message, result: r.Success, data: r.Data);
            }
            catch (Exception ex)
            {
                return (msg: ex.Message, result: false, data: "");
            }
        }

        async Task<(string base64Str, bool result)> ILabelPrintRequest.PreviewFromImageBase64Async(PreviewRequest previewRequest)
        {
            string api = "api/LabelPrint/preview";
            var httpResponseMessage = await _httpClient.PostAsJsonAsync<PreviewRequest>(api, previewRequest);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                var r = await System.Text.Json.JsonSerializer.DeserializeAsync
                    <PrintResponse>(contentStream);
                return (base64Str: r.Data, result: r.Success);
            }
            return ("调用失败", false);
        }

        async Task<(string msg, bool result)> ILabelPrintRequest.PrintAsync(PrintRequest printRequest, bool ShowDialog)
        {
            string api = "api/LabelPrint/print";
            var httpResponseMessage = await _httpClient.PostAsJsonAsync<PrintRequest>(api, printRequest);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                var r = await System.Text.Json.JsonSerializer.DeserializeAsync
                    <PrintResponse>(contentStream);
                return (msg: r.Message, result: r.Success);
            }
            return ("调用失败", false);
        }

    }
}
