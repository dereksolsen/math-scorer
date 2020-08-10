using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math_Competition_Scorer
{
    public class Team
    {
        public string school;
        public int district;
        public int place;
        public List<Student> schoolStudents = new List<Student>();
        public Team (string scl, int dis)
        {
            school = scl;
            district = dis;
        }

        void AddStudents(Student s)
        {
            schoolStudents.Add(s);
        }

        void setSchool (string s)
        {
            school = s;
        }
        public string getSchool()
        {
            return school;
        }

        void setDistrict (int dis)
        {
            district = dis;
        }
        public int getDistrict()
        {
            return district;
        }

        void setPlace(int pl)
        {
            place = pl;
        }
        int getPlace()
        {
            return place;
        }
    }
}
