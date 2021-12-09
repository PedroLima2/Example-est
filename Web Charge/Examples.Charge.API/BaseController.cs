using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Examples.Charge.Application.Common.Messages;
using Examples.Charge.Application.Messages.Response;

namespace Examples.Charge.API
{
    public class BaseController<TEntity> : ControllerBase where TEntity : class
    {
        protected new ActionResult Response(ResponseBase<TEntity> response)
        {
            if (response == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(new
                {
                    success = true,
                    data = response
                });
            }
        }

        protected new IActionResult Response(int? id = null, object response = null)
        {
            if (id == null)
            {
                return Ok(new
                {
                    success = true,
                    data = response
                });
            }
            else
            {
                return CreatedAtAction("Get", new { id },
                new
                {
                    success = true,
                    data = response ?? new object()
                });
            }
        }
    }
}
