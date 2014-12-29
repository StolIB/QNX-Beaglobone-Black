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

float Kp = 1, Ti = 1, Td = 0;
double T = 2;
int k = 1, x = 0, _x = 0, rec = 0;
float y = 0, y1 = 0, e = 0, ep = 0, de = 0, ie = 0, st = 0;
char _y[100];


struct timespec PIDstart, PIDstop, OBJstart, OBJstop, start, stop;

pthread_t threads[3];
pthread_mutex_t mutex;

void *inertial(void){
	double t;
	clock_gettime(CLOCK_REALTIME, &OBJstart);
	while(1){
		clock_gettime(CLOCK_REALTIME, &OBJstop);
		t = (OBJstop.tv_sec - OBJstart.tv_sec)*1000 + (OBJstop.tv_nsec-OBJstart.tv_nsec)/1000000;
		if (t >= 10){
			clock_gettime(CLOCK_REALTIME, &OBJstart);
			pthread_mutex_lock(&mutex);
			y = (1/(T*100))*(k*st-y1)+y1;
			y1 = y;
			pthread_mutex_unlock(&mutex);
		}
	}
}
void *pid(void){
	double t;
	clock_gettime(CLOCK_REALTIME, &PIDstart);
	while(1){
		clock_gettime(CLOCK_REALTIME, &PIDstop);
		t = (PIDstop.tv_sec - PIDstart.tv_sec)*1000 + (PIDstop.tv_nsec-PIDstart.tv_nsec)/1000000;
		if (t >= 10){ //t>=10ms
			clock_gettime(CLOCK_REALTIME, &PIDstart);
			pthread_mutex_lock(&mutex);
			e = x - y;
			ie += e;
			de = e - ep;
			st = Kp*(e+Ti*ie*0.01+Td*de*100);
			ep = e;
			pthread_mutex_unlock(&mutex);
		}
	}
}
void *doprocessing (int sock)
{
	while(1){
		int n;
		char buffer[256];
		char buf[256];
		bzero(buf, 256);
		bzero(buffer,256);

		n = read(sock, &_x, 256, 0);
		if (n < 0){
			perror("ERROR reading from socket");
			return NULL;
		}

		rec = ntohl(_x);
		pthread_mutex_lock(&mutex);
		if (rec >= 500000){
			Td = (float)(rec-500000)/100;
			//printf("Td: %f rec: %d\n", Td, rec);
		}
		else if (rec >= 300000){
			Ti = (float)(rec-300000)/100;
			//printf("Ti: %f rec: %d\n", Ti, rec);
		}
		else if (rec >= 100000){
			Kp = (float)(rec-100000)/100;
			//printf("Kp: %f rec: %d\n", Kp, rec);
		}
		else {
			x = rec;
		}

		itoa((int)(y*100), _y, 10);
		n = send(sock, &_y, 10, 0);
		pthread_mutex_unlock(&mutex);
		if (n < 0){
			perror("ERROR writing to socket");
			return NULL;
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
    pthread_create(&threads[OBJ], &attr, inertial, NULL);
    pthread_create(&threads[PID], &attr, pid, NULL);
    while (1){
		newsockfd = accept(sockfd, (struct sockaddr *) &cli_addr, &clilen);
		if (newsockfd < 0){
			perror("ERROR on accept");
			exit(1);
		}
		y = 0;
		y1 = 0;
		e = 0;
		ep = 0;
		de = 0;
		ie = 0;
		st = 0;

		pthread_create(&threads[PROC], &attr, doprocessing, newsockfd);
		pthread_join(threads[PROC], (void **)&status);


    }
    pthread_mutex_destroy(&mutex);
	pthread_attr_destroy(&attr);
    return 0;
}
