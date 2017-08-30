using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dailyAccount
{
    class user
    {
        int userid;
        int location;
        string username;
        int userclass;
        float commission;
        float persion365com;
        float comp365com;
        

        public int Userid
        {
            get
            {
                return userid;
            }

            set
            {
                userid = value;
            }
        }

        public int Location
        {
            get
            {
                return location;
            }

            set
            {
                location = value;
            }
        }

        public string Username
        {
            get
            {
                return username;
            }

            set
            {
                username = value;
            }
        }

        public int Userclass
        {
            get
            {
                return userclass;
            }

            set
            {
                userclass = value;
            }
        }

        public float Commission
        {
            get
            {
                return commission;
            }

            set
            {
                commission = value;
            }
        }

        public float Persion365com
        {
            get
            {
                return persion365com;
            }

            set
            {
                persion365com = value;
            }
        }

        public float Comp365com
        {
            get
            {
                return comp365com;
            }

            set
            {
                comp365com = value;
            }
        }
    }
}
