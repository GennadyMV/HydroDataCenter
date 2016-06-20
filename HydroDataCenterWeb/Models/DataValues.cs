using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HydroDataCenterWeb.Models
{
    public class DataValues
    {
        public int SiteCode;
        public String SiteName;

        public float Control
        {
            get
            {
                int count = 0;
                int summa = 0;
                foreach (var item in this.theValueList)
                {
                    if (item.percent != -32000)
                    {
                        count++;
                        summa += item.percent;
                    }
                }
                if (count == 0)
                {
                    return -32000;
                }
                return summa / count;
            }
        }
        public class Value
        {
            public DateTime Date;
            public DateTime DateUTC;

            public string valueAgk_
            {
                get
                {
                    if (this.valueAgk < -32000)
                    {
                        return "нет данных";
                    }
                    return this.valueAgk.ToString();
                }
            }

            public float valueAgk;



            public string valueHydroPost_
            {
                get
                {
                    if (this.valueHydroPost < -32000)
                    {
                        return "нет данных";
                    }
                    return this.valueHydroPost.ToString();
                }
            }
            public float valueHydroPost;

            public float delta;
            public int percent
            {
                get
                {
                    if (delta > -32000)
                    {
                        if (delta < 1)
                        {
                            return 100;
                        }
                        else {
                            if (delta < 3) {
                                return 75;
                            }
                            else
                                if (delta < 4) {
                                    return 50;
                                }
                                else
                                {
                                    return 0;
                                }
                        }
                    }
                    return -32000;

                  
                    
                }
            }

            public string delta_
            {
                get
                {
                    if (delta < -32000)
                    {
                        return "";
                    }
                    return delta.ToString();
                }
            }

            public string percent_
            {
                get
                {
                    if (percent != -32000)
                    {
                        return percent.ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
            }

            public Value()
            {

                delta = -32001;
                valueAgk = -32001;
                valueHydroPost = -32001;
            }
        }

        public List<Value> theValueList;

        public DataValues()
        {
            this.theValueList = new List<Value>();
        }

    }
}