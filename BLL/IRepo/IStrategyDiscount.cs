using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.IRepo
{
    public interface IStrategyDiscount
    {
        float GetFinalBill(float billAmt);
    }
}
