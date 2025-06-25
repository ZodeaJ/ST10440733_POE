using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10440733_PROG6221_POE
{
    public interface ICyberAdvice
    {
        //Methods for GetPasswordAdvice, GetPhishingAdvice and GetSafeBrowsingAdvice.
        public string GetPasswordAdvice();
        public string GetPhishingAdvice();
        public string GetSafeBrowsingAdvice();
        public string GetSocialEngineeringAdvice();
        public string GetPublicWifiSafetyAdvice();
        public string GetDeviceSafetyAdvice();
    }
}
