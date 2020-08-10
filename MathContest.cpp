/*  _______________________________________________________________________
                                                                         
                    Northern Minnesota Mathematics Contest               
	            Department of Mathematics and Computer Science
				           Bemidji State University

                    Scoring and Report Generation Program
                                Version 1.1.1

                            Author: James L. Richards
							Modifications: Glen Richgels
                                October 2009
    _______________________________________________________________________
*/

#include <iostream>
#include <iomanip>
#include <fstream>
#include <algorithm>
#include <string>
#include <stack>
#include "conwin2.h"
using namespace std;

#include "StudentInfo.h"
#include "Team.h"

const string YEAR = "2009",
			 ANNUAL = "37TH",
			 CONTEST_DATE = "OCTOBER 29, 2009";

Console screen(80, 40, 
		ANNUAL+" ANNUAL NORTHERN MINNESOTA MATHEMATICS CONTEST --  "
		+CONTEST_DATE);

class School
{
public:
	int district;
	string name;
};

void Fill(vector<School>& school)
// Fills the school vector with School information read from a file.
{
	ifstream in;
	School newSchool;

	in.open("MNSchools.txt");
	for (;;)
	{
		in >> newSchool.district;
		if (in.eof()) break;
		while (char(in.peek()) == ' ') in.get(); // skip spaces
		getline(in, newSchool.name);
		school.push_back(newSchool);
	}
}

string Find(int number, const vector<School>& school)
// Locates and returns the name of the school district whose number is
// specified.  "??????" is returned when the school district name is
// unknown.
{
	for (int k = 0; k < int(school.size()); k++)
		if (school[k].district == number)
			return school[k].name;
	return "??????";
}

string StringForm(int num, int width)
// Creates and returns a string representation for the integer num using
// blank spaces to pad on the left to fill out a field with the specified
// minimum width.
{
	string str;
	int k = 0, val = abs(num);

	do // digit-by-digit conversion to char
	{
		str = char(val % 10 + '0') + str;
		val /= 10;
	} while (val != 0);
	if (num < 0)
		str = '-' + str;
   if (int(str.size()) < width)
      str.insert(int(0), width-int(str.size()), ' ');
	return str;
}

void TrimSpaces(string& str)
// Removes all leading and trailing whitespace characters from str.
{
	while (!str.empty() && isspace(str[0]))
		str = str.erase(0,1);
	while (!str.empty() && isspace(str[str.size()-1]))
		str = str.erase(str.size()-1, 1);
}

string Center(const string& str, int width=80)
// Adds spaces to the beginning and end of str so that the original string
// in str is centered in a field having the specified width when sent to an
// output stream.
{
	int k = int(str.length());
	return string((width-k)/2, ' ') + str;
}

void DisplayHeader(ostream& out, int w, string yr, string annual, string date)
// Writes header information to the out stream for a report file.
{
	out << Center(annual + " ANNUAL NORTHERN MINNESOTA MATHEMATICS CONTEST", w)
		<< endl
		<< Center(date,w) 
		<< endl << endl
		<< Center("Department of Mathematics and Computer Science",w)
		<< endl
		<< Center("Bemidji State University",w)
		<< endl << endl;
}

void GetData(string fileName, vector<StudentInfo>& stud, 
			 const vector<School>& school, string& key, string& ties)
// Reads data for a student vector (stud), test key, and tiebreakers (ties)
// from the specified file gerated by test scoring software.
{
	ifstream input;
	string   record, last, first, initial, schl, test;
	int      div, district;

	input.open(fileName.data());
	if (!input.is_open())
	{
		cout << "File not found: " + fileName << "\n\n";
		cin.get();
		exit(0);
	}

	getline(input, key); key = key.substr(45, 40);
	getline(input, ties); ties = ties.substr(45, 40);
	for(;;)
	{
		getline(input, record);
		if (record.size() == 0 || input.eof()) break;
		last = record.substr(0, 20);
		TrimSpaces(last);
		if (last.empty()) last = "Student";
		first = record.substr(20, 12);
		TrimSpaces(first);
		if (first.empty()) first = "Unknown";
		initial = record.substr(32, 2);
		first += initial;
		div = atoi(record.substr(35,2).data());
		district = atoi(record.substr(38,6).data());
		test = record.substr(45,40);
		schl = Find(district, school);
		stud.push_back(StudentInfo(last,first,div,schl,test));		
	}
	input.close();
}

void ScoreTests(const string& division, vector<StudentInfo>& stud, 
				const string& key, const string& ties)
// Uses student test data (stud), an answer key, and tiebreakers (ties) to score
// the tests for the specified division and write an item analysis file.
{
	int item[40][6] = {0}; // table for item analysis

	for (int k = 0; k < int(stud.size()); k++)
	{
		int pts = 0, tb1 = 0, tb2 = 0;
		for (int i = 0; i < 40; i++)
		{
			if (stud[k].Test()[i] < '1' || stud[k].Test()[i] >'5')
				item[i][0] += 1;
			else
				item[i][int(stud[k].Test()[i])-48] += 1;  // item analysis
			if (stud[k].Test()[i] == key[i])      // correct answer
			{
				pts++;
				if (ties[i] == '1') tb1++;
				if (ties[i] == '2') tb2++;
			}
		}
		stud[k].SetScore(10000*pts + 100*tb1 + tb2);
	}

	ofstream out;

	out.open((division+YEAR+"Scores.txt").data());
	DisplayHeader(out, 120, YEAR, ANNUAL, CONTEST_DATE);
	out << Center(division + " Division Coded Scores With Tiebreakers",120) 
		<< "\n\n";
	out << "               1111111111222222222233333333334\n"
		<< "      1234567890123456789012345678901234567890\n"
		<< "      ----------------------------------------\n";
	out << "Key:  " + key + "\n";
	out << "Ties: " + ties + "\n\n";
	for (int k = 0; k < int(stud.size()); k++)
		out << right << setw(6) << stud[k].Score() << " "
			<< left << setw(36) << stud[k].NameLastFirst()			
			<< left << setw(10) << stud[k].DivisionAndClass()
			<< left << setw(25) << stud[k].School()
			<< stud[k].Test() << right << endl;

	out << '\n' << stud.size() << " students processed.\n\n";
	out << "\f\n";
	DisplayHeader(out, 120, YEAR, ANNUAL, CONTEST_DATE);
	out << Center(division+" Test Item Analysis", 120) << "\n\n"
		<< "Question    NA     1      2      3      4      5     TBR\n"
		<< "--------   ---    ---    ---    ---    ---    ---    ---\n";
	for (int i = 0; i < 40; i++)
	{
		float tot = 0.0;
		float corr;
		out << setw(5) << i+1 << "   ";
		for (int j = 0; j < 6; j++)
		{
			tot += item[i][j];
			out << setw(6) << item[i][j];
			if (key[i]-48 == j)
			{
				corr = float(item[i][j]);
				out << "*";
			}
			else
				out << " ";
		}
		if (ties[i] == '1')
			out << setw(6) << 'A';
		else if (ties[i] == '2')
			out << setw(6) << 'B';
		else
			out << setw(6) << ' ';
		out << "   " << fixed << showpoint << setprecision(1) 
			<< setw(6) << corr/tot * 100.0 << "%";
		out << endl;
	}
	out.close();
	//cout << "   File: " + division + "Scores.txt\n";
}

void SchoolScores(const string& divName, vector<StudentInfo>& student, 
				  vector<Team>& team)
// Calculates team scores for a particular division (divName) and sorts the 
// student components alphabetically by school and numerically within school
// groups so that the records are in descending score order within each group.
{
	ofstream out;
	string   schoolName = "", 
			 schoolClass = "";
	int		 scoreCount = 3, 
			 total;


	for (int k = 0; k < int(student.size()); k++)
	{
		if (student[k].School() != schoolName)
		{
			if (scoreCount < 3)
				team.push_back(Team(schoolName, schoolClass, total));
			schoolName = student[k].School();
			schoolClass = student[k].DivisionAndClass().substr(7,2);
			scoreCount = 1;
			total = student[k].Score();
		} 
		else if (scoreCount < 3)
		{
			scoreCount++;
			total += student[k].Score();
			if (scoreCount == 3)
				team.push_back(Team(schoolName, schoolClass, total));
		}
	}

	sort(team.begin(), team.end());

	out.open((divName+YEAR+"TeamScores.txt").data());
	DisplayHeader(out, 80, YEAR, ANNUAL, CONTEST_DATE);
	out << Center(divName + " Team Results", 80) << "\n\n"
		<< "      Team\n"
		<< "Rank  Score  Ties\n"
		<< "----  -----  ----\n";
	for (int i = int(team.size())-1; i >= 0; i--)
		out << setw(3) << int(team.size())-i << ".  "
			<< setw(4) << team[i].Total() / 10000
			<< "   " << setfill('0') << setw(4) << team[i].Total() % 10000
			<< setfill(' ') << "  " << team[i].Class()
			<< "  " << team[i].Name()
			<< endl;
	out.close();
}

void Arrange(const string& divName, vector<StudentInfo>& student)
// Uses student information that includes test scores to write a file containing 
// individual school reports for the specified division (divName).
{
	int teamScore = 0, 
		studentCount = 0;
	sort(student.begin(), student.end(), LessThanBySchool);

	ofstream out;
	string currentSchool = student[0].School();

	out.open((divName+YEAR+"SchoolGroups.txt").data());
	for (int k = 0; k < int(student.size()); k++)
	{
		if (studentCount == 0)
		{
			DisplayHeader(out, 80, YEAR, ANNUAL, CONTEST_DATE);
			out << currentSchool << ' ' 
				<< student[k].DivisionAndClass() << endl << endl;
		}
		studentCount ++;
		if (studentCount <= 3) 
			teamScore += student[k].Score() / 10000;
		out << setw(2) << right << student[k].Score() / 10000 
			//<< "  " << setw(4) << setfill('0') << student[k].Score() % 10000
			<< setfill(' ') << left;
		if (studentCount <= 3)
			out << "  * ";
		else
			out << "    ";
		out	<< setw(35) << student[k].NameLastFirst()
			//<< "  " << setw(9) << student[k].DivisionAndClass()
			//<< "  " << student[k].School()
			<< right << endl;
		if (k == int(student.size())-1 
			|| (k < int(student.size())-1 
			&& student[k+1].School() != currentSchool))
		{
			out << "\nTeam Score: " << teamScore 
				<< "\n\n* Team members awarded a T-shirt.\n";
			if (studentCount > 1)
				out << "\n" << studentCount << " students participated.\n";
			else
				out << "\n" << studentCount << " student participated.\n";
			if (k < int(student.size())-1)
			{
				out << "\f\n";
				currentSchool = student[k+1].School();
				teamScore = 0;
				studentCount = 0;
			}
		}
	}
	out.close();
	//cout << "   File: " + divName + "SchoolGroups.txt\n";
}

void Graphs(const string& divName, const vector<StudentInfo>& student, 
			const vector<Team>& team)
// Uses student and team data after grading is complete to write a bar graph
// showing scores distribution and team totals for a specified division 
// (divName).
{
	int freq[41] = {0};
	int total = 0;
	const char MARK = '*';

	for (int k = 0; k < int(student.size()); k++)
	{
		int score = student[k].Score()/10000;
		freq[score]++;
		total += score;
	}

	ofstream out;

	out.open((divName+YEAR+"Graph.txt").data());
	DisplayHeader(out, 80, YEAR, ANNUAL, CONTEST_DATE);
	out << Center(divName + " Division Frequency Distribution", 80)
		<< "\n\n"
		<< "Total Scores: " << student.size() << "\n"
		<< "Average Score: " << fixed << setprecision(1) << showpoint 
		<< float(total) / float(student.size())
		<< endl << endl;
	for (int i = 0; i < 41; i++)
	{
		out << setw(2) << i << ": " << setw(2) << freq[i] << ' ';
		if (freq[i] > 0) out << string(freq[i], MARK);
		out << endl;
	}

	// Team scores
	out << "\n" << divName+" Division Team Scores\n";
	for (int k = int(team.size())-1; k >= 0 ; k--)
		out << team[k].Total()/10000 << ' ';

	out.close();
	//cout << "   File: " + divName + "Graph.txt\n";
}

void Rank(const string& divName, vector<StudentInfo>& student)
// Uses student data including test scores to write a file that contains the
// ranging for all students in the specified division (divName).
{
	sort(student.begin(), student.end());

	ofstream out;

	out.open((divName+YEAR+"Ranking.txt").data());
	DisplayHeader(out, 80, YEAR, ANNUAL, CONTEST_DATE);
	out << Center(divName+" Division", 80) << "\n\n"
		<< Center("INDIVIDUAL RESULTS (INCLUDING TIEBREAKERS)", 80) << "\n\n"
		<< "Rank  Scr  Ties\n"
		<< "----  ---  ----\n";
	for (int k = int(student.size())-1; k >= 0; k--)
	{
		out	<< setw(3) << int(student.size())-k << ".  "
			<< setw(3) << right << student[k].Score() / 10000 
			<< "  " << setw(4) << setfill('0') << student[k].Score() % 10000
			<< setfill(' ') << left
			<< "  " << setw(35) << student[k].NameLastFirst()
			<< "  " << setw(9) << student[k].DivisionAndClass()
			<< "  " << student[k].School()
			<< right << endl;
	}
	out.close();
	//cout << "   File: " + divName + "Ranking.txt\n";
}

void Validate(const string& division, const vector<StudentInfo>& stud, 
			 const string& key, const string& ties)
// Attempts to read the data file produced by the test grading software for the
// specified division. If errors are detected, appropriate messages are inserted
// into the cout stream. When there are no errors, the student vector (stud),
// test key, and tiebreakers (ties) are ready for further processing.
{
	ofstream out;

	cout << "\nValidating " << division << " Division Data\n";
	out.open((division+YEAR+"Validation.txt").data());
	DisplayHeader(out, 120, YEAR, ANNUAL, CONTEST_DATE);
	out << Center(division + " Division", 120) << "\n\n";
	out << "               1111111111222222222233333333334\n"
		<< "      1234567890123456789012345678901234567890\n"
		<< "      ----------------------------------------\n";
	out << "Key:  " + key + "\n";
	out << "Ties: " + ties + "\n\n";
	if (key.length() == 40)
	{		
		out << "Answers for 40 questions in the key.\n";
		out << "Checking individual correct answers...\n";
		int errors = 0;
		for (int k = 0; k < 40; k++)
			if (key[k] < '1' || key[k] > '5')
			{
				out << "#" << k+1 << " answer '" << key[k] << "' is invalid.\n";
				errors++;
			}
		out << errors << " errors in the answer key.\n\n";
	}
	else
	{
		out << "ERROR: Key contains only " << key.length() << " answers.\n\n";
		cout << "ERROR: Key contains only " << key.length() << " answers.\n";
	}

	if (ties.length() == 40)
	{		
		out << "Tiebreakers marks for 40 answers.\n";
		out << "Checking individual tiebreakers...\n";
		int errors = 0;
		for (int k = 0; k < 40; k++)
			if (!(ties[k] == '1' || ties[k] == '2' || ties[k] == '*'))
			{
				out << "#" << k+1 << " tiebreaker '" << ties[k] << "' is invalid.\n";
				errors++;
			}
		out << errors << " tiebreaker errors.\n\n";
	}
	else
	{
		out << "ERROR: " << ties.length() << " is Too few tiebreaker marks.\n\n";
		cout << "ERROR: " << ties.length() << " is too few tiebreaker marks.\n";
	}

	int errors = 0;
	for (int k = 0; k < int(stud.size()); k++)
	{
		if (stud[k].NameFirstLast() == "Unknown Student"
			|| stud[k].DivisionAndClass() == "*********"
			|| stud[k].School() == "??????")
		{
			out << "<> ";
			errors++;
		}
		else
			out << "OK ";
		out << left << setw(36) << stud[k].NameLastFirst()			
			<< left << setw(10) << stud[k].DivisionAndClass()
			<< left << setw(25) << stud[k].School()
			<< stud[k].Test() << right << endl;
	}

	out << '\n' << stud.size() << " students processed.\n\n";
	out << errors << " student records contain one or more errors.\n\n";
	cout << errors << " student records contain one or more errors.\n";
	out.close();
	cout << "Output file: " + division + "Validation.txt\n";
	cout << division << " validation has ended.\n";
}

void AwardsScript()
// Writes the opening text for the awards ceremony to the Awards.txt file.
{
	ifstream in;
	ofstream out;

	in.open("AwardsIntro.txt");
	out.open("Awards.txt");
	for(;;)
	{
		string line;
		getline(in, line);
		if (in.eof()) break;
		out << line << endl;
	}
	out << "\f\n";
	in.close();
	out.close();
}

void IndividualAwards(vector<StudentInfo>& jrStudent,
					vector<StudentInfo>& srStudent)
// Appends a listing of individual award winners for both divisions to the end
// of the Awards.txt file.
{
	ofstream out;

	out.open("Awards.txt", ios::app);

	out << "\n\nTOP THREE INDIVIDUAL SCORERS ON EACH TEAM"
		<< "\n-------------------------------------------"
		<< "\nIf you are one of the top three scorers for your team, you are"
		<< "\na T-shirt winner. Will all the T-shirt winners please stand."
		<< "\nLet's give all these winners a round of applause.\n";

	sort(jrStudent.begin(), jrStudent.end());
	reverse(jrStudent.begin(), jrStudent.end());

	int jrN = 9;
	for (;;)
		if (jrStudent[jrN+1].Score()/10000 == jrStudent[9].Score()/10000)
			jrN++;
		else
			break;

	out << "\n\nJUNIOR DIVISION INDIVIDUAL AWARDS"
	    << "\n---------------------------------"
		<< "\n\nJunior Division Honorable Mention:\n\n";
	if (jrN == 9)
		out << "       none\n\n";
	else
		for (int k = jrN; k > 9; k--)
			out << setw(7) << " " << jrStudent[k].NameFirstLast() << ", " 
				<< jrStudent[k].School() << "\n\n";
	out << endl;

	out << "Junior Division Top Ten:\n\n";
	for (int k = 9; k >= 0; k--)
		out << setw(2) << k+1 << " --> " 
			<< jrStudent[k].NameFirstLast() << ", " 
			<< jrStudent[k].School() << "\n\n";

	sort(srStudent.begin(), srStudent.end());
	reverse(srStudent.begin(), srStudent.end());

	int srN = 9;
	for (;;)
		if (srStudent[srN+1].Score()/10000 == srStudent[9].Score()/10000)
			srN++;
		else
			break;
	out << "\f\n";

	out << "\n\nSENIOR DIVISION INDIVIDUAL AWARDS"
	    << "\n---------------------------------"
		<< "\n\nSenior Division Honorable Mention:\n\n";
	if (srN == 9)
		out << "       none\n\n";
	else
		for (int k = srN; k > 9; k--)
			out << setw(7) << " " << srStudent[k].NameFirstLast() << ", " 
				<< srStudent[k].School() << "\n\n";
	out << endl;
		
	out << "Senior Divsion Top Ten:\n\n";
	for (int k = 9; k >= 0; k--)
		out << setw(2) << k+1 << " --> " 
			<< srStudent[k].NameFirstLast() << ", " 
			<< srStudent[k].School() << "\n\n";

	out << "\f\n";
	out.close();
}

void TeamAwards(const vector<Team>& jrTeam,
			    const vector<Team>& srTeam)
// Appends a listing of team award winners for both divisions to the end
// of the Awards.txt file.
{
	ofstream out;

	out.open("Awards.txt", ios::app);

	out << "\n\nJUNIOR DIVISION TEAM AWARDS"
	    << "\n---------------------------\n";
	stack<string> junior;
	int n = 0;
	for(int k = int(jrTeam.size())-1; k >= 0; k--)
	{
		if (n < 2 && jrTeam[k].Class() == "AA")
		{
			junior.push(jrTeam[k].Name());
			n++;
		}
	}
	while (n > 0)
	{
		out << "\nClass AA - " << n-- << ": " << junior.top() << "\n";
		junior.pop();
	}
	out << endl;
	n = 0;
	for(int k = int(jrTeam.size())-1; k >= 0; k--)
	{
		if (n < 4 && jrTeam[k].Class() == " A")
		{
			junior.push(jrTeam[k].Name());
			n++;
		}
	}
	while (n > 0)
	{
		out << "\nClass  A - " << n-- << ": " << junior.top() << "\n";
		junior.pop();
	}
		
	out << "\n\nSENIOR DIVISION TEAM AWARDS"
	    << "\n---------------------------\n";
	stack<string> senior;
	n = 0;
	for(int k = int(srTeam.size())-1; k >= 0; k--)
	{
		if (n < 2 && srTeam[k].Class() == "AA")
		{
			senior.push(srTeam[k].Name());
			n++;
		}
	}
	while (n > 0)
	{
		out << "\nClass AA - " << n-- << ": " << senior.top() << "\n";
		senior.pop();
	}
	out << endl;
	n = 0;
	for(int k = int(srTeam.size())-1; k >= 0; k--)
	{
		if (n < 4 && srTeam[k].Class() == " A")
		{
			senior.push(srTeam[k].Name());
			n++;
		}
	}
	while (n > 0)
	{
		out << "\nClass  A - " << n-- << ": " << senior.top() << "\n";
		senior.pop();
	}
	
	out << "\n\nHow about another round of applause for all the Math Contest\n" 
		<< "winners.  Thank you all for participating and hope to see you\n"
		<< "again next year!\n";
	out.close();
}

void ClearLines(short first, short last)
{
	for (int k = first; k <= last; k++)
	{
		screen.PutCursor(0,k);
		screen.ClearLine();
	}
}

void SelectFromMenu(char& option)
// Presents a menu of options and waits for an option to be selected.
{
	screen.PutCursor(11,7);
	screen.ClearEOL();
	//cin >> option;
	cin.get(option);
	cin.ignore(256, '\n');
	option = toupper(option);
	ClearLines(8,20);
	screen.PutCursor(0,9);
}

void DisplayMenu()
{
	screen.SetBackColor(GREEN);
	screen.SetForeColor(WHITE);
	screen.Clear();
	cout << "\nM E N U\n======="
		 << "\n   P     Process"
		 << "\n   V     Validate"
		 << "\n   X     Exit"
		 << "\n\nSelection: ";
}

//<><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>

void main()
{
	vector<StudentInfo> studentJR, studentSR;
	vector<School>		school;
	string				keyJR, tiesJR,
						keySR, tiesSR;
	vector<Team>        teamJR,
						teamSR;

	char option;
	bool finished = false;

	Fill(school);

	GetData("Junior"+YEAR+".txt", studentJR, school, keyJR, tiesJR);
	GetData("Senior"+YEAR+".txt", studentSR, school, keySR, tiesSR);

	DisplayMenu();
	do
	{
		SelectFromMenu(option);
		switch (option)
		{
		case 'P': // Process
			screen.SetForeColor(YELLOW);
			AwardsScript();
			cout << "\nAwards ceremony script initialized.";
			cout << "\nStudent scoring ";
			ScoreTests("Junior", studentJR, keyJR, tiesJR);
			ScoreTests("Senior", studentSR, keySR, tiesSR);
			cout << "complete.";
			cout << "\nRanking ";
			Rank("Junior", studentJR);
			Rank("Senior", studentSR);
			cout << "complete.";
			IndividualAwards(studentJR, studentSR);
			cout << "\nIndividual award winners added to script.";
			cout << "\nSorting students ";
			Arrange("Junior", studentJR);
			Arrange("Senior", studentSR);
			cout << "complete.";
			cout << "\nTeam scoring ";
			SchoolScores("Junior", studentJR, teamJR);
			SchoolScores("Senior", studentSR, teamSR);
			cout << "complete.";
			cout << "\nGraphing ";
			Graphs("Junior", studentJR, teamJR);
			Graphs("Senior", studentSR, teamSR);
			cout << "complete.";
			TeamAwards(teamJR, teamSR);
			cout << "\nTeam award winners added to script.";
			cout << "\n\nScoring and report generation completed.";
			break;
		case 'V':
			screen.SetForeColor(YELLOW);
			Validate("Junior", studentJR, keyJR, tiesJR);
			Validate("Senior", studentSR, keySR, tiesSR);
			cout << "\nValidation has ended.\n";
			break;
		case 'X':
			finished = true;
			break;
		default:
			cout << '\007' << flush;
		}
	} while (!finished);
}
