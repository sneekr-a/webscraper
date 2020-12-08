#include "curl/curl.h"
#include <stdio.h>
#include <string>


/*
Function getHTML takes a string url and writes the HTML code to a string outfilename, which is the name of the output file
*/
int getHTML(const char url[], const char outfilename[]) {

	//--def and init
	CURL* curl = curl_easy_init();
	FILE* fp;

	//--setup

	//-open file
	fp = fopen(outfilename, "w");
	if (!fp) {
		fprintf(stderr, "file open failed");
		return EXIT_FAILURE;
	}

	//-set options
	CURLcode code = CURLE_OK;

	//set url option
	if ((code = curl_easy_setopt(curl, CURLOPT_URL, url)) != CURLE_OK) {
		fprintf(stderr, curl_easy_strerror(code));
		return EXIT_FAILURE;
	}

	//set writefile option
	//(error checking not necessary)
	curl_easy_setopt(curl, CURLOPT_WRITEDATA, fp);

	//--perform the curl
	if ((code = curl_easy_perform(curl)) != CURLE_OK) {
		fprintf(stderr, curl_easy_strerror(code));
		return EXIT_FAILURE;
	}


	//--perform cleanup
	curl_easy_cleanup(curl);
	fclose(fp);

	return EXIT_SUCCESS;
}