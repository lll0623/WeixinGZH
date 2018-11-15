namespace LesoftWuye2.Application.YsPay
{
    /// <summary>
    /// YsePay 的摘要说明
    /// </summary>
    public class JiShi
    {
        public JiShi()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        private string _method = "";
        private string _partner_id = "";
        private string _timestamp = "";
        private string _charset = "";
        private string _sign_type = "";
        private string _sign = "";
        private string _notify_url = "";
        private string _return_url = "";
        private string _version = "";

        public string method {
            get { return _method; }
            set { _method = value; }
        }
        public string partner_id
        {
            get { return _partner_id; }
            set { _partner_id = value; }
        }
        public string timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }
        public string charset
        {
            get { return _charset; }
            set { _charset = value; }
        }
        public string sign_type
        {
            get { return _sign_type; }
            set { _sign_type = value; }
        }
        public string sign
        {
            get { return _sign; }
            set { _sign = value; }
        }
        public string notify_url
        {
            get { return _notify_url; }
            set { _notify_url = value; }
        }
        public string return_url
        {
            get { return _return_url; }
            set { _return_url = value; }
        }
        public string version
        {
            get { return _version; }
            set { _version = value; }
        }
    }
}