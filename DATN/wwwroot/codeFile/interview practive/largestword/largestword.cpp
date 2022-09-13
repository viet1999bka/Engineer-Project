#include <bits/stdc++.h>
using namespace std;

string solution(string str);
int main(int argc, char** argv) {
	string str ="";
	for(int i = 1;; i++){
		if(argv[i] != NULL){
			string tmp(argv[i]);
			str += " " + tmp;
		}
		else{
			break;
		}
	}
	cout << solution(str);
}

