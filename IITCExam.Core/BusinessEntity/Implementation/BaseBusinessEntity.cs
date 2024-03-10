using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IITCExam.Core.BusinessEntity.Implementation
{
    public class BaseBusinessEntity
    {
        public ILogger<BaseBusinessEntity> logger;


        public BaseBusinessEntity(ILogger<BaseBusinessEntity> logger)
        {
            this.logger = logger;
        }
    }
}
