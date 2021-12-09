using System.Collections.Generic;

namespace Examples.Charge.Application.Common.Messages
{
    public class ResponseBase<TEntityDto> where TEntityDto : class
    {
        public bool Success { get; set; } = true;

        public IEnumerable<object> Errors { get; set; } = null;

        public List<TEntityDto> Objects { get; set; }
    }
}
