using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math_Competition_Scorer
{
    public class Student
    {

        public string fName;
        public string lName;
        public string mName;
        public int clss;
        public int place;
        public int school;
        public string strSchool;
        public List<string> answers = new List<string>();
        public List<bool> answersTF = new List<bool>();
        public int score;
        public int count;
        public string stAns;
        public string stCls;
        public Student(string lN, string fN, string mN, int cls, int schl, string ss, string ans)
        {
            lName = lN;
            fName = fN;
            mName = mN;
            clss = cls;
            school = schl;
            strSchool = ss;
            stAns = ans;
            SetAnswers(ans);
            setClass(cls);
        }

        void SetLName(string lstNm)
        {
            lName = lstNm;
        }

        void SetFName(string fstNm)
        {
            fName = fstNm;
        }

        void SetMName(string mdlNm)
        {
            mName = mdlNm;
        }

        void SetClass(int cls)
        {
            clss = cls;
        }

        void SetSchool(int schl)
        {
             school = schl;
        }

        void SetSS (int schl)
        {
            strSchool = Master.schoolDict[schl];
        }

        void SetAnswers(string ans)
        {
            ans.Trim();
            string a;
            foreach (char c in ans)
            {
                a = c.ToString();
                answers.Add(a);
            }
        }

        void setClass(int cls)
        {
            if (cls == 49 || cls == 59)
            {
                stCls = "A";
            }

            else if (cls == 41 || cls == 51)
            {
                stCls = "AA";
            }

            else
            {
                stCls = "Not A Valid Class Number";
            }
        }

        public void CalcSocre(List<string> key, List<string> tiebreaker)
        {
            int count = 0;
            int TB1 = 0;
            int TB2 = 0;
            for (int i = 0; i < answers.Count - 1; i++)
            {
                if (key[i] == answers[i])
                {
                    count += 1;
                    answersTF.Add(true);
                }
                else
                {
                    answersTF.Add(false);
                }
            }

            for (int j = 0; j < tiebreaker.Count; j++)
            {
                if (tiebreaker[j] == "1" & answersTF[j] == true)
                {
                    TB1 = TB1 + 1;
                }
                else if (tiebreaker[j] == "2" & answersTF[j] == true)
                {
                    TB2 = TB2 + 1;
                }
            }
            SetScore(count * 10000 + TB1 * 100 + TB2, count);
        }

        void setPlace(int pl)
        {
            place = pl;
        }

        void SetScore(int scr, int cnt)
        {
            score = scr;
            count = cnt;
        }
        public int GetPlace()
        {
            return place;
        }

        public string GetFName()
        {
            return fName;
        }

        public string GetLName()
        {
            return lName;
        }

        public string GetMName()
        {
            return mName;
        }

        public int GetClss()
        {
            return clss;
        }

        public int GetSchool()
        {
            return school;
        }

        public List<string> GetAnswers()
        {
            return answers;
        }

        public int GetScore()
        {
            return score;
        }

        public static bool operator >(Student c1, Student c2)
        {
            int c1S = c1.GetScore();
            int c2S = c2.GetScore();
            return c1S > c2S;
        }

        public static bool operator <(Student c1, Student c2)
        {
            int c1S = c1.GetScore();
            int c2S = c2.GetScore();
            return c1S < c2S;
        }


        public int Compare(Student compareScore)
        {
            // A null value means that this object is greater.
            if (compareScore == null)
                return 1;

            else
                return this.score.CompareTo(compareScore.score);
        }
        public int ToCompare(Student schl)
        {
            if (schl == null)
                return 1;

            else
                return this.school.CompareTo(schl.school);
        }

    }


    public class StudentScore : IComparer<Student>
    {
        public int Compare(Student x, Student y)
        {
            // TODO: Handle x or y being null, or them not having names
            return x.score.CompareTo(y.score);
        }

        public int ToCompare(Student x, Student y)
        {
            return x.school.CompareTo(y.school);
        }
    }
}
