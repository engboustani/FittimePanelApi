using FittimePanelApi.Models.URLShortening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.Services
{
    public interface IURLShortening
    {
        public Task<IURLShorteningResponseDTO> ShortURL(string title, string url);
    }
}
