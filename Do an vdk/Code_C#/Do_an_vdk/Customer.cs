using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Do_an_vdk
{
    class Customer
    {
        private String name;
        private String bienSoXe;     
        private String id_Card;
        private String phone;
        private String CMND;
        private String startTime;
        private String endTime;

        public  Customer(String name, String bienSoXe, String phone, String id_Card, String CMND, String startTime, String endTime)
        {
            this.name = name;
            this.phone = phone;
            this.id_Card = id_Card;
            this.bienSoXe = bienSoXe;
            this.CMND = CMND;
            this.startTime = startTime;
            this.endTime = endTime;

        }
        public Customer()
        {

        }
        public String getName()
        {
            return this.name;
        }
        public String getBienSoXe()
        {
            return this.bienSoXe;
        }
        public String getCMND()
        {
            return this.CMND;
        }
        public String getPhone()
        {
            return this.phone;
        }
        public String getIdCard()
        {
            return this.id_Card;
        }
        public String getStartTime()
        {
            return this.startTime;
        }
        public String getEndTime()
        {
            return this.endTime;
        }

    }
}
