#include "implementation.cpp"

int main() {
	const char url[] = "http://www.example.com/";
	const char outfilename[] = "httpget.txt";

	return getHTML(url, outfilename);
}