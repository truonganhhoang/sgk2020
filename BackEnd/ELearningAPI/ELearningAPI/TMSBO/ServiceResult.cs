using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMSBO
{
    public class ServiceResult
    {
        public ServiceResult()
        {
            Success = true;
            Code = Enumeration.HttpStatusCode.OK;
        }
        private bool _success;
        private Enumeration.HttpStatusCode? m_Code;
        public Enumeration.HttpStatusCode? Code
        {
            get { return m_Code; }
            set
            {
                m_Code = value;
                if (m_Code != Enumeration.HttpStatusCode.OK)
                {
                    Success = false;
                }
                else
                {
                    Success = true;
                }
            } 
        }
        public string Error { get; set; }
        public List<ValidateResult> ValidateInfo { get; set; }
        public long Total { get; set; }
        public object Data { get; set; }
        public bool Success
        {
            get
            {
                return _success;
            }
            set
            {
                if (!value)
                {
                    m_Code = Enumeration.HttpStatusCode.ServerError;
                }
                _success = value;
            }
        }

        public override string ToString()
        {
            string strDes = string.Empty;
            if (this.Success)
            {
                strDes = string.Format("Success: {0} - Code: {1}", Success, Code);
            }
            else
            {
                if(ValidateInfo != null)
                {
                    strDes = string.Format("Failed - Code: {0} - Message: {1}", Code, CommonFn.SerializeObject(ValidateInfo));
                }
            }
            return strDes;
        }
        public void SetError(Exception ex, string sInfo = "")
        {
            this.Code = Enumeration.HttpStatusCode.ServerError;
            this.Error = ex.Message;
            this.Success = false;
            CommonLog.CommonErrorLog(ex, sInfo);
        }
        public void SetValue(bool success, Enumeration.HttpStatusCode code, string errorMessage="", object data = null)
        {
            this.Success = success;
            this.Code = code;
            this.Data = data;
            this.ToString();
        }
    }
}
