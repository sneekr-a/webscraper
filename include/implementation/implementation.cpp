#include "curl/curl.h"
#include "implementation.h"
#include <stdio.h>
#include <string>


//^.h, line 8
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
	curl_easy_setopt(curl, CURLOPT_WRITEDATA, fp);

	//--perform the curl and pull html data
	if ((code = curl_easy_perform(curl)) != CURLE_OK) {
		fprintf(stderr, curl_easy_strerror(code));
		return EXIT_FAILURE;
	}


	//--perform cleanup
	curl_easy_cleanup(curl);
	fclose(fp);

	return EXIT_SUCCESS;
}