using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TaskScheduler.DTOs.BaseResponse
{
    public class BaseResultResponeVm
    {
        /// <summary>
        /// Результат виконання
        /// </summary>
        [JsonPropertyName("status")]
        public bool Status { get; set; }
        /// <summary>
        ///Текст повідомлення в разі не коректної відповіді
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("errors")]
        public List<string> Errors { get; set; }

        public BaseResultResponeVm()
        {
            Errors = new List<string>();
            Status = false;
            Message = "";
        }
    }
}
