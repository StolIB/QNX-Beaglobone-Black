#include <stdio.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <pthread.h>
#include <sched.h>
#include <time.h>

#define OBJ 0
#define PROC 1
#define PID 2

float Kp = 1.5, Ti = 0.3, Td = 1.5;
double T = 10;
int k = 1, x = 0, _x = 0;
float y = 0, y1 = 0, e = 0, e0 = 0, de = 0, ie = 0, st = 0;
char _y[100];

pthread_t threads[3];
pthread_mutex_t mutex;

void *inertial(void){
	while(1){
		pthread_mutex_lock(&mutex);
		y = (1/T)*(k*st-y1)+y1;
		y1 = y;
		pthread_mutex_unlock(&mutex);
		delay(50);
	}
}
void *pid(void){
	while(1){
		pthread_mutex_lock(&mutex);
		e = x - y;
		ie += e;
		de = e - e0;
		e0 = e;
		st = Kp*(e+Ti*ie+Td*de);
		pthread_mutex_unlock(&mutex);
		delay(50);
	}
}
void *doprocessing (int sock)
{
	while(1){
		int n;
		char buffer[256];
		bzero(buffer,256);

		n = read(sock, &_x, 4, 0);
		if (n < 0){
			perror("ERROR reading from socket");
			exit(1);
		}

		x = ntohl(_x);
		itoa((int)(y*100), _y, 10);
		n = send(sock, &_y, 10, 0);

		if (n < 0){
			perror("ERROR writing to socket");
			exit(1);
		}
	}
}

int main( int argc, char *argv[] )
{
	int status;
	pthread_attr_t attr;
	pthread_attr_init( &attr );
	pthread_attr_setdetachstate(&attr, PTHREAD_CREATE_JOINABLE);
	pthread_mutex_init(&mutex, NULL);

	int sockfd, newsockfd, portno, clilen;
    char buffer[256];
    struct sockaddr_in serv_addr, cli_addr;

    sockfd = socket(AF_INET, SOCK_STREAM, 0);
    if (sockfd < 0)
    {
        perror("ERROR opening socket");
        exit(1);
    }
    /* Initialize socket structure */
    bzero((char *) &serv_addr, sizeof(serv_addr));
    portno = 5001;
    serv_addr.sin_family = AF_INET;
    serv_addr.sin_addr.s_addr = INADDR_ANY;
    serv_addr.sin_port = htons(portno);

    /* Now bind the host address using bind() call.*/
    if (bind(sockfd, (struct sockaddr *) &serv_addr, sizeof(serv_addr)) < 0){
         perror("ERROR on binding");
         exit(1);
    }

    listen(sockfd,5);
    clilen = sizeof(cli_addr);

    //while (1){
	newsockfd = accept(sockfd, (struct sockaddr *) &cli_addr, &clilen);
	if (newsockfd < 0){
		perror("ERROR on accept");
		exit(1);
	}
	pthread_create(&threads[OBJ], &attr, inertial, NULL);
	pthread_create(&threads[PROC], &attr, doprocessing, newsockfd);
	pthread_create(&threads[PID], &attr, pid, NULL);
	pthread_join(threads[PROC], (void **)&status);
	pthread_mutex_destroy(&mutex);
	pthread_attr_destroy(&attr);

        /* Create child process */
       // while(1)
            	//doprocessing(newsockfd);
            //exit(0);

    //} /* end of while */
    return 0;
}
